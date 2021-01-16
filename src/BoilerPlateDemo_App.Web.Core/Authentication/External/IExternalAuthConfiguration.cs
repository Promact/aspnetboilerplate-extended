using System.Collections.Generic;

namespace BoilerPlateDemo_App.Authentication.External
{
    public interface IExternalAuthConfiguration
    {
        List<ExternalLoginProviderInfo> Providers { get; }
    }
}
