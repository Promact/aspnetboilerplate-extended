using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.EntityFrameworkCore;
using Abp.Extensions;
using Abp.IdentityFramework;
using Abp.Linq.Extensions;
using Abp.Localization;
using Abp.Runtime.Session;
using Abp.UI;
using BoilerPlateDemo_App.Authorization;
using BoilerPlateDemo_App.Authorization.Accounts;
using BoilerPlateDemo_App.Authorization.Roles;
using BoilerPlateDemo_App.Authorization.Users;
using BoilerPlateDemo_App.EntityFrameworkCore;
using BoilerPlateDemo_App.Roles.Dto;
using BoilerPlateDemo_App.Users.Dto;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;

namespace BoilerPlateDemo_App.Users
{
    
    public class UserAppService : AsyncCrudAppService<User, UserDto, long, PagedUserResultRequestDto, CreateUserDto, UserDto>, IUserAppService
    {
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly IRepository<Role> _roleRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IAbpSession _abpSession;
        private readonly LogInManager _logInManager;
        private readonly IDbContextProvider<BoilerPlateDemo_AppDbContext> _context;
        private readonly IConfiguration _configuration;

        public UserAppService(
            IRepository<User, long> repository,
            UserManager userManager,
            RoleManager roleManager,
            IRepository<Role> roleRepository,
            IPasswordHasher<User> passwordHasher,
            IAbpSession abpSession,
            LogInManager logInManager,
            IDbContextProvider<BoilerPlateDemo_AppDbContext> context,
            IConfiguration configuration)
            : base(repository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _roleRepository = roleRepository;
            _passwordHasher = passwordHasher;
            _abpSession = abpSession;
            _logInManager = logInManager;
            _context = context;
            _configuration = configuration;
        }

