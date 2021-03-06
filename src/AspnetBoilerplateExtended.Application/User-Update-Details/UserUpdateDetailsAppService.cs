﻿using AspnetBoilerplateExtended.Authorization.Users;
using AspnetBoilerplateExtended.User_Update_Details.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspnetBoilerplateExtended.User_Update_Details
{
    public class UserUpdateDetailsAppService : AspnetBoilerplateExtendedAppServiceBase,IUserUpdateDetailsAppService
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
        /// Generic Method for Updating User Details
        /// </summary>
        /// <param name="updateUserDetailModel">UpdateDetailDto </param>
        /// <returns>Update user detail</returns>
        public async Task UpdateUser<T>(T updateUserDetailModel) where T : class
        {
            var id = (long)(typeof(T).GetProperty("Id").GetValue(updateUserDetailModel));
            var Name = typeof(T).GetProperty("Name").GetValue(updateUserDetailModel);
            var Surname = typeof(T).GetProperty("Surname").GetValue(updateUserDetailModel);
            var UserName = typeof(T).GetProperty("UserName").GetValue(updateUserDetailModel);
            var EmailAddress = typeof(T).GetProperty("EmailAddress").GetValue(updateUserDetailModel);
            var getUser = await _userManager.GetUserByIdAsync(id);
            getUser.Name = (string)Name;
            getUser.Surname = (string)Surname;
            getUser.UserName = (string)UserName;
            getUser.EmailAddress = (string)EmailAddress;
            CheckErrors(await _userManager.UpdateAsync(getUser));
        }
        /// <summary>
        /// Method for Updating User Details
        /// </summary>
        /// <param name="updateDetailDto"></param>
        /// <returns>Update user detail</returns>
        public async Task UpdateUserDetails(UserUpdateDetailDto updateDetailDto)
        {
            await UpdateUser<UserUpdateDetailDto>(updateDetailDto);
        }
    }
}
