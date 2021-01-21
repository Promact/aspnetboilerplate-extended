using AspnetBoilerplateExtended.User_Update_Details.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspnetBoilerplateExtended.User_Update_Details
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
        /// Generic Method for Updating User Details
        /// </summary>
        /// <param name="updateUserDetailModel">UpdateDetailDto </param>
        /// <returns>Update user detail</returns>
        Task UpdateUser<T>(T updateUserDetailModel) where T : class;
    }
}
