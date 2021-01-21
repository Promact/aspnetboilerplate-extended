using Abp.Authorization;
using AspnetBoilerplateExtended.Authorization.Roles;
using AspnetBoilerplateExtended.Authorization.Users;

namespace AspnetBoilerplateExtended.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
