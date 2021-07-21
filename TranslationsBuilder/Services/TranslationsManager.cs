using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Microsoft.AnalysisServices.Tabular;
using TranslationsBuilder.Models;
using System.Windows.Forms;
using System.Drawing;

namespace TranslationsBuilder.Services {

  class TranslationsManager {

    private static Server server = new Server();
    public static Model model;
    public static bool IsConnected = false;

    static TranslationsManager() {
      if (!string.IsNullOrEmpty(AppSettings.Server)) {
        server.Connect(AppSettings.Server);
        model = server.Databases[0].Model;
        IsConnected = true;
      }
    }

    const string DatasetAnnotationName = "FriendlyDatasetName";

    public static string DatasetName {
      get {
        if (model.Annotations.Contains(DatasetAnnotationName)) {
          return model.Annotations[DatasetAnnotationName].Value;
        }
        else {
          return model.Database.Name;
        }
      }
      set {

        if (model.Annotations.Contains(DatasetAnnotationName)) {
          model.Annotations[DatasetAnnotationName].Value = value;
        }
        else {
          model.Annotations.Add(new Annotation { Name = DatasetAnnotationName, Value = value });
        }
        model.SaveChanges();
      }
    }

    public static List<string> GetTables() {
      var tables = new List<string>();
      //*** enumerate through tables
      foreach (Table table in model.Tables) {
        tables.Add(table.Name);
      }
      return tables;
    }

    public static List<string> GetSecondaryCulturesInDataModel() {
      List<string> secondaryCultures = new List<string>();
      foreach (var culture in model.Cultures) {
        if (!culture.Name.Equals(model.Culture)) {
          secondaryCultures.Add(culture.Name);
        }
      }
      return secondaryCultures;
    }

    public static List<string> GetSecondaryCultureDisplayNamesInDataModel() {
      var secondaryCultures = new List<string>();
      foreach (var culture in model.Cultures) {
        if (!culture.Name.Equals(model.Culture)) {
          secondaryCultures.Add(SupportedLanguages.AllLangauges[culture.Name].FullName);
        }
      }
      return secondaryCultures;
    }

    public static List<string> GetSecondaryCulturesAvailable() {
      var secondaryCultures = new List<string>();
      foreach (var language in SupportedLanguages.AllLangauges) {
        if (!language.Value.LanguageTag.Equals(model.Culture)) {
          secondaryCultures.Add(language.Value.LanguageTag);
        }
      }
      return secondaryCultures;
    }

    public static void PopulateDefaultCultureTranslations() {

      // get default Culture object
      Culture culture = model.Cultures[model.Culture];

      foreach (Table table in model.Tables) {
        if (!table.IsHidden) {

          // set translation for table
          culture.ObjectTranslations.SetTranslation(table, TranslatedProperty.Caption, table.Name);

          // set translations for all non-hidden columns
          foreach (Column column in table.Columns) {
            if (!column.IsHidden) {
              culture.ObjectTranslations.SetTranslation(column, TranslatedProperty.Caption, column.Name);
            }
          };

          // set translations for all non-hidden measures
          foreach (Measure measure in table.Measures) {
            if (!measure.IsHidden) {
              culture.ObjectTranslations.SetTranslation(measure, TranslatedProperty.Caption, measure.Name);
            }
          };

          // set translations for all non-hidden hierarchies
          foreach (Hierarchy hierarchy in table.Hierarchies) {
            if (!hierarchy.IsHidden) {
              culture.ObjectTranslations.SetTranslation(hierarchy, TranslatedProperty.Caption, hierarchy.Name);
            }
          };
        }
        model.SaveChanges();
      }
    }

    private static void UpdateStatus(IStatusCalback StatusCalback, string CultureName, string ObjectName, string OriginalText, string TranslatedText) {
      if (StatusCalback != null) {
        string TranslationType = "From " + model.Culture + " to " + CultureName;
        StatusCalback.updateLoadingStatus(TranslationType, ObjectName, OriginalText, TranslatedText);
      }
    }

