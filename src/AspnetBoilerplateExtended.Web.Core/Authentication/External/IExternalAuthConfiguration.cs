using System.Collections.Generic;

namespace AspnetBoilerplateExtended.Authentication.External
{
    public interface IExternalAuthConfiguration
    {
        List<ExternalLoginProviderInfo> Providers { get; }
    }
}
