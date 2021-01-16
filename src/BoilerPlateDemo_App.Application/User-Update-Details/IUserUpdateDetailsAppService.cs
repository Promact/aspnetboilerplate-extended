using BoilerPlateDemo_App.User_Update_Details.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BoilerPlateDemo_App.User_Update_Details
{
    public interface IUserUpdateDetailsAppService
    {
        /// <summary>
        /// Method for Getting User Details
        /// </summary>
        /// <param name="id">Current User Id</param>
        /// <returns>Details of User</returns>
        Task<UserUpdateDetailDto> GetUserDetailsAsync(string id);

        /// <summary>
        /// Method for Updating User Details
        /// </summary>
        /// <param name="input">UpdateDetailDto </param>
        /// <returns></returns>
        Task UpdateUser<T>(T input) where T : class;
    }
}