    public static void PopulateCultureWithMachineTranslations(string CultureName) {

      // add culture to model if it doesn't already exist
      if (!model.Cultures.ContainsName(CultureName)) {
        model.Cultures.Add(new Culture { Name = CultureName });
      }

      // get target Culture object where translations will be added
      Culture culture = model.Cultures[CultureName];

      // enumerate through all non-hidden tables, columns and measures
      foreach (Table table in model.Tables) {
        if (!table.IsHidden) {

          // (1) get machine translation for table name and use it set translation for Caption property
          String translatedName = TranslatorService.TranslateContent(table.Name, CultureName);
          culture.ObjectTranslations.SetTranslation(table, TranslatedProperty.Caption, translatedName);

          // (2) get machine translations for visible column names and use them to set translations
          foreach (Column column in table.Columns) {
            if (column.IsHidden) {
              translatedName = TranslatorService.TranslateContent(column.Name, CultureName);
              culture.ObjectTranslations.SetTranslation(column, TranslatedProperty.Caption, translatedName);
            }
          };

          // (3) get machine translations for visible measure names and use them to set translations
          foreach (Measure measure in table.Measures) {
            if (!measure.IsHidden) {
              translatedName = TranslatorService.TranslateContent(measure.Name, CultureName);
              culture.ObjectTranslations.SetTranslation(measure, TranslatedProperty.Caption, translatedName);
            }
          };

          // (4) get machine translations for visible hnames and use them to set translations
          foreach (Hierarchy hierarchy in table.Hierarchies) {
            if (!hierarchy.IsHidden) {
              translatedName = TranslatorService.TranslateContent(hierarchy.Name, CultureName);
              culture.ObjectTranslations.SetTranslation(hierarchy, TranslatedProperty.Caption, translatedName);
            }
          };
        }
      }
      model.SaveChanges();
    }

    public static void PopulateCultureWithMachineTranslations(string CultureName, IStatusCalback StatusCalback = null) {

      // add culture to data model if it doesn't already exist
      if (!model.Cultures.ContainsName(CultureName)) {
        model.Cultures.Add(new Culture { Name = CultureName });
      }

      // load culture metadata object
      Culture culture = model.Cultures[CultureName];

      //*** enumerate through tables
      foreach (Table table in model.Tables) {
        // get/set translation for table name
        var translatedTableName = TranslateContent(table.Name, CultureName);
        culture.ObjectTranslations.SetTranslation(table, TranslatedProperty.Caption, translatedTableName);
        UpdateStatus(StatusCalback, CultureName, table.Name, table.Name, translatedTableName);
        //*** enumerate through columns
        foreach (Column column in table.Columns) {
          if (column.IsHidden == false) {
            Console.Write(".");
            // get/set translation for column name
            var translatedColumnName = TranslateContent(column.Name, CultureName);
            culture.ObjectTranslations.SetTranslation(column, TranslatedProperty.Caption, translatedColumnName);
            UpdateStatus(StatusCalback, CultureName, $"{table.Name}[{column.Name}]", column.Name, translatedColumnName);
          }
        };
        //*** enumerate through measures
        foreach (Measure measure in table.Measures) {
          // get/set translation for measure name
          var translatedMeasureName = TranslateContent(measure.Name, CultureName);
          culture.ObjectTranslations.SetTranslation(measure, TranslatedProperty.Caption, translatedMeasureName);
          UpdateStatus(StatusCalback, CultureName, $"{table.Name}[{measure.Name}]", measure.Name, translatedMeasureName);
        };
        //*** enumerate through hierarchies
        foreach (Hierarchy hierarchy in table.Hierarchies) {
          // get/set translation for hierarchy name
          var translatedHierarchyName = TranslateContent(hierarchy.Name, CultureName);
          culture.ObjectTranslations.SetTranslation(hierarchy, TranslatedProperty.Caption, translatedHierarchyName);
          UpdateStatus(StatusCalback, CultureName, $"{table.Name}[{hierarchy.Name}]", hierarchy.Name, translatedHierarchyName);
        };
      }
      model.SaveChanges();
      Console.WriteLine();
    }

    static string TranslateContent(string Content, string ToCultureName) {
      return TranslatorService.TranslateContent(Content, ToCultureName);
    }

    public static void ExportTranslations() {

      TranslationSet translationSet = new TranslationSet {
        DefaultLangauge = SupportedLanguages.AllLangauges[model.Culture],
        SecondaryLanguages = new List<Language>()
      };

      foreach (var culture in model.Cultures) {
        if (culture.Name != model.Culture) {
          translationSet.SecondaryLanguages.Add(SupportedLanguages.AllLangauges[culture.Name]);
        }
      }

      ExportTranslations(translationSet);

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

      DirectoryInfo path = Directory.CreateDirectory(AppSettings.TranslationsOutboxFolderPath);
      string filePath = path + @"\" + DatasetName + "-Translations.csv";
      StreamWriter writer = new StreamWriter(File.Open(filePath, FileMode.Create), Encoding.UTF8);
      writer.Write(csv);
      writer.Flush();
      writer.Dispose();

      if (true) {
        ExcelUtilities.OpenCsvInExcel(filePath);
      }

    }

