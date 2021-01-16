using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using BoilerPlateDemo_App.Configuration.Dto;

namespace BoilerPlateDemo_App.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : BoilerPlateDemo_AppAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
