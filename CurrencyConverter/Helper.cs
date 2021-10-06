using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Serializers.NewtonsoftJson;

namespace CurrencyConverter
{
    public static class Helper
    {
        public static Dictionary<Currency, CurrencySetting> CurrencySettings { get; set; } = new()
        {
            [Currency.Rub] = new CurrencySetting
            {
                //Currency = Currency.Rub,
                IsAfter = true,
                Symbols = new List<string> { "р", "руб", "руб.", "р.", "рублей", "рубля" },
                ISO = "RUB",
            },
            [Currency.Tenge] = new CurrencySetting
            {
                //Currency = Currency.Rub,
                IsAfter = true,
                Symbols = new List<string> { "т", "тнг", "тнг.", "т.", "тенге" },
                ISO = "KZT",
            },
        };

        public static List<ParsingResult> ParseSumAndCurrency(string input)
        {
            var result = new List<ParsingResult>();

            var possibleCurrencySymbols = CurrencySettings.Values.SelectMany(x => x.Symbols).ToDictionary(x => x).Keys.Select(x => x).Select(Regex.Escape).ToList();
            var possibleCurrencySymbolsStr = string.Join("|", possibleCurrencySymbols);

            var matches = Regex.Matches(input, @$"\b(?'sumWcur'(?'sum'(?!0+ 00)(?=.{{1,9}}( |$))(?!0(?! ))\d{{1,3}}( \d{{3}})*( \d+)?) ?(?'cur'{possibleCurrencySymbolsStr}))(\s|$)", RegexOptions.Multiline);

            foreach (Match match in matches)
            {
                var parsingResult = new ParsingResult();

                parsingResult.Currency = DetectCurrency(match.Groups["cur"].Value);
                parsingResult.Sum = decimal.Parse(match.Groups["sum"].Value);

                result.Add(parsingResult);
            }

            return result;
        }

        public static Currency DetectCurrency(string input)
        {
            var currencySetting = CurrencySettings.FirstOrDefault(x => x.Value.Symbols.Contains(input));

            return currencySetting.Key;
        }

        public static void Q()
        {
            
        }
    }

    public class ParsingResult
    {
        public decimal Sum { get; set; }

        public Currency Currency { get; set; }

        public override string ToString() => $"{Sum}{Currency}";
    }

    public enum Currency
    {
        None,

        Rub,

        Tenge
    }

    //public class Currency
    //{
    //    public Currencies Type { get; set; }
    //}

    // \b(?'sumWcur'(?'sum'[0-9 ]+) ?(?'cur'р|руб|руб\.))(\s|$)
    // \b(?'sumWcur'(?'sum'[0-9 ]+) ?(?'cur'[a-zA-Zа-яА-Я.]+))(\s|$)
    // \b(?'sumWcur'(?'sum'(?!0+ 00)(?=.{1,9}( |$))(?!0(?! ))\d{1,3}( \d{3})*( \d+)?) ?(?'cur'р|руб|руб\.|р\.|рублей|рубля|т|тнг|тнг\.|т\.|тенге))(\s|$)
    // \b(?'sumWcur'(?'sum'(?!0+\.00)(?=.{1,9}(\.|$))(?!0(?!\.))\d{1,3}((,| |)\d{3})*(\.\d+)?) ?(?'cur'р|руб|руб\.|р\.|рублей|рубля|т|тнг|тнг\.|т\.|тенге))(\s|$)

    /*
     * 5р
       5 р
       
       5000р
       5000 р
       
       5 000 | 5.000
       5 000р | 5,000р
       
       5 5 000р | 5 5,000р 
       5 5 000 р | 5 5,000 р
       
       5р 5 000р | 5р 5,000р
       5 р 5 000 р | 5 р 5,000 р
       
       55 5 000р | 55 5,000р
       55 5 000 р | 55 5,000 р
       
       555 5 000р | 555 5,000р
       555 5 000 р | 555 5,000 р
       
       5555 5 000р | 5555 5,000р
       5555 5 000 р | 5555 5,000 р
       
       5555р 5 000р | 5555р 5,000р
       5555 р 5 000 р | 5555 р 5,000 р
       
       5 000 000р | 2.000.000р
     */


    // ^(?!0+\.00)(?=.{1,9}(\.|$))(?!0(?!\.))\d{1,3}((,| |)\d{3})*(\.\d+)?$

    /*
     * 5
       50
       500
       5000
       50000
       500000
       5000000
       
       5
       50
       500
       5 000
       50 000
       500 000
       5 000 000
       
       5 5
       5 50
       500
       5 5 000
       5 50 000
       500 000
       5 5 000 000
       
       5
       50
       500
       5,000
       50,000
       500,000
       5,000,000
       
       5 5
       5 50
       500
       5 5,000
       5 50,000
       500,000
       5 5,000,000
       
       5.00
       50.00
       500.00
       5,000.00
       50,000.00
       500,000.00
       5,000,000.00
       
       5 5.00
       5 50.00
       500.00
       5 5,000.00
       5 50,000.00
       500,000.00
       5 5,000,000.00
     */

    public class CurrencySetting
    {
        //public Currency Currency { get; set; }

        public List<string> Symbols { get; set; }

        public bool IsBefore { get; set; }

        public bool IsAfter { get; set; }

        public string ISO { get; set; }
    }
}