    public static void ExportTranslationsSheet(string CultureName, IStatusCalback StatusCalback = null) {

      if (!model.Cultures.ContainsName(CultureName)) {
        model.Cultures.Add(new Culture { Name = CultureName });
      }


      // load culture metadata object
      string defaultCultureName = model.Culture;
      Culture defaultCulture = model.Cultures[defaultCultureName];
      Culture targetCulture = model.Cultures[CultureName];

      // set csv file headers
      string csv = $"Object Type,Object Name";
      csv += "," + defaultCultureName;
      csv += "," + CultureName;
      csv += "\n";

      foreach (Table table in model.Tables) {
        // exclude hidden tables
        if (!table.IsHidden) {
          // do not include 'Localized Labels' table name for translation
          if (!table.Name.Equals("Localized Labels")) {
            csv += $"Table,{table.Name}";
            csv += "," + defaultCulture.ObjectTranslations[table, TranslatedProperty.Caption]?.Value;
            csv += "," + targetCulture.ObjectTranslations[table, TranslatedProperty.Caption]?.Value;
            csv += "\n";
          }
          // columns
          foreach (Column column in table.Columns) {
            if (!column.IsHidden) {
              csv += $"Column,{table.Name}[{column.Name}]";
              csv += "," + defaultCulture.ObjectTranslations[column, TranslatedProperty.Caption]?.Value;
              csv += "," + targetCulture.ObjectTranslations[column, TranslatedProperty.Caption]?.Value;
              csv += "\n";
            }
          }

          // measures
          foreach (Measure measure in table.Measures) {
            if (!measure.IsHidden) {
              csv += $"Measure,{table.Name}[{measure.Name}]";
              csv += "," + defaultCulture.ObjectTranslations[measure, TranslatedProperty.Caption]?.Value;
              csv += "," + targetCulture.ObjectTranslations[measure, TranslatedProperty.Caption]?.Value;
              csv += "\n";
            }
          };

          foreach (Hierarchy hierarchy in table.Hierarchies) {
            if (!hierarchy.IsHidden) {
              csv += $"Hierarchy,{table.Name}[{hierarchy.Name}]";
              csv += "," + defaultCulture.ObjectTranslations[hierarchy, TranslatedProperty.Caption]?.Value;
              csv += "," + targetCulture.ObjectTranslations[hierarchy, TranslatedProperty.Caption]?.Value;
              csv += "\n";
            }
          };
        }

      }
      string languageName = SupportedLanguages.AllLangauges[CultureName].DisplayName;

      string folderPath = (Directory.CreateDirectory(AppSettings.TranslationsOutboxFolderPath)).FullName;
      string filePath = folderPath + @"/" + DatasetName + "-Translations-" + languageName + ".csv";
      StreamWriter writer = new StreamWriter(File.Open(filePath, FileMode.Create), Encoding.UTF8);
      writer.Write(csv);
      writer.Flush();
      writer.Dispose();

      if (true) {
        ExcelUtilities.OpenCsvInExcel(filePath);
      }

    }

