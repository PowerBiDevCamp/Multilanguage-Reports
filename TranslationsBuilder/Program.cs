using System;
using System.Collections.Generic;
using TranslationsBuilder.Models;
using TranslationsBuilder.Services;

namespace TranslationsBuilder {

  class Program {

    static void Main() {

      TranslationSet translationSet = new TranslationSet {
        DefaultLangauge = SupportedLanguages.English,
        SecondaryLanguages = new List<Language>() {
          SupportedLanguages.Spanish,
          SupportedLanguages.French,
          SupportedLanguages.German,
          SupportedLanguages.Dutch,
          SupportedLanguages.Hebrew,
          SupportedLanguages.Russian,
          SupportedLanguages.Japanese,
          SupportedLanguages.Chinese
        }
      };

      TranslationsManager.PopulateDefaultCultureTranslations();
      foreach (var language in translationSet.SecondaryLanguages) {
        TranslationsManager.PopulateTranslations(language.LanguageTag);
      }

      TranslationsManager.ExportTranslations(translationSet);

      // CountryLookupTableGenerator.GenerateCountryLookup(translationSet);

      Console.WriteLine();
      Console.WriteLine("Press ENTER to continue");
      Console.ReadLine();

    }


  }


}
