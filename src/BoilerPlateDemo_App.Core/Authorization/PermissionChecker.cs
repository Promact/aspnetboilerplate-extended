using Abp.Authorization;
using BoilerPlateDemo_App.Authorization.Roles;
using BoilerPlateDemo_App.Authorization.Users;

namespace BoilerPlateDemo_App.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
