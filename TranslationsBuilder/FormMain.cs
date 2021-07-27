using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using TranslationsBuilder.Models;
using TranslationsBuilder.Services;

using Microsoft.AnalysisServices.Tabular;

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

      listSecondaryCultures.Items.AddRange(TranslationsManager.GetSecondaryCultureFullNamesInDataModel().ToArray());

      listLanguageForTransation.Items.AddRange(TranslationsManager.GetSecondaryCultureFullNamesInDataModel().ToArray());
      if (listLanguageForTransation.Items.Count > 0) {
        listLanguageForTransation.SelectedIndex = 0;
      }

      listCultureToPopulate.Items.AddRange(TranslationsManager.GetSecondaryCultureFullNamesInDataModel().ToArray());
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
      this.Refresh();
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
          listSecondaryCultures.Items.AddRange(TranslationsManager.GetSecondaryCultureFullNamesInDataModel().ToArray());
          PopulateGridWithTranslations();

          string cultureFullNames = SupportedLanguages.AllLangauges[cultureName].FullName;

          listCultureToPopulate.Items.Clear();
          listCultureToPopulate.Items.AddRange(TranslationsManager.GetSecondaryCultureFullNamesInDataModel().ToArray());
          listCultureToPopulate.SelectedIndex = listCultureToPopulate.Items.IndexOf(cultureFullNames);

          listLanguageForTransation.Items.Clear();
          listLanguageForTransation.Items.AddRange(TranslationsManager.GetSecondaryCultureFullNamesInDataModel().ToArray());
          listLanguageForTransation.SelectedIndex = listLanguageForTransation.Items.IndexOf(cultureFullNames);

        }
      }
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
        TranslationsManager.ImportTranslations(dialogOpenFile.FileName);
        LoadModel();
        PopulateGridWithTranslations();
      }

    }

    private void ExportTranslationsSheet(object sender, EventArgs e) {

      Language targetLanguage = SupportedLanguages.GetLanguageFromFullName(listLanguageForTransation.SelectedItem.ToString());
      TranslationsManager.ExportTranslations(targetLanguage.LanguageTag);

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

    private void GenenrateMachineTranslations(object sender, EventArgs e) {

      Language targetLanguage = SupportedLanguages.GetLanguageFromFullName(listCultureToPopulate.SelectedItem.ToString());
      string targetLanguageTag = targetLanguage.LanguageTag;

      using (FormLoadingStatus dialog = new FormLoadingStatus()) {
        dialog.StartPosition = FormStartPosition.CenterScreen;
        dialog.Show(this);
        TranslationsManager.PopulateCultureWithMachineTranslations(targetLanguageTag, dialog);
        dialog.Close();
      }

      PopulateGridWithTranslations();
    }

    private void GenenrateAllMachineTranslations(object sender, EventArgs e) {

      using (FormLoadingStatus dialog = new FormLoadingStatus()) {

        dialog.StartPosition = FormStartPosition.CenterScreen;
        dialog.Show(this);

        foreach (var language in TranslationsManager.GetSecondaryCulturesInDataModel()) {
          TranslationsManager.PopulateCultureWithMachineTranslations(language, dialog);
          PopulateGridWithTranslations();
        }

        dialog.Close();
      }

      PopulateGridWithTranslations();

    }
  
  }
}
