using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace BoilerPlateDemo_App.Controllers
{
    public abstract class BoilerPlateDemo_AppControllerBase: AbpController
    {
        protected BoilerPlateDemo_AppControllerBase()
        {
            LocalizationSourceName = BoilerPlateDemo_AppConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
