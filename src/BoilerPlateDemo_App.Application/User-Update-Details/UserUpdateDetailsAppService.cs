using BoilerPlateDemo_App.Authorization.Users;
using BoilerPlateDemo_App.User_Update_Details.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BoilerPlateDemo_App.User_Update_Details
{
    public class UserUpdateDetailsAppService : BoilerPlateDemo_AppAppServiceBase, IUserUpdateDetailsAppService
    {
        private readonly UserManager _userManager;
        public UserUpdateDetailsAppService(UserManager userManager)
        {
            _userManager = userManager;
        }
        /// <summary>
        /// Method for Getting User Details
        /// </summary>
        /// <param name="id">Current User Id</param>
        /// <returns>Details of User</returns>
        public async Task<UserUpdateDetailDto> GetUserDetailsAsync(string id)
        {
            var user = await _userManager.GetUserByIdAsync(long.Parse(id));
            UserUpdateDetailDto updateDetailDto = new Dto.UserUpdateDetailDto
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                UserName = user.UserName,
                EmailAddress = user.EmailAddress
            };
            return updateDetailDto;
        }

        /// <summary>
        /// Method for Updating User Details
        /// </summary>
        /// <param name="input">UpdateDetailDto </param>
        /// <returns></returns>
        public async Task UpdateUser<T>(T input) where T : class   
        {
            var id = (long)(typeof(T).GetProperty("Id").GetValue(input));
            var Name = typeof(T).GetProperty("Name").GetValue(input);
            var Surname = typeof(T).GetProperty("Surname").GetValue(input);
            var UserName = typeof(T).GetProperty("UserName").GetValue(input);
            var EmailAddress = typeof(T).GetProperty("EmailAddress").GetValue(input);
            var getUser = await _userManager.GetUserByIdAsync(id);
            getUser.Name = (string)Name;
            getUser.Surname = (string)Surname;
            getUser.UserName = (string)UserName;
            getUser.EmailAddress = (string)EmailAddress;
            CheckErrors(await _userManager.UpdateAsync(getUser));
        }
        public async Task UpdateUserDetails(UserUpdateDetailDto updateDetailDto)
        {
            await UpdateUser<UserUpdateDetailDto>(updateDetailDto);
        }
    }
}
