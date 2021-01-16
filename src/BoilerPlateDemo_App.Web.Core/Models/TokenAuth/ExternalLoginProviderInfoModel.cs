using Abp.AutoMapper;
using BoilerPlateDemo_App.Authentication.External;

namespace BoilerPlateDemo_App.Models.TokenAuth
{
    [AutoMapFrom(typeof(ExternalLoginProviderInfo))]
    public class ExternalLoginProviderInfoModel
    {
        public string Name { get; set; }

        public string ClientId { get; set; }
    }
}