        public async Task SendMail(string DisplayName, string emailSubject, string emailBody, string emailAddress)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(DisplayName, _configuration["MailSetting:MailId"]));
                message.To.Add(MailboxAddress.Parse(emailAddress));
                message.Subject = emailSubject;
                message.Body = new TextPart(TextFormat.Html) { Text = emailBody };
                using var smtp = new MailKit.Net.Smtp.SmtpClient();
                smtp.Connect(_configuration["MailSetting:Host"], int.Parse(_configuration["MailSetting:Port"]), SecureSocketOptions.StartTlsWhenAvailable);
                smtp.Authenticate(_configuration["MailSetting:Username"], _configuration["MailSetting:Password"]);
                smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;
                await smtp.SendAsync(message);
                smtp.Disconnect(true);
            }
            catch (Exception exception)
            {
                Logger.Info(exception.Message, exception);
            }
        }

        public async Task<string> GetEmailOfUserForResetPassword(SendResetPasswordLinkDto sendResetPasswordLinkDto)
        {
            var resetCode = await SendResetPasswordLink<SendResetPasswordLinkDto>(sendResetPasswordLinkDto);
            return resetCode;
        }

        /// <summary>
        /// Method for sending reset link to email
        /// </summary>
        /// <param name="email">email of the user</param>
        /// <returns>passwordResetCode</returns>
        public async Task<string> SendResetPasswordLink<T>(T input) where T : class
        {
            var email = (string)typeof(T).GetProperty("Email").GetValue(input);
            if (!new Regex(AccountAppService.EmailRegex).IsMatch(email))
            {
                throw new UserFriendlyException(AppConsts.InvalidEmail);
            }
            try
            {
                var result = _configuration.GetSection("MailSetting").GetChildren().ToList();
                var values = result.ToDictionary(x => x.Key);

                var user = await _userManager.FindByEmailAsync(email);
                var host = values["Host"].Value;
                var mailId = values["MailId"].Value;
                var password = values["Password"].Value;
                var port = values["Port"].Value;
                var resetLink = _configuration["App:ClientRootAddress"];
                var route = AppConsts.Route;
                var emailSubject = AppConsts.ResetPasswordMailSubject;
                if (user != null)
                {
                    var guid = Guid.NewGuid();
                    user.PasswordResetCode = guid.ToString();
                    await _userManager.UpdateAsync(user);
                    var emailBody = "Hi <b>" + user.Name + " " + user.Surname + "</b>,<br><br>" + AppConsts.ResetPasswordMailBody3 + "<br><br>" + AppConsts.ResetPasswordMailBody1 + "<br>" + AppConsts.ResetPasswordMailBody2 + "<br><br><a class='btn btn-primary text-white' href=" + resetLink + route + user.PasswordResetCode + ">" + AppConsts.ResetPasswordMailLinkTitle + "</a>. <br><br>" + AppConsts.CheersMessage + " <br><br>" + AppConsts.CetAtTeamMessage;
                    await SendMail("CET AT", emailSubject, emailBody, email);
                    return user.PasswordResetCode.ToString();
                }
                throw new UserFriendlyException(AppConsts.UserNotExistMessage);
            }

            catch (Exception exception)
            {
                Logger.Info(exception.Message, exception);
                if (exception is UserFriendlyException)
                {
                    throw new UserFriendlyException(AppConsts.UserNotExistMessage);
                }
            }
            return "";
        }

        /// <summary>
        /// Method for resetting password
        /// </summary>
        /// <param name="input">ResetPassword DTO</param>
        /// <returns>bool</returns>
        public async Task<bool> ResetPasswordOfUser(ResetPasswordFromLinkDto resetPasswordFromLinkDto)
        {
            var response = await ResetPasswordFromLink<ResetPasswordFromLinkDto>(resetPasswordFromLinkDto);
            return response;
        }
        /// <summary>
        /// Method for resetting password
        /// </summary>
        /// <param name="input">ResetPassword DTO</param>
        /// <returns>bool</returns>
        public async Task<bool> ResetPasswordFromLink<T>(T input) where T : class
        {
            var newPassword = (string)typeof(T).GetProperty("NewPassword").GetValue(input);
            var passwordToken = (string)typeof(T).GetProperty("PasswordToken").GetValue(input);
            if (!new Regex(AccountAppService.PasswordRegex).IsMatch(newPassword))
            {
                throw new UserFriendlyException(AppConsts.InvalidPasswordMessage);
            }
            var cn = _context.GetDbContext();
            var user = await cn.Users.FirstOrDefaultAsync(x => x.PasswordResetCode == passwordToken);
            if (user != null)
            {
                var reslut = await _userManager.ChangePasswordAsync(user, newPassword);
                if (reslut.Succeeded)
                {
                    user.PasswordResetCode = Guid.NewGuid().ToString();
                    await _userManager.UpdateAsync(user);
                    return true;
                }
            }
            throw new UserFriendlyException(AppConsts.AuthenticationFailedMessage);
        }

        [AbpAuthorize(PermissionNames.Pages_Users)]
        public override async Task<UserDto> CreateAsync(CreateUserDto input)
        {
            CheckCreatePermission();

            var user = ObjectMapper.Map<User>(input);

            user.TenantId = AbpSession.TenantId;
            user.IsEmailConfirmed = true;

            await _userManager.InitializeOptionsAsync(AbpSession.TenantId);

            CheckErrors(await _userManager.CreateAsync(user, input.Password));

            if (input.RoleNames != null)
            {
                CheckErrors(await _userManager.SetRolesAsync(user, input.RoleNames));
            }

            CurrentUnitOfWork.SaveChanges();

            return MapToEntityDto(user);
        }
        [AbpAuthorize(PermissionNames.Pages_Users)]
        public override async Task<UserDto> UpdateAsync(UserDto input)
        {
            CheckUpdatePermission();

            var user = await _userManager.GetUserByIdAsync(input.Id);

            MapToEntity(input, user);

            CheckErrors(await _userManager.UpdateAsync(user));

            if (input.RoleNames != null)
            {
                CheckErrors(await _userManager.SetRolesAsync(user, input.RoleNames));
            }

            return await GetAsync(input);
        }
        [AbpAuthorize(PermissionNames.Pages_Users)]
        public override async Task DeleteAsync(EntityDto<long> input)
        {
            var user = await _userManager.GetUserByIdAsync(input.Id);
            await _userManager.DeleteAsync(user);
        }

        [AbpAuthorize(PermissionNames.Pages_Users_Activation)]
        public async Task Activate(EntityDto<long> user)
        {
            await Repository.UpdateAsync(user.Id, async (entity) =>
            {
                entity.IsActive = true;
            });
        }

        [AbpAuthorize(PermissionNames.Pages_Users_Activation)]
        public async Task DeActivate(EntityDto<long> user)
        {
            await Repository.UpdateAsync(user.Id, async (entity) =>
            {
                entity.IsActive = false;
            });
        }
        [AbpAuthorize(PermissionNames.Pages_Users)]
        public async Task<ListResultDto<RoleDto>> GetRoles()
        {
            var roles = await _roleRepository.GetAllListAsync();
            return new ListResultDto<RoleDto>(ObjectMapper.Map<List<RoleDto>>(roles));
        }
        [AbpAuthorize(PermissionNames.Pages_Users)]
        public async Task ChangeLanguage(ChangeUserLanguageDto input)
        {
            await SettingManager.ChangeSettingForUserAsync(
                AbpSession.ToUserIdentifier(),
                LocalizationSettingNames.DefaultLanguage,
                input.LanguageName
            );
        }
        [AbpAuthorize(PermissionNames.Pages_Users)]
        protected override User MapToEntity(CreateUserDto createInput)
        {
            var user = ObjectMapper.Map<User>(createInput);
            user.SetNormalizedNames();
            return user;
        }
        [AbpAuthorize(PermissionNames.Pages_Users)]
        protected override void MapToEntity(UserDto input, User user)
        {
            ObjectMapper.Map(input, user);
            user.SetNormalizedNames();
        }
        [AbpAuthorize(PermissionNames.Pages_Users)]
        protected override UserDto MapToEntityDto(User user)
        {
            var roleIds = user.Roles.Select(x => x.RoleId).ToArray();

            var roles = _roleManager.Roles.Where(r => roleIds.Contains(r.Id)).Select(r => r.NormalizedName);

            var userDto = base.MapToEntityDto(user);
            userDto.RoleNames = roles.ToArray();

            return userDto;
        }
        [AbpAuthorize(PermissionNames.Pages_Users)]
        protected override IQueryable<User> CreateFilteredQuery(PagedUserResultRequestDto input)
        {
            return Repository.GetAllIncluding(x => x.Roles)
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.UserName.Contains(input.Keyword) || x.Name.Contains(input.Keyword) || x.EmailAddress.Contains(input.Keyword))
                .WhereIf(input.IsActive.HasValue, x => x.IsActive == input.IsActive);
        }
        [AbpAuthorize(PermissionNames.Pages_Users)]
        protected override async Task<User> GetEntityByIdAsync(long id)
        {
            var user = await Repository.GetAllIncluding(x => x.Roles).FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                throw new EntityNotFoundException(typeof(User), id);
            }

            return user;
        }
        [AbpAuthorize(PermissionNames.Pages_Users)]
        protected override IQueryable<User> ApplySorting(IQueryable<User> query, PagedUserResultRequestDto input)
        {
            return query.OrderBy(r => r.UserName);
        }
        [AbpAuthorize(PermissionNames.Pages_Users)]
        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
        [AbpAuthorize(PermissionNames.Pages_Users)]
        public async Task<bool> ChangePassword(ChangePasswordDto input)
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("Please log in before attemping to change password.");
            }
            long userId = _abpSession.UserId.Value;
            var user = await _userManager.GetUserByIdAsync(userId);
            var loginAsync = await _logInManager.LoginAsync(user.UserName, input.CurrentPassword, shouldLockout: false);
            if (loginAsync.Result != AbpLoginResultType.Success)
            {
                throw new UserFriendlyException("Your 'Existing Password' did not match the one on record.  Please try again or contact an administrator for assistance in resetting your password.");
            }
            if (!new Regex(AccountAppService.PasswordRegex).IsMatch(input.NewPassword))
            {
                throw new UserFriendlyException("Passwords must be at least 8 characters, contain a lowercase, uppercase, and number.");
            }
            user.Password = _passwordHasher.HashPassword(user, input.NewPassword);
            CurrentUnitOfWork.SaveChanges();
            return true;
        }
        [AbpAuthorize(PermissionNames.Pages_Users)]
        public async Task<bool> ResetPassword(ResetPasswordDto input)
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("Please log in before attemping to reset password.");
            }
            long currentUserId = _abpSession.UserId.Value;
            var currentUser = await _userManager.GetUserByIdAsync(currentUserId);
            var loginAsync = await _logInManager.LoginAsync(currentUser.UserName, input.AdminPassword, shouldLockout: false);
            if (loginAsync.Result != AbpLoginResultType.Success)
            {
                throw new UserFriendlyException("Your 'Admin Password' did not match the one on record.  Please try again.");
            }
            if (currentUser.IsDeleted || !currentUser.IsActive)
            {
                return false;
            }
            var roles = await _userManager.GetRolesAsync(currentUser);
            if (!roles.Contains(StaticRoleNames.Tenants.Admin))
            {
                throw new UserFriendlyException("Only administrators may reset passwords.");
            }

            var user = await _userManager.GetUserByIdAsync(input.UserId);
            if (user != null)
            {
                user.Password = _passwordHasher.HashPassword(user, input.NewPassword);
                CurrentUnitOfWork.SaveChanges();
            }

            return true;
        }
    }
}

