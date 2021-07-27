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

    public static TranslationsTable GetTranslationsTable(string targtLanguage = null) {

      TranslationSet translationSet = new TranslationSet {
        DefaultLangauge = SupportedLanguages.AllLangauges[model.Culture],
        SecondaryLanguages = new List<Language>()
      };

      if (targtLanguage != null) {
        // create table with a single secondary culture if targetLanguage is passed
        translationSet.SecondaryLanguages.Add(SupportedLanguages.AllLangauges[targtLanguage]);
      }
      else {
        // create table for all secondary cultures if no language is passed
        foreach (var culture in model.Cultures) {
          if (culture.Name != model.Culture) {
            translationSet.SecondaryLanguages.Add(SupportedLanguages.AllLangauges[culture.Name]);
          }
        }
      }

      int secondaryLanguageCount = translationSet.SecondaryLanguages.Count;
      int columnCount = 4 + secondaryLanguageCount;

      string[] Headers = new string[columnCount];

      // set column headers
      Headers[0] = "Object Type";
      Headers[1] = "Property";
      Headers[2] = "Name";
      Headers[3] = translationSet.DefaultLangauge.FullName;
      int index = 4;
      foreach (var language in translationSet.SecondaryLanguages) {
        Headers[index] = language.FullName;
        index += 1;
      }

      var defaultCulture = model.Cultures[model.Culture];
      List<string[]> Rows = new List<string[]>();

      foreach (Table table in model.Tables) {
        // exclude hidden tables
        if (!table.IsHidden) {
          // do not include 'Localized Labels' table name for translation
          if (!table.Name.Equals("Localized Labels")) {
            Rows.AddRange((GetTableRows(table, defaultCulture, translationSet)));
          }

          foreach (Column column in table.Columns) {
            if (!column.IsHidden) {
              Rows.AddRange(GetColumnRows(table, column, defaultCulture, translationSet));
            }
          }

          foreach (Measure measure in table.Measures) {
            if (!measure.IsHidden) {
              Rows.AddRange(GetMeasureRows(table, measure, defaultCulture, translationSet));
            }
          }

          foreach (Hierarchy hierarchy in table.Hierarchies) {
            if (!hierarchy.IsHidden) {
              Rows.AddRange(GetHierarchyRows(table, hierarchy, defaultCulture, translationSet));
            }
          }

        }
      }
      return new TranslationsTable {
        Headers = Headers,
        Rows = Rows
      };

    }

    public static List<string[]> GetTableRows(Table table, Culture defaultCulture, TranslationSet translationSet) {

      List<string[]> rows = new List<string[]>();

      // add row for caption
      List<string> captionRowValues = new List<string> {
        "Table",
        "Caption",
        { table.Name },
        defaultCulture.ObjectTranslations[table, TranslatedProperty.Caption]?.Value };

      foreach (var language in translationSet.SecondaryLanguages) {
        captionRowValues.Add(model.Cultures[language.LanguageTag].ObjectTranslations[table, TranslatedProperty.Caption]?.Value);
      }
      rows.Add(captionRowValues.ToArray());

      if (!string.IsNullOrEmpty(table.Description)) {
        List<string> descriptionRowValues = new List<string> { "Table", "Description", table.Name, defaultCulture.ObjectTranslations[table, TranslatedProperty.Description]?.Value };
        foreach (var language in translationSet.SecondaryLanguages) {
          descriptionRowValues.Add(model.Cultures[language.LanguageTag].ObjectTranslations[table, TranslatedProperty.Description]?.Value);
        }
        rows.Add(descriptionRowValues.ToArray());
      }

      return rows;
    }

    public static List<string[]> GetColumnRows(Table table, Column column, Culture defaultCulture, TranslationSet translationSet) {

      List<string[]> rows = new List<string[]>();

      // always add row for caption
      List<string> captionRowValues = new List<string> { "Column", "Caption", $"{table.Name}[{column.Name}]", defaultCulture.ObjectTranslations[column, TranslatedProperty.Caption]?.Value };
      foreach (var language in translationSet.SecondaryLanguages) {
        captionRowValues.Add(model.Cultures[language.LanguageTag].ObjectTranslations[column, TranslatedProperty.Caption]?.Value);
      }
      rows.Add(captionRowValues.ToArray());

      if (!string.IsNullOrEmpty(column.DisplayFolder)) {
        List<string> displayFolderRowValues = new List<string> { "Column", "DisplayFolder", $"{table.Name}[{column.Name}]", defaultCulture.ObjectTranslations[column, TranslatedProperty.DisplayFolder]?.Value };
        foreach (var language in translationSet.SecondaryLanguages) {
          displayFolderRowValues.Add(model.Cultures[language.LanguageTag].ObjectTranslations[column, TranslatedProperty.DisplayFolder]?.Value);
        }
        rows.Add(displayFolderRowValues.ToArray());
      }

      if (!string.IsNullOrEmpty(column.Description)) {
        List<string> descriptionRowValues = new List<string> { "Column", "Description", $"{table.Name}[{column.Name}]", defaultCulture.ObjectTranslations[column, TranslatedProperty.Description]?.Value };
        foreach (var language in translationSet.SecondaryLanguages) {
          descriptionRowValues.Add(model.Cultures[language.LanguageTag].ObjectTranslations[column, TranslatedProperty.Description]?.Value);
        }
        rows.Add(descriptionRowValues.ToArray());
      }

      return rows;
    }

    public static List<string[]> GetMeasureRows(Table table, Measure measure, Culture defaultCulture, TranslationSet translationSet) {

      List<string[]> rows = new List<string[]>();

      // always add row for caption
      List<string> captionRowValues = new List<string> { "Measure", "Caption", $"{table.Name}[{measure.Name}]", defaultCulture.ObjectTranslations[measure, TranslatedProperty.Caption]?.Value };
      foreach (var language in translationSet.SecondaryLanguages) {
        captionRowValues.Add(model.Cultures[language.LanguageTag].ObjectTranslations[measure, TranslatedProperty.Caption]?.Value);
      }
      rows.Add(captionRowValues.ToArray());

      if (!string.IsNullOrEmpty(measure.DisplayFolder)) {
        List<string> displayFolderRowValues = new List<string> { "Measure", "DisplayFolder", $"{table.Name}[{measure.Name}]", defaultCulture.ObjectTranslations[measure, TranslatedProperty.DisplayFolder]?.Value };
        foreach (var language in translationSet.SecondaryLanguages) {
          displayFolderRowValues.Add(model.Cultures[language.LanguageTag].ObjectTranslations[measure, TranslatedProperty.DisplayFolder]?.Value);
        }
        rows.Add(displayFolderRowValues.ToArray());
      }

      if (!string.IsNullOrEmpty(measure.Description)) {
        List<string> descriptionRowValues = new List<string> { "Measure", "Description", $"{table.Name}[{measure.Name}]", defaultCulture.ObjectTranslations[measure, TranslatedProperty.Description]?.Value };
        foreach (var language in translationSet.SecondaryLanguages) {
          descriptionRowValues.Add(model.Cultures[language.LanguageTag].ObjectTranslations[measure, TranslatedProperty.Description]?.Value);
        }
        rows.Add(descriptionRowValues.ToArray());
      }

      return rows;
    }

    public static List<string[]> GetHierarchyRows(Table table, Hierarchy hierarchy, Culture defaultCulture, TranslationSet translationSet) {

      List<string[]> rows = new List<string[]>();

      // always add row for caption
      List<string> captionRowValues = new List<string> { "Hierarchy", "Caption", $"{table.Name}[{hierarchy.Name}]", defaultCulture.ObjectTranslations[hierarchy, TranslatedProperty.Caption]?.Value };
      foreach (var language in translationSet.SecondaryLanguages) {
        captionRowValues.Add(model.Cultures[language.LanguageTag].ObjectTranslations[hierarchy, TranslatedProperty.Caption]?.Value);
      }
      rows.Add(captionRowValues.ToArray());


      foreach(Level hierarchyLevel in hierarchy.Levels) {
        List<string> levelCaptionRowValues = new List<string> { "Level", "Caption", $"{table.Name}[{hierarchy.Name}]{hierarchyLevel.Name}", defaultCulture.ObjectTranslations[hierarchyLevel, TranslatedProperty.Caption]?.Value };
        foreach (var language in translationSet.SecondaryLanguages) {
          levelCaptionRowValues.Add(model.Cultures[language.LanguageTag].ObjectTranslations[hierarchyLevel, TranslatedProperty.Caption]?.Value);
        }
        rows.Add(levelCaptionRowValues.ToArray());
      }


      if (!string.IsNullOrEmpty(hierarchy.DisplayFolder)) {
        List<string> displayFolderRowValues = new List<string> { "Hierarchy", "DisplayFolder", $"{table.Name}[{hierarchy.Name}]", defaultCulture.ObjectTranslations[hierarchy, TranslatedProperty.DisplayFolder]?.Value };
        foreach (var language in translationSet.SecondaryLanguages) {
          displayFolderRowValues.Add(model.Cultures[language.LanguageTag].ObjectTranslations[hierarchy, TranslatedProperty.DisplayFolder]?.Value);
        }
        rows.Add(displayFolderRowValues.ToArray());
      }


      if (!string.IsNullOrEmpty(hierarchy.Description)) {
        List<string> descriptionRowValues = new List<string> { "Hierarchy", "Description", $"{table.Name}[{hierarchy.Name}]", defaultCulture.ObjectTranslations[hierarchy, TranslatedProperty.Description]?.Value };
        foreach (var language in translationSet.SecondaryLanguages) {
          descriptionRowValues.Add(model.Cultures[language.LanguageTag].ObjectTranslations[hierarchy, TranslatedProperty.Description]?.Value);
        }
        rows.Add(descriptionRowValues.ToArray());
      }

      return rows;
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

    public static List<string> GetSecondaryCultureFullNamesInDataModel() {
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
          if (!string.IsNullOrEmpty(table.Description)) {
            culture.ObjectTranslations.SetTranslation(table, TranslatedProperty.Description, table.Description);
          }

          // set translations for all non-hidden columns
          foreach (Column column in table.Columns) {
            if (!column.IsHidden) {
              culture.ObjectTranslations.SetTranslation(column, TranslatedProperty.Caption, column.Name);
              if (!string.IsNullOrEmpty(column.DisplayFolder)) {
                culture.ObjectTranslations.SetTranslation(column, TranslatedProperty.DisplayFolder, column.DisplayFolder);
              }
              if (!string.IsNullOrEmpty(column.Description)) {
                culture.ObjectTranslations.SetTranslation(column, TranslatedProperty.Description, column.Description);
              }
            }
          };

          // set translations for all non-hidden measures
          foreach (Measure measure in table.Measures) {
            if (!measure.IsHidden) {
              culture.ObjectTranslations.SetTranslation(measure, TranslatedProperty.Caption, measure.Name);
              if (!string.IsNullOrEmpty(measure.DisplayFolder)) {
                culture.ObjectTranslations.SetTranslation(measure, TranslatedProperty.DisplayFolder, measure.DisplayFolder);
              }
              if (!string.IsNullOrEmpty(measure.Description)) {
                culture.ObjectTranslations.SetTranslation(measure, TranslatedProperty.Description, measure.Description);
              }
            }
          };

          // set translations for all non-hidden hierarchies
          foreach (Hierarchy hierarchy in table.Hierarchies) {
            if (!hierarchy.IsHidden) {
              culture.ObjectTranslations.SetTranslation(hierarchy, TranslatedProperty.Caption, hierarchy.Name);

              // translate colum names for hierachy levels
              foreach (var level in hierarchy.Levels) {
                culture.ObjectTranslations.SetTranslation(level, TranslatedProperty.Caption, level.Name);
              }

              if (!string.IsNullOrEmpty(hierarchy.DisplayFolder)) {
                culture.ObjectTranslations.SetTranslation(hierarchy, TranslatedProperty.DisplayFolder, hierarchy.DisplayFolder);
              }
              if (!string.IsNullOrEmpty(hierarchy.Description)) {
                culture.ObjectTranslations.SetTranslation(hierarchy, TranslatedProperty.Description, hierarchy.Description);
              }
            }
          };
        }
        model.SaveChanges();
      }
    }

    public static void PopulateCultureWithMachineTranslations(string CultureName, IStatusCalback StatusCalback = null) {

      // add culture to data model if it doesn't already exist
      if (!model.Cultures.ContainsName(CultureName)) {
        model.Cultures.Add(new Culture { Name = CultureName });
      }

      // load culture metadata object
      Culture culture = model.Cultures[CultureName];

      // enumerate through tables
      foreach (Table table in model.Tables) {

        // set Caption translation for table
        var translatedTableName = TranslateContent(table.Name, CultureName);
        culture.ObjectTranslations.SetTranslation(table, TranslatedProperty.Caption, translatedTableName);
        UpdateStatus(StatusCalback, CultureName, table.Name, table.Name, translatedTableName);

        // set Description translation for table
        if (!string.IsNullOrEmpty(table.Description)) {
          var translatedTableDescription = TranslateContent(table.Description, CultureName);
          culture.ObjectTranslations.SetTranslation(table, TranslatedProperty.Description, translatedTableDescription);
          UpdateStatus(StatusCalback, CultureName, table.Name, table.Description, translatedTableDescription);
        }

        // enumerate through columns
        foreach (Column column in table.Columns) {
          if (column.IsHidden == false) {

            // set Caption translation for column
            var translatedColumnName = TranslateContent(column.Name, CultureName);
            culture.ObjectTranslations.SetTranslation(column, TranslatedProperty.Caption, translatedColumnName);
            UpdateStatus(StatusCalback, CultureName, $"{table.Name}[{column.Name}]", column.Name, translatedColumnName);

            // set DisplayFolder translation for column
            if (!string.IsNullOrEmpty(column.DisplayFolder)) {
              var translatedColumnDisplayFolder = TranslateContent(column.DisplayFolder, CultureName);
              culture.ObjectTranslations.SetTranslation(column, TranslatedProperty.DisplayFolder, translatedColumnDisplayFolder);
              UpdateStatus(StatusCalback, CultureName, $"{table.Name}[{column.Name}]", column.DisplayFolder, translatedColumnDisplayFolder);
            }

            // set Description translation for column
            if (!string.IsNullOrEmpty(column.Description)) {
              var translatedColumnDescription = TranslateContent(column.Description, CultureName);
              culture.ObjectTranslations.SetTranslation(column, TranslatedProperty.Description, translatedColumnDescription);
              UpdateStatus(StatusCalback, CultureName, $"{table.Name}[{column.Name}]", column.Description, translatedColumnDescription);
            }

          }
        };

        // enumerate through measures
        foreach (Measure measure in table.Measures) {

          // set Caption translation for measure
          var translatedMeasureName = TranslateContent(measure.Name, CultureName);
          culture.ObjectTranslations.SetTranslation(measure, TranslatedProperty.Caption, translatedMeasureName);
          UpdateStatus(StatusCalback, CultureName, $"{table.Name}[{measure.Name}]", measure.Name, translatedMeasureName);

          // set DisplayFolder translation for measure
          if (!string.IsNullOrEmpty(measure.DisplayFolder)) {
            var translatedMeasureDisplayFolder = TranslateContent(measure.DisplayFolder, CultureName);
            culture.ObjectTranslations.SetTranslation(measure, TranslatedProperty.DisplayFolder, translatedMeasureDisplayFolder);
            UpdateStatus(StatusCalback, CultureName, $"{table.Name}[{measure.Name}]", measure.DisplayFolder, translatedMeasureDisplayFolder);
          }

          // set Description translation for measure
          if (!string.IsNullOrEmpty(measure.Description)) {
            var translatedMeasureDescription = TranslateContent(measure.Description, CultureName);
            culture.ObjectTranslations.SetTranslation(measure, TranslatedProperty.Description, translatedMeasureDescription);
            UpdateStatus(StatusCalback, CultureName, $"{table.Name}[{measure.Name}]", measure.Description, translatedMeasureDescription);
          }

        };

        // enumerate through hierarchies
        foreach (Hierarchy hierarchy in table.Hierarchies) {

          // set Caption translation for hierachy
          var translatedHierarchyName = TranslateContent(hierarchy.Name, CultureName);
          culture.ObjectTranslations.SetTranslation(hierarchy, TranslatedProperty.Caption, translatedHierarchyName);
          UpdateStatus(StatusCalback, CultureName, $"{table.Name}[{hierarchy.Name}]", hierarchy.Name, translatedHierarchyName);

          // translate colum names for hierachy levels
          foreach(var level in hierarchy.Levels) {
            var translatedLevelName = TranslateContent(level.Name, CultureName);
            culture.ObjectTranslations.SetTranslation(level, TranslatedProperty.Caption, translatedLevelName);
            UpdateStatus(StatusCalback, CultureName, $"{table.Name}[{hierarchy.Name}]{translatedLevelName}", hierarchy.Name, translatedLevelName);
          }

          // set DisplayFolder translation for hierarchy
          if (!string.IsNullOrEmpty(hierarchy.DisplayFolder)) {
            var translatedHierarchyDisplayFolder = TranslateContent(hierarchy.DisplayFolder, CultureName);
            culture.ObjectTranslations.SetTranslation(hierarchy, TranslatedProperty.DisplayFolder, translatedHierarchyDisplayFolder);
            UpdateStatus(StatusCalback, CultureName, $"{table.Name}[{hierarchy.Name}]", hierarchy.DisplayFolder, translatedHierarchyDisplayFolder);
          }

          // set Description translation for measure
          if (!string.IsNullOrEmpty(hierarchy.Description)) {
            var translatedHierachyDescription = TranslateContent(hierarchy.Description, CultureName);
            culture.ObjectTranslations.SetTranslation(hierarchy, TranslatedProperty.Description, translatedHierachyDescription);
            UpdateStatus(StatusCalback, CultureName, $"{table.Name}[{hierarchy.Name}]", hierarchy.Description, translatedHierachyDescription);
          }

        };

      }

      model.SaveChanges();
    }

    static string TranslateContent(string Content, string ToCultureName) {
      return TranslatorService.TranslateContent(Content, ToCultureName);
    }

    private static void UpdateStatus(IStatusCalback StatusCalback, string CultureName, string ObjectName, string OriginalText, string TranslatedText) {
      if (StatusCalback != null) {
        string TranslationType = "From " + model.Culture + " to " + CultureName;
        StatusCalback.updateLoadingStatus(TranslationType, ObjectName, OriginalText, TranslatedText);
      }
    }

    public static void ExportTranslations(string targetLanguage = null) {

      var translationsTable = GetTranslationsTable(targetLanguage);

      string linebreak = "\r\n";

      // set csv file headers
      string csv = string.Join(",", translationsTable.Headers) + linebreak;

      // add line for each row
      foreach (var row in translationsTable.Rows) {
        csv += string.Join(",", row) + linebreak;
      }

      DirectoryInfo path = Directory.CreateDirectory(AppSettings.TranslationsOutboxFolderPath);

      string filePath;
      if (targetLanguage == null) {
        filePath = path + @"/" + DatasetName + "-Translations.csv";
      }
      else {
        string targetLanguageDisplayName = SupportedLanguages.AllLangauges[targetLanguage].DisplayName;
        filePath = path + @"/" + DatasetName + "-Translations-" + targetLanguageDisplayName + ".csv";
      }

      StreamWriter writer = new StreamWriter(File.Open(filePath, FileMode.Create), Encoding.UTF8);
      writer.Write(csv);
      writer.Flush();
      writer.Dispose();

      if (true) {
        string excelFilePath = @"""" + filePath + @"""";
        ExcelUtilities.OpenCsvInExcel(excelFilePath);
      }


    }   

    private static string GetTableName(string ObjectName) {
      return ObjectName.Substring(0, ObjectName.IndexOf("["));
    }

    private static string GetChildName(string ObjectName) {
      return ObjectName.Substring(ObjectName.IndexOf("[") + 1, (ObjectName.IndexOf("]") - ObjectName.IndexOf("[") - 1));
    }

    private static string GetLevelName(string ObjectName) {
      return ObjectName.Substring(ObjectName.IndexOf("]") + 1, ObjectName.Length - ObjectName.IndexOf("]") - 1);
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
          case "Level":
            string levelName = GetLevelName(ObjectName);
            return table.Hierarchies[childName].Levels.Find(levelName);
          default:
            throw new ApplicationException("Unknown tabular object type - " + TablularObjectType);
        }
      }
    }

    public static void SetDatasetObjectTranslation(string TablularObjectType, string TranslatedPropertyName, string ObjectName, string TargetLanguage, string TranslatedValue) {
      var targetObject = TranslationsManager.GetMetadataObject(TablularObjectType, ObjectName);

      switch (TranslatedPropertyName) {

        case "Caption":
          model.Cultures[TargetLanguage].ObjectTranslations.SetTranslation(targetObject, TranslatedProperty.Caption, TranslatedValue);
          break;

        case "DisplayFolder":
          model.Cultures[TargetLanguage].ObjectTranslations.SetTranslation(targetObject, TranslatedProperty.DisplayFolder, TranslatedValue);
          break;

        case "Description":
          model.Cultures[TargetLanguage].ObjectTranslations.SetTranslation(targetObject, TranslatedProperty.Description, TranslatedValue);
          break;

      }

      model.SaveChanges();

    }

    public static void ImportTranslations(string filePath) {

      string linebreak = "\r\n";

      FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read);
      StreamReader reader = new StreamReader(stream);
      var lines = reader.ReadToEnd().Trim().Split(linebreak);
      var headers = lines[0].Split(",");      

      var cultureCount = headers.Length - 3;
      var culturesList = new List<string>();

      for (int columnNumber = 3; (columnNumber < headers.Length); columnNumber++) {
        culturesList.Add(SupportedLanguages.GetLanguageFromFullName(headers[columnNumber]).LanguageTag);
      }

      foreach (string cultureName in culturesList) {
        if (!model.Cultures.Contains(cultureName)) {
          model.Cultures.Add( new Culture { Name = cultureName });
        }
      }

      // enumerate through lines in CSV data
      for (int lineNumber = 1; lineNumber <= lines.Length - 1; lineNumber++) {
        var row = lines[lineNumber];
        var rowValues = row.Split(",");
        string objectType = rowValues[0];
        string propertyName = rowValues[1];
        string objectName = rowValues[2];
        // enumerate across language columns
        for (int columnNumber = 3; (columnNumber < headers.Length); columnNumber++) {
          string targetLanguage = SupportedLanguages.GetLanguageFromFullName(headers[columnNumber]).LanguageTag;
          string translatedValue = rowValues[columnNumber];
          if (!string.IsNullOrEmpty(translatedValue)) {
            TranslationsManager.SetDatasetObjectTranslation(objectType, propertyName, objectName, targetLanguage, translatedValue);
          }
        }
      }

      // close file and release resources
      reader.Close();
      stream.Close();
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
        System.Console.WriteLine("Coud not find Microsoft Excel on this PC.");
      }
    }

  }

}

