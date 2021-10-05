using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

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

            var matches = Regex.Matches(input, @$"\b(?'sumWcur'(?'sum'[0-9 ]+) ?(?'cur'{possibleCurrencySymbolsStr}))(\s|$)", RegexOptions.Multiline);

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

    public class CurrencySetting
    {
        //public Currency Currency { get; set; }

        public List<string> Symbols { get; set; }

        public bool IsBefore { get; set; }

        public bool IsAfter { get; set; }

        public string ISO { get; set; }
    }
}