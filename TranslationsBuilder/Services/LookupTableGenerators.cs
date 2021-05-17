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

    public class CityData {
      public string Country { get; set; }
      public string City { get; set; }
      public string FulllName { 
        get {
          return City + ", " + Country;
        }
      }

    }

    static CityData[] productSalesCityList = {
      new CityData{ Country="Ireland", City="Cork" },
      new CityData{ Country="Ireland", City="Dublin" },
      new CityData{ Country="Ireland", City="Limerick" },
      new CityData{ Country="Ireland", City="Tralee" },
      new CityData{ Country="Ireland", City="Waterford" },
      new CityData{ Country="United Kingdom", City="Birmingham" },
      new CityData{ Country="United Kingdom", City="Bristol" },
      new CityData{ Country="United Kingdom", City="Leeds" },
      new CityData{ Country="United Kingdom", City="Liverpool" },
      new CityData{ Country="United Kingdom", City="London" },
      new CityData{ Country="United Kingdom", City="Manchester" },
      new CityData{ Country="United Kingdom", City="Newcastle" },
      new CityData{ Country="France", City="Lyon" },
      new CityData{ Country="France", City="Marseille" },
      new CityData{ Country="France", City="Nantes" },
      new CityData{ Country="France", City="Nice" },
      new CityData{ Country="France", City="Paris" },
      new CityData{ Country="France", City="Toulouse" },
      new CityData{ Country="Netherlands", City="Amsterdam" },
      new CityData{ Country="Netherlands", City="Rotterdam" },
      new CityData{ Country="Netherlands", City="The Hague" },
      new CityData{ Country="Netherlands", City="Utrecht" },
      new CityData{ Country="Spain", City="Alicante" },
      new CityData{ Country="Spain", City="Barcelona" },
      new CityData{ Country="Spain", City="Bilbao" },
      new CityData{ Country="Spain", City="Córdoba" },
      new CityData{ Country="Spain", City="Madrid" },
      new CityData{ Country="Spain", City="Murcia" },
      new CityData{ Country="Spain", City="Seville" },
      new CityData{ Country="Spain", City="Valencia" },
      new CityData{ Country="Spain", City="Zaragoza" },
      new CityData{ Country="Switzerland", City="Basel" },
      new CityData{ Country="Switzerland", City="Geneva" },
      new CityData{ Country="Switzerland", City="Lucerne" },
      new CityData{ Country="Switzerland", City="Zürich" },
      new CityData{ Country="Italy", City="Bologna" },
      new CityData{ Country="Italy", City="Catania" },
      new CityData{ Country="Italy", City="Florence" },
      new CityData{ Country="Italy", City="Genoa" },
      new CityData{ Country="Italy", City="Milan" },
      new CityData{ Country="Italy", City="Naples" },
      new CityData{ Country="Italy", City="Palermo" },
      new CityData{ Country="Italy", City="Rome" },
      new CityData{ Country="Germany", City="Berlin" },
      new CityData{ Country="Germany", City="Bonn" },
      new CityData{ Country="Germany", City="Cologne" },
      new CityData{ Country="Germany", City="Dresden" },
      new CityData{ Country="Germany", City="Düsseldorf" },
      new CityData{ Country="Germany", City="Frankfurt" },
      new CityData{ Country="Germany", City="Hamburg" },
      new CityData{ Country="Germany", City="Munich" }
    };

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
        OpenCsvInExcel(filePath);
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
        OpenCsvInExcel(filePath);
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
        OpenCsvInExcel(filePath);
      }

    }


    public static void GenerateProductSalesCityLookup(TranslationSet translationSet) {

      // set headers
      string csv = translationSet.DefaultLangauge.LanguageTag;
      foreach (var language in translationSet.SecondaryLanguages) {
        csv += "," + language.LanguageTag;
      }
      csv += "\n";


      foreach (CityData city in productSalesCityList) {
        Console.Write("Getting translations for " + city.City);
        csv += city.City;
        foreach (var language in translationSet.SecondaryLanguages) {
          Console.Write(" [" + language.LanguageTag + "]. ");
          csv += "," + TranslatorService.TranslateContent(city.City, language.LanguageTag);
        }
        csv += "\n";
        Console.WriteLine();
      }

      string filePath = System.IO.Directory.GetCurrentDirectory() + @"..\..\..\..\..\Data\CityNameTranslations.csv";
      StreamWriter writer = new StreamWriter(File.Open(filePath, FileMode.Create), Encoding.UTF8);
      writer.Write(csv);
      writer.Flush();
      writer.Dispose();

      if (true) {
        OpenCsvInExcel(filePath);
      }

    }

    public static void GenerateProductSalesCityNameLookup(TranslationSet translationSet) {

      // set headers
      string csv = translationSet.DefaultLangauge.LanguageTag;
      foreach (var language in translationSet.SecondaryLanguages) {
        csv += "," + language.LanguageTag;
      }
      csv += "\n";


      foreach (CityData city in productSalesCityList) {
        Console.Write("Getting translations for " + city.FulllName);
        csv += city.FulllName;
        foreach (var language in translationSet.SecondaryLanguages) {
          Console.Write(" [" + language.LanguageTag + "]. ");
          csv += "," + TranslatorService.TranslateContent(city.FulllName, language.LanguageTag);
        }
        csv += "\n";
        Console.WriteLine();
      }

      string filePath = System.IO.Directory.GetCurrentDirectory() + @"..\..\..\..\..\Data\CityFullNameTranslations.csv";
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
        OpenCsvInExcel(filePath);
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