    public static TranslationsTable GetTranslationsTable() {

      TranslationSet translationSet = new TranslationSet {
        DefaultLangauge = SupportedLanguages.AllLangauges[model.Culture],
        SecondaryLanguages = new List<Language>()
      };

      foreach (var culture in model.Cultures) {
        if (culture.Name != model.Culture) {
          translationSet.SecondaryLanguages.Add(SupportedLanguages.AllLangauges[culture.Name]);
        }
      }

      int secondaryLanguageCount = translationSet.SecondaryLanguages.Count;
      int columnCount = 3 + secondaryLanguageCount;

      string[] Headers = new string[columnCount];

      // set column headers
      Headers[0] = "Object Type";
      Headers[1] = "Object Name";
      Headers[2] = translationSet.DefaultLangauge.LanguageTag;
      int index = 3;
      foreach (var language in translationSet.SecondaryLanguages) {
        Headers[index] = language.LanguageTag;
        index += 1;
      }

      var cu = model.Cultures;

      var defaultCulture = model.Cultures[model.Culture];

      List<string[]> Rows = new List<string[]>();

      foreach (Table table in model.Tables) {
        // exclude hidden tables
        if (!table.IsHidden) {
          // do not include 'Localized Labels' table name for translation
          if (!table.Name.Equals("Localized Labels")) {
            List<string> rowValues = new List<string> {
              "Table",
              {table.Name},
              defaultCulture.ObjectTranslations[table, TranslatedProperty.Caption]?.Value
            };
            foreach (var language in translationSet.SecondaryLanguages) {
              rowValues.Add(model.Cultures[language.LanguageTag].ObjectTranslations[table, TranslatedProperty.Caption]?.Value);
            }
            Rows.Add(rowValues.ToArray());
          }

          foreach (Column column in table.Columns) {
            if (!column.IsHidden) {
              List<string> rowValues = new List<string> {
                "Column",
                $"{table.Name}[{column.Name}]",
                defaultCulture.ObjectTranslations[column, TranslatedProperty.Caption]?.Value
              };
              foreach (var language in translationSet.SecondaryLanguages) {
                rowValues.Add(model.Cultures[language.LanguageTag].ObjectTranslations[column, TranslatedProperty.Caption]?.Value);
              }
              Rows.Add(rowValues.ToArray());
            }
          }

          foreach (Measure measure in table.Measures) {
            if (!measure.IsHidden) {
              List<string> rowValues = new List<string> {
                "Measure",
                $"{table.Name}[{measure.Name}]",
                defaultCulture.ObjectTranslations[measure, TranslatedProperty.Caption]?.Value
              };
              foreach (var language in translationSet.SecondaryLanguages) {
                rowValues.Add(model.Cultures[language.LanguageTag].ObjectTranslations[measure, TranslatedProperty.Caption]?.Value);
              }
              Rows.Add(rowValues.ToArray());
            }
          }

          foreach (Hierarchy hierarchy in table.Hierarchies) {
            if (!hierarchy.IsHidden) {
              List<string> rowValues = new List<string> {
                "Hierarchy",
                $"{table.Name}[{hierarchy.Name}]",
                defaultCulture.ObjectTranslations[hierarchy, TranslatedProperty.Caption]?.Value
              };
              foreach (var language in translationSet.SecondaryLanguages) {
                rowValues.Add(model.Cultures[language.LanguageTag].ObjectTranslations[hierarchy, TranslatedProperty.Caption]?.Value);
              }
              Rows.Add(rowValues.ToArray());
            }
          }

        }

      }

      return new TranslationsTable {
        Headers = Headers,
        Rows = Rows
      };

    }

    private static string GetTableName(string ObjectName) {
      return ObjectName.Substring(0, ObjectName.IndexOf("["));
    }

    private static string GetChildName(string ObjectName) {
      return ObjectName.Substring(ObjectName.IndexOf("[") + 1, (ObjectName.IndexOf("]") - ObjectName.IndexOf("[") - 1));
    }

    public static MetadataObject GetMetadataObject(string TablularObjectType, string ObjectName) {
      if (TablularObjectType.Equals("Table")) {
        return model.Tables[ObjectName];
      }
      else {
        string tableName = GetTableName(ObjectName);
        string childName = GetChildName(ObjectName);
        Table table = model.Tables[tableName];
        switch (TablularObjectType) {
          case "Column":
            return table.Columns[childName];
          case "Measure":
            return table.Measures[childName];
          case "Hierarchy":
            return table.Hierarchies[childName];
          default:
            throw new ApplicationException("Unknown tabular object type - " + TablularObjectType);
        }
      }
    }

    public static void SetDatasetObjectTranslation(string TablularObjectType, string ObjectName, string TargetLanguage, string TranslatedValue) {
      try {
        var targetObject = TranslationsManager.GetMetadataObject(TablularObjectType, ObjectName);
        model.Cultures[TargetLanguage].ObjectTranslations.SetTranslation(targetObject, TranslatedProperty.Caption, TranslatedValue);
        model.SaveChanges();
      }
      catch (ArgumentException ex) {
        // ignore error if target does not exist
      }
    }

  }

  class ExcelUtilities {

    public static void OpenCsvInExcel(string FilePath) {

      ProcessStartInfo startInfo = new ProcessStartInfo();

      string excelLocation1 = @"C:\Program Files\Microsoft Office\root\Office16\EXCEL.EXE";
      string excelLocation2 = @"C:\Program Files (x86)\Microsoft Office\root\Office16\EXCEL.EXE";
      bool excelFound = false;

      if (File.Exists(excelLocation1)) {
        startInfo.FileName = excelLocation1;
        excelFound = true;
      }
      else {
        if (File.Exists(excelLocation2)) {
          startInfo.FileName = excelLocation2;
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

