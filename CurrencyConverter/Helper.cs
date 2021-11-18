using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
                IsAfter = true,
                Symbols = new List<string> { "р", "руб", "руб.", "р.", "рублей", "рубля", "₽", "rub" },
                ISO = "RUB",
            },
            [Currency.Tenge] = new CurrencySetting
            {
                IsAfter = true,
                Symbols = new List<string> { "т", "тнг", "тнг.", "т.", "тенге" },
                ISO = "KZT",
            },
            [Currency.Dollar] = new CurrencySetting
            {
                IsAfter = true,
                IsBefore = true,
                Symbols = new List<string> { "д", "$", "д.", "дол", "долларов", "доллара", "доллар", "баксов", "бакса", "бакс" },
                ISO = "USD",
            },
        };

        public static List<GroupSetting> GroupSettings { get; set; } = new()
        {
            //new()
            //{
            //    ChatId = 66227,
            //    CurrencyConvertSettings = new()
            //    {
            //        new()
            //        {
            //            Currency = Currency.Dollar,
            //            TargetCurrencies = new()
            //            {
            //                Currency.Rub,
            //                Currency.Tenge,
            //            }
            //        },
            //        new()
            //        {
            //            Currency = Currency.Rub,
            //            TargetCurrencies = new()
            //            {
            //                Currency.Tenge,
            //            }
            //        },
            //        new()
            //        {
            //            Currency = Currency.Tenge,
            //            TargetCurrencies = new()
            //            {
            //                Currency.Rub,
            //            }
            //        },
            //    }
            //},
        };

        public static GroupSetting DefaultGroupSetting { get; set; } = new GroupSetting
        {
            ChatId = 66227,
            CurrencyConvertSettings = new List<CurrencyConvertSetting>
            {
                new CurrencyConvertSetting
                {
                    Currency = Currency.Dollar,
                    TargetCurrencies = new List<Currency>
                    {
                        Currency.Rub,
                        Currency.Tenge,
                    }
                },
                new CurrencyConvertSetting
                {
                    Currency = Currency.Rub,
                    TargetCurrencies = new List<Currency>
                    {
                        Currency.Tenge,
                        Currency.Dollar,
                    }
                },
                new CurrencyConvertSetting
                {
                    Currency = Currency.Tenge,
                    TargetCurrencies = new List<Currency>
                    {
                        Currency.Rub,
                        Currency.Dollar,
                    }
                },
            }
        };

        public static GroupSetting GetGroupSetting(long chatId)
        {
            return GroupSettings.FirstOrDefault(x => x.ChatId == chatId) ?? DefaultGroupSetting;
        }

        public static List<Currency> GetTargetCurrencies(this GroupSetting groupSetting, Currency currency)
        {
            return groupSetting.CurrencyConvertSettings.FirstOrDefault(x => x.Currency == currency).TargetCurrencies;
        }

        public static decimal GetExchangeRate(Currency sourceCurrency, Currency targetCurrency)
        {
            var source1 = JToken.Parse(TestRequest)["conversion_rates"][CurrencySettings[sourceCurrency].ISO].Value<decimal>();
            
            var target1 = JToken.Parse(TestRequest)["conversion_rates"][CurrencySettings[targetCurrency].ISO].Value<decimal>();
            
            var exchangeRate = target1 / source1;
            
            return exchangeRate;
        }

        //public static 

        public static List<ParsingResult> ParseSumAndCurrency(string input)
        {
            var result = new List<ParsingResult>();

            var possibleCurrencySymbols = CurrencySettings.Values.SelectMany(x => x.Symbols).ToDictionary(x => x).Keys.Select(x => x).Select(Regex.Escape).ToList();
            var possibleCurrencySymbolsStr = string.Join("|", possibleCurrencySymbols);

            //var matches = Regex.Matches(input, @$"\b(?'sumWcur'(?'sum'(?!0+ 00)(?=.{{1,9}}( |$))(?!0(?! ))\d{{1,3}}( \d{{3}})*( \d+)?) ?(?'cur'{possibleCurrencySymbolsStr}))(\s|$)", RegexOptions.Multiline);
            //var matches = Regex.Matches(input, @$"\b(?'sumWcur'(?'sum'(?!0+\.00)(?=.{{1,9}}(\.|$))(?!0(?!\.))\d{{1,3}}((,| |)\d{{3}})*(\.\d+)?) ?(?'cur'{possibleCurrencySymbolsStr}))(\s|$)", RegexOptions.Multiline);
            var matches = Regex.Matches(input, @$"\b(?'sumWcur'(?'sum'(?!0+\.00)(?=.{{1,9}}(\.|$| ))(?!0(?!\.))\d{{1,3}}((,| |)\d{{3}})*(\.\d+)?) ?(?'cur'{possibleCurrencySymbolsStr}))(\s|$)", RegexOptions.Multiline);

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

        public static string TestRequest = @"{
 ""conversion_rates"":{
  ""USD"":1,
  ""AED"":3.67,
  ""KZT"":423.98,
  ""RUB"":72.47
 }
}";
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

        Tenge,

        Dollar,
    }

    //public class Currency
    //{
    //    public Currencies Type { get; set; }
    //}

    // \b(?'sumWcur'(?'sum'[0-9 ]+) ?(?'cur'р|руб|руб\.))(\s|$)
    // \b(?'sumWcur'(?'sum'[0-9 ]+) ?(?'cur'[a-zA-Zа-яА-Я.]+))(\s|$)
    // \b(?'sumWcur'(?'sum'(?!0+ 00)(?=.{1,9}( |$))(?!0(?! ))\d{1,3}( \d{3})*( \d+)?) ?(?'cur'р|руб|руб\.|р\.|рублей|рубля|т|тнг|тнг\.|т\.|тенге))(\s|$)
    // \b(?'sumWcur'(?'sum'(?!0+\.00)(?=.{1,9}(\.|$))(?!0(?!\.))\d{1,3}((,| |)\d{3})*(\.\d+)?) ?(?'cur'р|руб|руб\.|р\.|рублей|рубля|т|тнг|тнг\.|т\.|тенге))(\s|$)
    // \b(?'sumWcur'(?'sum'(?!0+\.00)(?=.{1,9}(\.|$| ))(?!0(?!\.))\d{1,3}((,| |)\d{3})*(\.\d+)?) ?(?'cur'р|руб|руб\.|р\.|рублей|рубля|т|тнг|тнг\.|т\.|тенге))(\s|$)

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

    public class GroupSetting
    {
        public long ChatId { get; set; }

        public List<CurrencyConvertSetting> CurrencyConvertSettings { get; set; } = new();

        //public Currency SourceCurrency { get; set; }

        //public List<Currency> TargetCurrencies { get; set; }
    }

    public class CurrencyConvertSetting
    {
        public Currency Currency { get; set; }

        public List<Currency> TargetCurrencies { get; set; }
    }
}