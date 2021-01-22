using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;

namespace AspnetBoilerplateExtended.Authorization
{
    public class AspnetBoilerplateExtendedAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            context.CreatePermission(PermissionNames.Pages_Users, L("Users"));
            context.CreatePermission(PermissionNames.Pages_Users_Activation, L("UsersActivation"));
            context.CreatePermission(PermissionNames.Pages_Roles, L("Roles"));
            context.CreatePermission(PermissionNames.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);

            #region application

            var app = context.CreatePermission(PermissionNames.Pages_Applications, L("Applications"));
            app.CreateChildPermission(PermissionNames.Pages_Applications_Create, L("Applications.Create"));
            app.CreateChildPermission(PermissionNames.Pages_Applications_Edit, L("Applications.Edit"));
            app.CreateChildPermission(PermissionNames.Pages_Applications_Delete, L("Applications.Delete"));
            app.CreateChildPermission(PermissionNames.Pages_Applications_View, L("Applications.View"));
            #endregion
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, AspnetBoilerplateExtendedConsts.LocalizationSourceName);
        }
    }
}
