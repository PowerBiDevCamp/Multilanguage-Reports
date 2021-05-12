using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranslationsBuilder.Models;

namespace TranslationsBuilder.Services {

  class CountryLookupTableGenerator {

    static string[] countryList = {
      "Afghanistan",
      "Albania",
      "Algeria",
      "Angola",
      "Antigua And Barbuda",
      "Argentina",
      "Armenia",
      "Aruba",
      "Australia",
      "Austria",
      "Azerbaijan",
      "Bahamas",
      "Bahrain",
      "Bangladesh",
      "Barbados",
      "Belarus",
      "Belgium",
      "Belize",
      "Benin",
      "Bermuda",
      "Bhutan",
      "Bolivia",
      "Bosnia",
      "Botswana",
      "Brazil",
      "Brunei",
      "Bulgaria",
      "Burkina Faso",
      "Burundi",
      "Cambodia",
      "Cameroon",
      "Canada",
      "Cayman Islands",
      "Central African Republic",
      "Chad",
      "Chile",
      "China",
      "Colombia",
      "Comoros",
      "Costa Rica",
      "Croatia",
      "Cuba",
      "Cyprus",
      "Czech Republic",
      "Denmark",
      "Djibouti",
      "Dominica",
      "Dominican Republic",
      "Ecuador",
      "Egypt",
      "El Salvador",
      "Equatorial Guinea",
      "Estonia",
      "Ethiopia",
      "Fiji",
      "Finland",
      "France",
      "Gabon",
      "Gambia",
      "Georgia",
      "Germany",
      "Ghana",
      "Greece",
      "Grenada",
      "Guatemala",
      "Guinea",
      "Guinea-Bissau",
      "Guyana",
      "Haiti",
      "Honduras",
      "Hong Kong",
      "Hungary",
      "Iceland",
      "India",
      "Indonesia",
      "Iran",
      "Iraq",
      "Ireland",
      "Isle Of Man",
      "Israel",
      "Italy",
      "Jamaica",
      "Japan",
      "Jordan",
      "Kazakhstan",
      "Kenya",
      "Kiribati",
      "Kuwait",
      "Latvia",
      "Lebanon",
      "Lesotho",
      "Liberia",
      "Libya",
      "Lithuania",
      "Luxembourg",
      "Macao",
      "Madagascar",
      "Malawi",
      "Malaysia",
      "Maldives",
      "Mali",
      "Malta",
      "Marshall Islands",
      "Mauritania",
      "Mauritius",
      "Mexico",
      "Micronesia",
      "Moldova",
      "Mongolia",
      "Montenegro",
      "Morocco",
      "Mozambique",
      "Myanmar",
      "Namibia",
      "Nauru",
      "Nepal",
      "Netherlands",
      "New Zealand",
      "Nicaragua",
      "Niger",
      "Nigeria",
      "Norway",
      "Oman",
      "Pakistan",
      "Palau",
      "Panama",
      "Papua New Guinea",
      "Paraguay",
      "Peru",
      "Philippines",
      "Poland",
      "Portugal",
      "Puerto Rico",
      "Qatar",
      "Romania",
      "Russia",
      "Rwanda",
      "Samoa",
      "Saudi Arabia",
      "Senegal",
      "Serbia",
      "Seychelles",
      "Sierra Leone",
      "Singapore",
      "Slovenia",
      "Solomon Islands",
      "South Africa",
      "South Korea",
      "South Sudan",
      "Spain",
      "Sri Lanka",
      "Sudan",
      "Suriname",
      "Sweden",
      "Switzerland",
      "Tajikistan",
      "Tanzania",
      "Thailand",
      "Timor-Leste",
      "Togo",
      "Tonga",
      "Trinidad And Tobago",
      "Tunisia",
      "Turkey",
      "Turkmenistan",
      "Turks and Caicos",
      "Tuvalu",
      "United Arab Emirates",
      "Uganda",
      "Ukraine",
      "United Kingdom",
      "United States",
      "Uruguay",
      "Uzbekistan",
      "Vanuatu",
      "Venezuela",
      "Vietnam",
      "Yemen",
      "Zambia",
      "Zimbabwe"
    };

    public static void GenerateCountryLookup(TranslationSet translationSet) {

      // set headers
      string csv = translationSet.DefaultLangauge.DisplayLabel;
      foreach (var language in translationSet.SecondaryLanguages) {
        csv += ", " + language.DisplayLabel;
      }
      csv += "\n";


      foreach (string country in countryList) {
        csv += country;
        foreach (var language in translationSet.SecondaryLanguages) {
          csv += "," + TranslatorService.TranslateContent(country, language.LanguageTag);
        }
        csv += "\n";
      }

      string filePath = System.IO.Directory.GetCurrentDirectory() + @"\CountryNameTranslations.csv";
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
