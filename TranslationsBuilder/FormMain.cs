using Microsoft.AnalysisServices.Tabular;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TranslationsBuilder.Models;
using TranslationsBuilder.Services;

namespace TranslationsBuilder {

  public partial class FormMain : Form {

    public FormMain() {
      InitializeComponent();
    }

    private void onLoad(object sender, EventArgs e) {

      if (TranslationsManager.IsConnected) {
        LoadModel();
      }

    }

    public void LoadModel() {
      var model = TranslationsManager.model;

      txtServer.Text = model.Server.ConnectionString;
      txtDataset.Text = TranslationsManager.DatasetName;

      txtDefaultCulture.Text = SupportedLanguages.AllLangauges[model.Culture].FullName;
      txtCompatibilityLevel.Text = model.Database.CompatibilityLevel.ToString();
      txtEstimatedSize.Text = (model.Database.EstimatedSize / 1000000).ToString("#.0") + " MB";

      listSecondaryCultures.Items.AddRange(TranslationsManager.GetSecondaryCultureDisplayNamesInDataModel().ToArray());

      listLanguageForTransation.Items.AddRange(TranslationsManager.GetSecondaryCulturesInDataModel().ToArray());
      if (listLanguageForTransation.Items.Count > 0) {
        listLanguageForTransation.SelectedIndex = 0;
      }

      listCultureToPopulate.Items.AddRange(TranslationsManager.GetSecondaryCulturesInDataModel().ToArray());
      if (listCultureToPopulate.Items.Count > 0) {
        listCultureToPopulate.SelectedIndex = 0;
      }

      gridTranslations.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.Black;
      gridTranslations.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
      gridTranslations.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
      gridTranslations.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
      gridTranslations.AllowUserToAddRows = false;

      SetGenenrateMachineTranslationsButton();

      PopulateGridWithTranslations();
    }

    public void PopulateGridWithTranslations() {
      gridTranslations.Rows.Clear();
      gridTranslations.Columns.Clear();

      var translationsTable = TranslationsManager.GetTranslationsTable();

      // populate colum headers
      gridTranslations.ColumnCount = translationsTable.Headers.Length;
      for (int index = 0; index <= translationsTable.Headers.Length - 1; index++) {
        gridTranslations.Columns[index].Name = translationsTable.Headers[index];
      }

      // populate rows
      foreach (var row in translationsTable.Rows) {
        gridTranslations.Rows.Add(row);
      }

      gridTranslations.AutoResizeColumns();
      gridTranslations.ClearSelection();
    }

    private void PopulateDefaultCultureTranslations(object sender, EventArgs e) {
      TranslationsManager.PopulateDefaultCultureTranslations();
      this.PopulateGridWithTranslations();
    }

    private void AddSecondaryCulture(object sender, EventArgs e) {

      using (FormAddCultureDialog dialog = new FormAddCultureDialog()) {
        dialog.StartPosition = FormStartPosition.CenterParent;
        dialog.ShowDialog(this);
        if (dialog.DialogResult == DialogResult.OK) {
          string cultureName = dialog.getLanguage();
          TranslationsManager.model.Cultures.Add(new Culture { Name = dialog.getLanguage() });
          TranslationsManager.model.SaveChanges();
          listSecondaryCultures.Items.Clear();
          listSecondaryCultures.Items.AddRange(TranslationsManager.GetSecondaryCulturesInDataModel().ToArray());
          PopulateGridWithTranslations();

          listCultureToPopulate.Items.Clear();
          listCultureToPopulate.Items.AddRange(TranslationsManager.GetSecondaryCulturesInDataModel().ToArray());
          listCultureToPopulate.SelectedIndex = listCultureToPopulate.Items.IndexOf(cultureName);

          listLanguageForTransation.Items.Clear();
          listLanguageForTransation.Items.AddRange(TranslationsManager.GetSecondaryCulturesInDataModel().ToArray());
          listLanguageForTransation.SelectedIndex = listLanguageForTransation.Items.IndexOf(cultureName);

        }
      }


    }

    private void GenenrateMachineTranslations(object sender, EventArgs e) {

      Language targetLanguage = SupportedLanguages.AllLangauges[listCultureToPopulate.SelectedItem.ToString()];
      string targetLanguageTag = targetLanguage.LanguageTag;

      using (FormLoadingStatus dialog = new FormLoadingStatus()) {
        dialog.StartPosition = FormStartPosition.CenterScreen;
        dialog.Show(this);
        TranslationsManager.PopulateCultureWithMachineTranslations(targetLanguageTag, dialog);
        dialog.Close();
      }


      PopulateGridWithTranslations();


    }

    private void ExportTranslations(object sender, EventArgs e) {
      TranslationsManager.ExportTranslations();
    }

    private void ImportTranslations(object sender, EventArgs e) {

      dialogOpenFile.InitialDirectory = AppSettings.TranslationsInboxFolderPath;
      dialogOpenFile.Filter = "CSV files (*.csv)|*.csv";
      dialogOpenFile.FilterIndex = 1;
      dialogOpenFile.RestoreDirectory = true;

      if (dialogOpenFile.ShowDialog() == DialogResult.OK) {

        FileStream stream = File.Open(dialogOpenFile.FileName,FileMode.Open, FileAccess.Read);
        StreamReader reader = new StreamReader(stream);        
        var lines = reader.ReadToEnd().Trim().Split("\r\n");
        var headers = lines[0].Split(",");
        var targetLanguage = headers[3];
        for (int lineNumber = 1; lineNumber <= lines.Length - 1; lineNumber++) {
          var row = lines[lineNumber];
          var rowValues = row.Split(",");
          string objectType = rowValues[0];
          string objectName = rowValues[1];
          string translatedValue = rowValues[3];
          TranslationsManager.SetDatasetObjectTranslation(objectType, objectName, targetLanguage, translatedValue);
        }

        PopulateGridWithTranslations();
      }
    }

    private void ExportTranslationsSheet(object sender, EventArgs e) {

      Language targetLanguage = SupportedLanguages.AllLangauges[listLanguageForTransation.SelectedItem.ToString()];
      string targetLanguageTag = targetLanguage.LanguageTag;

      using (FormLoadingStatus dialog = new FormLoadingStatus()) {
        dialog.StartPosition = FormStartPosition.CenterParent;
        dialog.Show(this);
        TranslationsManager.ExportTranslationsSheet(targetLanguageTag);
        dialog.Close();
      }

      PopulateGridWithTranslations();
    }

    private void ConfigureSettings(object sender, EventArgs e) {

      using (FormConfig dialog = new FormConfig()) {
        dialog.StartPosition = FormStartPosition.CenterParent;
        dialog.ShowDialog(this);
        if (dialog.DialogResult == DialogResult.OK) {
          SetGenenrateMachineTranslationsButton();
        }

      }
    }

    private void SetGenenrateMachineTranslationsButton() {
      if (TranslatorService.IsAvailable) {
        grpMachineTranslations.Visible = true;
      }
      else {
        grpMachineTranslations.Visible = false;
      }


    }

    private void SetDatasetName(object sender, EventArgs e) {

      using (FormSetDatasetName dialog = new FormSetDatasetName()) {
        dialog.StartPosition = FormStartPosition.CenterParent;
        dialog.DatasetName = TranslationsManager.DatasetName;
        dialog.ShowDialog(this);
        if (dialog.DialogResult == DialogResult.OK) {
          TranslationsManager.DatasetName = dialog.DatasetName;
          txtDataset.Text = dialog.DatasetName;
        }

      }


    }
  }
}
