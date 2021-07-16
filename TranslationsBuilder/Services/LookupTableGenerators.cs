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

    static string[] ProductSalesCountryList = {
        "Ireland",
        "United Kingdom",
        "France",
        "Netherlands",
        "Spain",
        "Switzerland",
        "Germany",
        "Italy"
    };

    static string[] continentList = {
      "North America",
      "South America",
      "Europe",
      "Africa",
      "Asia",
      "Oceania"
    };

    static string[] countryList = {
        "Afghanistan",
        "Albania",
        "Algeria",
        "Angola",
        "Antigua and Barbuda",
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
        "British Virgin Islands",
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
        "Congo",
        "Cook Islands",
        "Costa Rica",
        "Côte d'Ivoire",
        "Croatia",
        "Cuba",
        "Curaçao",
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
        "Eritrea",
        "Estonia",
        "Ethiopia",
        "Falkland Islands",
        "Fiji",
        "Finland",
        "France",
        "French Guiana",
        "French Polynesia",
        "Gabon",
        "Gambia",
        "Georgia",
        "Germany",
        "Ghana",
        "Gibraltar",
        "Greece",
        "Greenland",
        "Grenada",
        "Guadeloupe",
        "Guam",
        "Guatemala",
        "Guinea",
        "Guinea-Bissau",
        "Guyana",
        "Haiti",
        "Holy See",
        "Honduras",
        "Hong Kong",
        "Hungary",
        "Iceland",
        "India",
        "Indonesia",
        "Iran",
        "Iraq",
        "Ireland",
        "Isle of Man",
        "Israel",
        "Italy",
        "Jamaica",
        "Japan",
        "Jordan",
        "Kazakhstan",
        "Kenya",
        "Kiribati",
        "Kuwait",
        "Kyrgyzstan",
        "Laos",
        "Latvia",
        "Lebanon",
        "Lesotho",
        "Liberia",
        "Libya",
        "Liechtenstein",
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
        "Martinique",
        "Mauritania",
        "Mauritius",
        "Mayotte",
        "Mexico",
        "Micronesia",
        "Moldova",
        "Monaco",
        "Mongolia",
        "Montenegro",
        "Montserrat",
        "Morocco",
        "Mozambique",
        "Myanmar",
        "Namibia",
        "Nauru",
        "Nepal",
        "Netherlands",
        "New Caledonia",
        "New Zealand",
        "Nicaragua",
        "Niger",
        "Nigeria",
        "Niue",
        "North Korea",
        "Northern Mariana Islands",
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
        "Réunion",
        "Romania",
        "Russia",
        "Rwanda",
        "Saint Barthelemy",
        "Saint Helena",
        "Saint Kitts & Nevis",
        "Saint Lucia",
        "Saint Martin",
        "Saint Pierre & Miquelon",
        "Samoa",
        "San Marino",
        "Sao Tome & Principe",
        "Saudi Arabia",
        "Senegal",
        "Serbia",
        "Seychelles",
        "Sierra Leone",
        "Singapore",
        "Sint Maarten",
        "Slovakia",
        "Slovenia",
        "Solomon Islands",
        "Somalia",
        "South Africa",
        "South Korea",
        "South Sudan",
        "Spain",
        "Sri Lanka",
        "Sudan",
        "Suriname",
        "Sweden",
        "Switzerland",
        "Syria",
        "Taiwan",
        "Tajikistan",
        "Tanzania",
        "Thailand",
        "Timor-Leste",
        "Togo",
        "Tokelau",
        "Tonga",
        "Trinidad and Tobago",
        "Tunisia",
        "Turkey",
        "Turkmenistan",
        "Turks and Caicos",
        "Tuvalu",
        "Uganda",
        "Ukraine",
        "United Arab Emirates",
        "United Kingdom",
        "United States",
        "Uruguay",
        "Uzbekistan",
        "Vanuatu",
        "Venezuela",
        "Vietnam",
        "Wallis & Futuna",
        "Western Sahara",
        "Yemen",
        "Zambia",
        "Zimbabwe"
    };

    public static void GenerateContinentLookup(TranslationSet translationSet) {

      // set headers
      string csv = translationSet.DefaultLangauge.LanguageTag;
      foreach (var language in translationSet.SecondaryLanguages) {
        csv += "," + language.LanguageTag;
      }
      csv += "\n";

      foreach (string continent in continentList) {
        Console.Write("Getting translations for " + continent);

        csv += continent;
        foreach (var language in translationSet.SecondaryLanguages) {
          Console.Write(" [" + language.LanguageTag + "]. ");
          csv += "," + TranslatorService.TranslateContent(continent, language.LanguageTag);
        }
        csv += "\n";
        Console.WriteLine();
      }

      string filePath = System.IO.Directory.GetCurrentDirectory() + @"..\..\..\..\..\Data\ContinentNameTranslations.csv";
      StreamWriter writer = new StreamWriter(File.Open(filePath, FileMode.Create), Encoding.UTF8);
      writer.Write(csv);
      writer.Flush();
      writer.Dispose();

      if (true) {
        ExcelUtilities.OpenCsvInExcel(filePath);
      }

    }

    public static void GenerateCountryLookup(TranslationSet translationSet) {

      // set headers
      string csv = translationSet.DefaultLangauge.LanguageTag;
      foreach (var language in translationSet.SecondaryLanguages) {
        csv += "," + language.LanguageTag;
      }
      csv += "\n";


      foreach (string country in countryList) {
        Console.Write("Getting translations for " + country);
        csv += country;
        foreach (var language in translationSet.SecondaryLanguages) {
          Console.Write(" [" + language.LanguageTag + "]. ");
          csv += "," + TranslatorService.TranslateContent(country, language.LanguageTag);
        }
        csv += "\n";
        Console.WriteLine();
      }

      string filePath = System.IO.Directory.GetCurrentDirectory() + @"..\..\..\..\..\Data\CountryNameTranslations.csv";
      StreamWriter writer = new StreamWriter(File.Open(filePath, FileMode.Create), Encoding.UTF8);
      writer.Write(csv);
      writer.Flush();
      writer.Dispose();

      if (true) {
        ExcelUtilities.OpenCsvInExcel(filePath);
      }

    }

    public static void GenerateProductSalesCountryLookup(TranslationSet translationSet) {

      // set headers
      string csv = translationSet.DefaultLangauge.LanguageTag;
      foreach (var language in translationSet.SecondaryLanguages) {
        csv += "," + language.LanguageTag;
      }
      csv += "\n";


      foreach (string country in ProductSalesCountryList) {
        Console.Write("Getting translations for " + country);
        csv += country;
        foreach (var language in translationSet.SecondaryLanguages) {
          Console.Write(" [" + language.LanguageTag + "]. ");
          csv += "," + TranslatorService.TranslateContent(country, language.LanguageTag);
        }
        csv += "\n";
        Console.WriteLine();
      }

      string filePath = System.IO.Directory.GetCurrentDirectory() + @"..\..\..\..\..\Data\CountryNameTranslations.csv";
      StreamWriter writer = new StreamWriter(File.Open(filePath, FileMode.Create), Encoding.UTF8);
      writer.Write(csv);
      writer.Flush();
      writer.Dispose();

      if (true) {
        ExcelUtilities.OpenCsvInExcel(filePath);
      }

    }

  }


  class ProductLookupTableGenerator {

    class Product {
      public string CategoryName { get; set; }
      public string ProductName { get; set; }
      public Product(string CategoryName, string ProductName) {
        this.CategoryName = CategoryName;
        this.ProductName = ProductName;
      }
    }

    static List<Product> productList = new List<Product> {
      new Product("Fruits", "Apples"),
      new Product("Fruits", "Bananas"),
      new Product("Fruits", "Oranges"),
      new Product("Vegetables", "Carrots"),
      new Product("Vegetables", "Cucumbers"),
      new Product("Vegetables", "Tomatoes"),
      new Product("Vegetables", "Potatoes"),
      new Product("Dairy", "Butter"),
      new Product("Dairy", "Cheese"),
      new Product("Dairy", "Milk")
   };

    public static void GenerateProductLookup(TranslationSet translationSet) {

      // set headers
      string csv = translationSet.DefaultLangauge.LanguageTag;
      foreach (var language in translationSet.SecondaryLanguages) {
        csv += "," + language.LanguageTag;
      }
      csv += "\n";


      foreach (Product product in productList) {
        csv += product.ProductName;
        foreach (var language in translationSet.SecondaryLanguages) {
          csv += "," + TranslatorService.TranslateContent(product.ProductName, language.LanguageTag);
        }
        csv += "\n";
      }

      string filePath = System.IO.Directory.GetCurrentDirectory() + @"..\..\..\..\..\Data\ProductNameTranslations.csv";
      StreamWriter writer = new StreamWriter(File.Open(filePath, FileMode.Create), Encoding.UTF8);
      writer.Write(csv);
      writer.Flush();
      writer.Dispose();

      if (true) {
        ExcelUtilities.OpenCsvInExcel(filePath);
      }

    }

    public static void GenerateCategoryLookup(TranslationSet translationSet) {

      // set headers
      string csv = translationSet.DefaultLangauge.LanguageTag;
      foreach (var language in translationSet.SecondaryLanguages) {
        csv += "," + language.LanguageTag;
      }
      csv += "\n";

      string[] categories = { "Fruits", "Vegetables", "Dairy" };
      foreach (string category in categories) {
        csv += category;
        foreach (var language in translationSet.SecondaryLanguages) {
          csv += "," + TranslatorService.TranslateContent(category, language.LanguageTag);
        }
        csv += "\n";
      }

      string filePath = System.IO.Directory.GetCurrentDirectory() + @"..\..\..\..\..\Data\CategoryNameTranslations.csv";
      StreamWriter writer = new StreamWriter(File.Open(filePath, FileMode.Create), Encoding.UTF8);
      writer.Write(csv);
      writer.Flush();
      writer.Dispose();

      if (true) {
        ExcelUtilities.OpenCsvInExcel(filePath);
      }

    }


  

  }



}
