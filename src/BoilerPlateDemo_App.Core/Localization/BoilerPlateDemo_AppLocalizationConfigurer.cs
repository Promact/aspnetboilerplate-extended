using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Reflection.Extensions;

namespace BoilerPlateDemo_App.Localization
{
    public static class BoilerPlateDemo_AppLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(BoilerPlateDemo_AppConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(BoilerPlateDemo_AppLocalizationConfigurer).GetAssembly(),
                        "BoilerPlateDemo_App.Localization.SourceFiles"
                    )
                )
            );
        }
    }
}
