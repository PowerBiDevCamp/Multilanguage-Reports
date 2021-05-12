using System;

namespace TranslationsBuilder.Models {

  class SupportedLanguages {

    static public Language English = new Language {
      LanguageTag = "en-US",
      TranslationId = "en",
      TranslationGroup = "English",
      DisplayName = "English (United States)",
      NativeName = "English (United States)"
    };

    static public Language Spanish = new Language {
      LanguageTag = "es-ES",
      TranslationId = "es",
      TranslationGroup = "Spanish",
      DisplayName = "Spanish (Spain, International Sort)",
      NativeName = "español (España, alfabetización internacional)"
    };

    static public Language German = new Language {
      LanguageTag = "de-DE",
      TranslationId = "de",
      TranslationGroup = "German",
      DisplayName = "German (Germany)",
      NativeName = "Deutsch (Deutschland)"
    };

    static public Language French = new Language {
      LanguageTag = "fr-FR",
      TranslationId = "fr",
      TranslationGroup = "French",
      DisplayName = "French (France)",
      NativeName = "français (France)"
    };

    static public Language Hebrew = new Language {
      LanguageTag = "he-IL",
      TranslationId = "he",
      TranslationGroup = "Hebrew",
      DisplayName = "Hebrew (Israel)",
      NativeName = "עברית (ישראל)"
    };

    static public Language Japanese = new Language {
      LanguageTag = "ja-JP",
      TranslationId = "ja",
      TranslationGroup = "Japanese",
      DisplayName = "Japanese (Japan)",
      NativeName = "日本語 (日本)"
    };

    static public Language Dutch = new Language {
      LanguageTag = "nl-NL",
      TranslationId = "nl",
      TranslationGroup = "Dutch",
      DisplayName = "Dutch (Netherlands)",
      NativeName = "Nederlands (Nederland)"
    };

    static public Language Russian = new Language {
      LanguageTag = "ru-RU",
      TranslationId = "ru",
      TranslationGroup = "Russian",
      DisplayName = "Russian (Russia)",
      NativeName = "русский (Россия)"
    };

    static public Language Chinese = new Language {
      LanguageTag = "zh-CN",
      TranslationId = "zh-Hans",
      TranslationGroup = "Chinese",
      DisplayName = "Chinese (China)",
      NativeName = "中文(中国)"
    };

    static public Language Irish = new Language {
      LanguageTag = "ga-IE",
      TranslationId = "ga",
      TranslationGroup = "Irish",
      DisplayName = "Irish (Ireland)",
      NativeName = "Gaeilge (Éire)"
    };

  }
}
