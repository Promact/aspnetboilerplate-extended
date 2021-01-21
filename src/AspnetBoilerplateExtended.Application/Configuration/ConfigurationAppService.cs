using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using AspnetBoilerplateExtended.Configuration.Dto;

namespace AspnetBoilerplateExtended.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : AspnetBoilerplateExtendedAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
