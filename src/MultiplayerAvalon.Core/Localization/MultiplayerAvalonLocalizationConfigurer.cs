using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Reflection.Extensions;

namespace MultiplayerAvalon.Localization
{
    public static class MultiplayerAvalonLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(MultiplayerAvalonConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(MultiplayerAvalonLocalizationConfigurer).GetAssembly(),
                        "MultiplayerAvalon.Localization.SourceFiles"
                    )
                )
            );
        }
    }
}
