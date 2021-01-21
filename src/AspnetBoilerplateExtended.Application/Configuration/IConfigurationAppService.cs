using System.Threading.Tasks;
using AspnetBoilerplateExtended.Configuration.Dto;

namespace AspnetBoilerplateExtended.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
