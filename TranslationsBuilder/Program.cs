using System;
using System.Collections.Generic;
using TranslationsBuilder.Models;
using TranslationsBuilder.Services;

namespace TranslationsBuilder {

  class Program {

    static string consoleDelimeter = "╠═══════════════════════════════════════════════════════════════════════════════╣";
    static string consoleHeader = "║";

    static string appFolder = Environment.CurrentDirectory + "\\";



    static void myConsoleWriteLine(string s) {
      Console.WriteLine("{0,0}  {1,-76} {0,0}", consoleHeader, s);
    }

    static void Main(string[] args) {

      if (args.Length > 0) {
        AppSettings.connectString = args[0];
      }


      string userInput = "";
      while (userInput != "0") {
        Console.WriteLine();
        Console.WriteLine(consoleDelimeter);
        myConsoleWriteLine($"Server  : {AppSettings.connectString}");
        Console.WriteLine(consoleDelimeter);
        myConsoleWriteLine($"    0   Exit");
        myConsoleWriteLine($"");
        myConsoleWriteLine($"    1   Generate Metadata Tranlations (5 languages)");
        myConsoleWriteLine($"    2   Generate Metadata Tranlations (12 languages)");

        Console.WriteLine(consoleDelimeter);
        Console.WriteLine();

        Console.CursorVisible = true;
        Console.CursorSize = 100;     // Emphasize the cursor.
        userInput = Console.ReadLine();

        switch (userInput) {
          case "1":
            GenerateMetadataTranlations();
            break;
          case "2":
            GenerateMetadataTranlationsAllLanguages();
            break;
          default:
            Console.WriteLine($"You chose option: {userInput}");
            break;
        }
      }

    }

    static void GenerateMetadataTranlations() {

      TranslationSet translationSet = new TranslationSet {
        DefaultLangauge = SupportedLanguages.English,
        SecondaryLanguages = new List<Language>() {
          SupportedLanguages.Spanish,
          SupportedLanguages.French,
         SupportedLanguages.German,
          SupportedLanguages.Dutch
        }
      };

      TranslationsManager.PopulateDefaultCultureTranslations();
      foreach (var language in translationSet.SecondaryLanguages) {
        TranslationsManager.PopulateTranslations(language.LanguageTag);
      }

      TranslationsManager.ExportTranslations(translationSet);

    }

    static void GenerateMetadataTranlationsAllLanguages() {

      TranslationSet translationSet = new TranslationSet {
        DefaultLangauge = SupportedLanguages.English,
        SecondaryLanguages = new List<Language>() {
          SupportedLanguages.Spanish,
          SupportedLanguages.Portuguese,
          SupportedLanguages.French,
         SupportedLanguages.German,
         SupportedLanguages.Dutch,
         SupportedLanguages.Irish,
          SupportedLanguages.Russian,
          SupportedLanguages.Hebrew,
          SupportedLanguages.Hindi,
          SupportedLanguages.Japanese,
          SupportedLanguages.Chinese
        }
      };

      TranslationsManager.PopulateDefaultCultureTranslations();
      foreach (var language in translationSet.SecondaryLanguages) {
        TranslationsManager.PopulateTranslations(language.LanguageTag);
      }

      TranslationsManager.ExportTranslations(translationSet);

    }

  }
}
