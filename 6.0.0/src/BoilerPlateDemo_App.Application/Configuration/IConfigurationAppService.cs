using System.Threading.Tasks;
using BoilerPlateDemo_App.Configuration.Dto;

namespace BoilerPlateDemo_App.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
