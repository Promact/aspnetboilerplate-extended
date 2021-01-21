using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace AspnetBoilerplateExtended.Controllers
{
    public abstract class AspnetBoilerplateExtendedControllerBase: AbpController
    {
        protected AspnetBoilerplateExtendedControllerBase()
        {
            LocalizationSourceName = AspnetBoilerplateExtendedConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
