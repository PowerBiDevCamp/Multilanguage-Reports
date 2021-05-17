using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Microsoft.AnalysisServices.Tabular;
using TranslationsBuilder.Models;

namespace TranslationsBuilder.Services {

  class TranslationsManager {

    static Server server = new Server();
    static Model model;

    static TranslationsManager() {
      server.Connect(AppSettings.connectString);
      model = server.Databases[0].Model;
    }

    public static void PopulateDefaultCultureTranslations() {

      Culture defaultCulture = model.Cultures[model.Culture];

      Console.Write("Settings data model translations for default culture of " + defaultCulture.Name);

      foreach (Table table in model.Tables) {

        Console.Write(".");
        defaultCulture.ObjectTranslations.SetTranslation(table, TranslatedProperty.Caption, table.Name);

        foreach (Column column in table.Columns) {
          if (column.Type != ColumnType.RowNumber) {
            Console.Write(".");
            defaultCulture.ObjectTranslations.SetTranslation(column, TranslatedProperty.Caption, column.Name);
          }
        };

        foreach (Measure measure in table.Measures) {
          Console.Write(".");
          defaultCulture.ObjectTranslations.SetTranslation(measure, TranslatedProperty.Caption, measure.Name);
        };

        foreach (Hierarchy hierarchy in table.Hierarchies) {
          Console.Write(".");
          defaultCulture.ObjectTranslations.SetTranslation(hierarchy, TranslatedProperty.Caption, hierarchy.Name);
        };
        
      }

      model.SaveChanges();

      Console.WriteLine();
    }

    public static void PopulateTranslations(string CultureName) {

      Console.Write("Calling to Azure Translator Service to get translations for " + CultureName);

      Culture culture = new Culture {
        Name = CultureName
      };

      if (!model.Cultures.ContainsName(CultureName)) {
        model.Cultures.Add(culture);
      }

      culture = model.Cultures[CultureName];

      foreach (Table table in model.Tables) {
        Console.Write(".");
        var translatedTableName = TranslateContent(table.Name, CultureName);
        culture.ObjectTranslations.SetTranslation(table, TranslatedProperty.Caption, translatedTableName);

        foreach (Column column in table.Columns) {
          if (column.Type != ColumnType.RowNumber) {
            Console.Write(".");
            var translatedColumnName = TranslateContent(column.Name, CultureName);
            culture.ObjectTranslations.SetTranslation(column, TranslatedProperty.Caption, translatedColumnName);
          }
        };

        foreach (Measure measure in table.Measures) {
          Console.Write(".");
          var translatedMeasureName = TranslateContent(measure.Name, CultureName);
          culture.ObjectTranslations.SetTranslation(measure, TranslatedProperty.Caption, translatedMeasureName);
        };

        foreach (Hierarchy hierarchy in table.Hierarchies) {
          Console.Write(".");
          var translatedHierarchyName = TranslateContent(hierarchy.Name, CultureName);
          culture.ObjectTranslations.SetTranslation(hierarchy, TranslatedProperty.Caption, translatedHierarchyName);
        };

      }

      model.SaveChanges();

      Console.WriteLine();
    }

    static string TranslateContent(string Content, string ToCultureName) {
      return TranslatorService.TranslateContent(Content, ToCultureName);
    }

    public static void ExportTranslations(TranslationSet translationSet) {
      
      Console.WriteLine();
      Console.WriteLine("Exporting model translations in CVS format to to open in Excel");

      // ensure default language is correct for model
      if (translationSet.DefaultLangauge.LanguageTag != model.Culture) {
        throw new ApplicationException("ERROR: Translation set has different default language than the model");
      }

      string defaultCultureName = model.Culture;
      Culture defaultCulture = model.Cultures[defaultCultureName];

      // set csv file headers
      string csv = $"Object Type,Object Name";
      csv += ", " + translationSet.DefaultLangauge.LanguageTag;
      foreach (var language in translationSet.SecondaryLanguages) {
        csv += ", " + language.LanguageTag;
      }
      csv += "\n";

      foreach (Table table in model.Tables) {
        // exclude hidden tables
        if (!table.IsHidden) {
          // do not include 'Localized Labels' table name for translation
          if (!table.Name.Equals("Localized Labels")) {
            csv += $"Table,{table.Name}";
            csv += "," + defaultCulture.ObjectTranslations[table, TranslatedProperty.Caption]?.Value;
            foreach (var language in translationSet.SecondaryLanguages) {
              csv += ", " + model.Cultures[language.LanguageTag].ObjectTranslations[table, TranslatedProperty.Caption]?.Value;
            }
            csv += "\n";
          }
          // columns
          foreach (Column column in table.Columns) {
            if (!column.IsHidden) {
              csv += $"Column,{table.Name}[{column.Name}]";
              csv += "," + defaultCulture.ObjectTranslations[column, TranslatedProperty.Caption]?.Value;
              foreach (var language in translationSet.SecondaryLanguages) {
                csv += ", " + model.Cultures[language.LanguageTag].ObjectTranslations[column, TranslatedProperty.Caption]?.Value;
              }
              csv += "\n";
            }
          }

          // measures
          foreach (Measure measure in table.Measures) {
            if (!measure.IsHidden) {
              csv += $"Measure,{table.Name}[{measure.Name}]";
              csv += "," + defaultCulture.ObjectTranslations[measure, TranslatedProperty.Caption]?.Value;
              foreach (var language in translationSet.SecondaryLanguages) {
                csv += ", " + model.Cultures[language.LanguageTag].ObjectTranslations[measure, TranslatedProperty.Caption]?.Value;
              }
              csv += "\n";
            }
          };

          foreach (Hierarchy hierarchy in table.Hierarchies) {
            if (!hierarchy.IsHidden) {
              csv += $"Hierarchy,{table.Name}[{hierarchy.Name}]";
              csv += "," + defaultCulture.ObjectTranslations[hierarchy, TranslatedProperty.Caption]?.Value;
              foreach (var language in translationSet.SecondaryLanguages) {
                csv += ", " + model.Cultures[language.LanguageTag].ObjectTranslations[hierarchy, TranslatedProperty.Caption]?.Value;
              }
              csv += "\n";
            }
          };
        }

      }

      string filePath = System.Reflection.Assembly.GetExecutingAssembly().Location + @"DataModelTranslations.csv";
      StreamWriter writer = new StreamWriter(File.Open(filePath, FileMode.Create), Encoding.UTF8);
      writer.Write(csv);
      writer.Flush();
      writer.Dispose();

      if (true) {
        OpenCsvInExcel(filePath);
      }

    }

    private static void OpenCsvInExcel(string FilePath) {

      ProcessStartInfo startInfo = new ProcessStartInfo();

      bool excelFound = false;
      if (File.Exists("C:\\Program Files\\Microsoft Office\\root\\Office16\\EXCEL.EXE")) {
        startInfo.FileName = "C:\\Program Files\\Microsoft Office\\root\\Office16\\EXCEL.EXE";
        excelFound = true;
      }
      else {
        if (File.Exists("C:\\Program Files (x86)\\Microsoft Office\\root\\Office16\\EXCEL.EXE")) {
          startInfo.FileName = "C:\\Program Files (x86)\\Microsoft Office\\root\\Office16\\EXCEL.EXE";
          excelFound = true;
        }
      }
      if (excelFound) {
        startInfo.Arguments = FilePath;
        Process.Start(startInfo);
      }
      else {
        System.Console.WriteLine("Coud not find Microsoft Exce on this PC.");
      }

    }

  }
}
