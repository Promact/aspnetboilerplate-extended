using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Reflection.Extensions;

namespace AspnetBoilerplateExtended.Localization
{
    public static class AspnetBoilerplateExtendedLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(AspnetBoilerplateExtendedConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(AspnetBoilerplateExtendedLocalizationConfigurer).GetAssembly(),
                        "AspnetBoilerplateExtended.Localization.SourceFiles"
                    )
                )
            );
        }
    }
}
