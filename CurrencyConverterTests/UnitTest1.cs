using System;
using CurrencyConverter;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CurrencyConverterTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Test001()
        {
            var expected = Helper.ParseSumAndCurrency("5000р");
            Assert.AreEqual(1, expected.Count);
            Assert.AreEqual(5000m, expected[0].Sum);
            Assert.AreEqual(Currency.Rub, expected[0].Currency);

            expected = Helper.ParseSumAndCurrency("5 000р");
            Assert.AreEqual(1, expected.Count);
            Assert.AreEqual(5000m, expected[0].Sum);
            Assert.AreEqual(Currency.Rub, expected[0].Currency);

            expected = Helper.ParseSumAndCurrency("5000 р");
            Assert.AreEqual(1, expected.Count);
            Assert.AreEqual(5000m, expected[0].Sum);
            Assert.AreEqual(Currency.Rub, expected[0].Currency);

            expected = Helper.ParseSumAndCurrency("5 000 р");
            Assert.AreEqual(1, expected.Count);
            Assert.AreEqual(5000m, expected[0].Sum);
            Assert.AreEqual(Currency.Rub, expected[0].Currency);

            expected = Helper.ParseSumAndCurrency("тест 5000р");
            Assert.AreEqual(1, expected.Count);
            Assert.AreEqual(5000m, expected[0].Sum);
            Assert.AreEqual(Currency.Rub, expected[0].Currency);

            expected = Helper.ParseSumAndCurrency("5000р тест");
            Assert.AreEqual(1, expected.Count);
            Assert.AreEqual(5000m, expected[0].Sum);
            Assert.AreEqual(Currency.Rub, expected[0].Currency);

            expected = Helper.ParseSumAndCurrency("тест 5 000р - 7000 р тест");
            Assert.AreEqual(2, expected.Count);
            Assert.AreEqual(5000m, expected[0].Sum);
            Assert.AreEqual(Currency.Rub, expected[0].Currency);
            Assert.AreEqual(7000m, expected[1].Sum);
            Assert.AreEqual(Currency.Rub, expected[1].Currency);

            expected = Helper.ParseSumAndCurrency("тест 5 000р - 7000 руб тест");
            Assert.AreEqual(2, expected.Count);
            Assert.AreEqual(5000m, expected[0].Sum);
            Assert.AreEqual(Currency.Rub, expected[0].Currency);
            Assert.AreEqual(7000m, expected[1].Sum);
            Assert.AreEqual(Currency.Rub, expected[1].Currency);

            expected = Helper.ParseSumAndCurrency("тест 5 5 000р - 7000 руб тест");
            Assert.AreEqual(2, expected.Count);
            Assert.AreEqual(5000m, expected[0].Sum);
            Assert.AreEqual(Currency.Rub, expected[0].Currency);
            Assert.AreEqual(7000m, expected[1].Sum);
            Assert.AreEqual(Currency.Rub, expected[1].Currency);

            expected = Helper.ParseSumAndCurrency("тест 5 000р - 7 7000 руб тест");
            Assert.AreEqual(2, expected.Count);
            Assert.AreEqual(5000m, expected[0].Sum);
            Assert.AreEqual(Currency.Rub, expected[0].Currency);
            Assert.AreEqual(7000m, expected[1].Sum);
            Assert.AreEqual(Currency.Rub, expected[1].Currency);

            expected = Helper.ParseSumAndCurrency("тест 5 5 000р - 7 7000 руб тест");
            Assert.AreEqual(2, expected.Count);
            Assert.AreEqual(5000m, expected[0].Sum);
            Assert.AreEqual(Currency.Rub, expected[0].Currency);
            Assert.AreEqual(7000m, expected[1].Sum);
            Assert.AreEqual(Currency.Rub, expected[1].Currency);

            expected = Helper.ParseSumAndCurrency("тест 5 5000р - 7 7 000 руб тест");
            Assert.AreEqual(2, expected.Count);
            Assert.AreEqual(5000m, expected[0].Sum);
            Assert.AreEqual(Currency.Rub, expected[0].Currency);
            Assert.AreEqual(7000m, expected[1].Sum);
            Assert.AreEqual(Currency.Rub, expected[1].Currency);

            expected = Helper.ParseSumAndCurrency("р7000");
            Assert.AreEqual(0, expected.Count);

            expected = Helper.ParseSumAndCurrency("р 7000");
            Assert.AreEqual(0, expected.Count);

            expected = Helper.ParseSumAndCurrency("р7 000");
            Assert.AreEqual(0, expected.Count);

            expected = Helper.ParseSumAndCurrency("р 7 000");
            Assert.AreEqual(0, expected.Count);
        }

        [TestMethod]
        public void Test002_Todo()
        {
            var expected = Helper.ParseSumAndCurrency("тест 5 000 - 7000 р тест");
            Assert.AreEqual(2, expected.Count);
            Assert.AreEqual(5000m, expected[0].Sum);
            Assert.AreEqual(Currency.Rub, expected[0].Currency);
            Assert.AreEqual(7000m, expected[1].Sum);
            Assert.AreEqual(Currency.Rub, expected[1].Currency);

            expected = Helper.ParseSumAndCurrency("тест 5 5 000 - 7000 р тест");
            Assert.AreEqual(2, expected.Count);
            Assert.AreEqual(5000m, expected[0].Sum);
            Assert.AreEqual(Currency.Rub, expected[0].Currency);
            Assert.AreEqual(7000m, expected[1].Sum);
            Assert.AreEqual(Currency.Rub, expected[1].Currency);

            expected = Helper.ParseSumAndCurrency("1.00р - 5.8 т тест");
            Assert.AreEqual(2, expected.Count);
            Assert.AreEqual(1m, expected[0].Sum);
            Assert.AreEqual(Currency.Rub, expected[0].Currency);
            Assert.AreEqual(5.8m, expected[1].Sum);
            Assert.AreEqual(Currency.Tenge, expected[1].Currency);

            expected = Helper.ParseSumAndCurrency("1,00р - 5,8 т тест");
            Assert.AreEqual(2, expected.Count);
            Assert.AreEqual(1m, expected[0].Sum);
            Assert.AreEqual(Currency.Rub, expected[0].Currency);
            Assert.AreEqual(5.8m, expected[1].Sum);
            Assert.AreEqual(Currency.Tenge, expected[1].Currency);
        }

        [TestMethod]
        public void Test003()
        {
            Assert.AreEqual(Currency.Rub, Helper.DetectCurrency("р"));
            Assert.AreEqual(Currency.Rub, Helper.DetectCurrency("р."));
            Assert.AreEqual(Currency.None, Helper.DetectCurrency("ру"));
            Assert.AreEqual(Currency.Rub, Helper.DetectCurrency("руб"));
            Assert.AreEqual(Currency.Rub, Helper.DetectCurrency("руб."));
            Assert.AreEqual(Currency.None, Helper.DetectCurrency("сруб"));
            Assert.AreEqual(Currency.None, Helper.DetectCurrency("ср"));
            Assert.AreEqual(Currency.Rub, Helper.DetectCurrency("рублей"));
            Assert.AreEqual(Currency.Rub, Helper.DetectCurrency("рубля"));
            Assert.AreEqual(Currency.None, Helper.DetectCurrency("срубля"));

            Assert.AreEqual(Currency.Tenge, Helper.DetectCurrency("т"));
            Assert.AreEqual(Currency.Tenge, Helper.DetectCurrency("т."));
            Assert.AreEqual(Currency.None, Helper.DetectCurrency("те"));
            Assert.AreEqual(Currency.Tenge, Helper.DetectCurrency("тнг"));
            Assert.AreEqual(Currency.Tenge, Helper.DetectCurrency("тнг."));
            Assert.AreEqual(Currency.None, Helper.DetectCurrency("стнг"));
            Assert.AreEqual(Currency.None, Helper.DetectCurrency("ст"));
            Assert.AreEqual(Currency.Tenge, Helper.DetectCurrency("тенге"));
            Assert.AreEqual(Currency.None, Helper.DetectCurrency("стенге"));
        }

        [TestMethod]
        public void Test004()
        {
            Helper.Q();
        }

        [TestMethod]
        public void Test005()
        {
            var expected = Helper.ParseSumAndCurrency("чтоб в тенге переводил...");
            Assert.AreEqual(0, expected.Count);

            expected = Helper.ParseSumAndCurrency(" тенге переводил...");
            Assert.AreEqual(0, expected.Count);

            expected = Helper.ParseSumAndCurrency("тенге переводил...");
            Assert.AreEqual(0, expected.Count);

            expected = Helper.ParseSumAndCurrency("тенге ");
            Assert.AreEqual(0, expected.Count);

            expected = Helper.ParseSumAndCurrency("тенге");
            Assert.AreEqual(0, expected.Count);

            expected = Helper.ParseSumAndCurrency("о, тенге");
            Assert.AreEqual(0, expected.Count);

            expected = Helper.ParseSumAndCurrency("о тенге");
            Assert.AreEqual(0, expected.Count);

            expected = Helper.ParseSumAndCurrency("отенге");
            Assert.AreEqual(0, expected.Count);

            expected = Helper.ParseSumAndCurrency("100 отенге");
            Assert.AreEqual(0, expected.Count);
        }

        [TestMethod]
        public void Test006()
        {
            var expected = Helper.ParseSumAndCurrency("6000$");
            Assert.AreEqual(1, expected.Count);
            Assert.AreEqual(6000m, expected[0].Sum);
            Assert.AreEqual(Currency.Dollar, expected[0].Currency);
            
            expected = Helper.ParseSumAndCurrency("6000 $");
            Assert.AreEqual(1, expected.Count);
            Assert.AreEqual(6000m, expected[0].Sum);
            Assert.AreEqual(Currency.Dollar, expected[0].Currency);

            expected = Helper.ParseSumAndCurrency("6 000$");
            Assert.AreEqual(1, expected.Count);
            Assert.AreEqual(6000m, expected[0].Sum);
            Assert.AreEqual(Currency.Dollar, expected[0].Currency);
            
            expected = Helper.ParseSumAndCurrency("6 000 $");
            Assert.AreEqual(1, expected.Count);
            Assert.AreEqual(6000m, expected[0].Sum);
            Assert.AreEqual(Currency.Dollar, expected[0].Currency);


            expected = Helper.ParseSumAndCurrency("6000$ тест");
            Assert.AreEqual(1, expected.Count);
            Assert.AreEqual(6000m, expected[0].Sum);
            Assert.AreEqual(Currency.Dollar, expected[0].Currency);
            
            expected = Helper.ParseSumAndCurrency("6000 $ тест");
            Assert.AreEqual(1, expected.Count);
            Assert.AreEqual(6000m, expected[0].Sum);
            Assert.AreEqual(Currency.Dollar, expected[0].Currency);

            expected = Helper.ParseSumAndCurrency("6 000$ тест");
            Assert.AreEqual(1, expected.Count);
            Assert.AreEqual(6000m, expected[0].Sum);
            Assert.AreEqual(Currency.Dollar, expected[0].Currency);
            
            expected = Helper.ParseSumAndCurrency("6 000 $ тест");
            Assert.AreEqual(1, expected.Count);
            Assert.AreEqual(6000m, expected[0].Sum);
            Assert.AreEqual(Currency.Dollar, expected[0].Currency);


            expected = Helper.ParseSumAndCurrency("тест 6000$");
            Assert.AreEqual(1, expected.Count);
            Assert.AreEqual(6000m, expected[0].Sum);
            Assert.AreEqual(Currency.Dollar, expected[0].Currency);

            expected = Helper.ParseSumAndCurrency("тест 6000 $");
            Assert.AreEqual(1, expected.Count);
            Assert.AreEqual(6000m, expected[0].Sum);
            Assert.AreEqual(Currency.Dollar, expected[0].Currency);

            expected = Helper.ParseSumAndCurrency("тест 6 000$");
            Assert.AreEqual(1, expected.Count);
            Assert.AreEqual(6000m, expected[0].Sum);
            Assert.AreEqual(Currency.Dollar, expected[0].Currency);

            expected = Helper.ParseSumAndCurrency("тест 6 000 $");
            Assert.AreEqual(1, expected.Count);
            Assert.AreEqual(6000m, expected[0].Sum);
            Assert.AreEqual(Currency.Dollar, expected[0].Currency);


            expected = Helper.ParseSumAndCurrency("тест 6000$ тест");
            Assert.AreEqual(1, expected.Count);
            Assert.AreEqual(6000m, expected[0].Sum);
            Assert.AreEqual(Currency.Dollar, expected[0].Currency);

            expected = Helper.ParseSumAndCurrency("тест 6000 $ тест");
            Assert.AreEqual(1, expected.Count);
            Assert.AreEqual(6000m, expected[0].Sum);
            Assert.AreEqual(Currency.Dollar, expected[0].Currency);

            expected = Helper.ParseSumAndCurrency("тест 6 000$ тест");
            Assert.AreEqual(1, expected.Count);
            Assert.AreEqual(6000m, expected[0].Sum);
            Assert.AreEqual(Currency.Dollar, expected[0].Currency);

            expected = Helper.ParseSumAndCurrency("тест 6 000 $ тест");
            Assert.AreEqual(1, expected.Count);
            Assert.AreEqual(6000m, expected[0].Sum);
            Assert.AreEqual(Currency.Dollar, expected[0].Currency);
        }
        
        [TestMethod]
        public void Test007_Todo()
        {
            var expected = Helper.ParseSumAndCurrency("$6000");
            Assert.AreEqual(1, expected.Count);
            Assert.AreEqual(6000m, expected[0].Sum);
            Assert.AreEqual(Currency.Dollar, expected[0].Currency);

            expected = Helper.ParseSumAndCurrency("$ 6000");
            Assert.AreEqual(1, expected.Count);
            Assert.AreEqual(6000m, expected[0].Sum);
            Assert.AreEqual(Currency.Dollar, expected[0].Currency);

            expected = Helper.ParseSumAndCurrency("$ 6 000");
            Assert.AreEqual(1, expected.Count);
            Assert.AreEqual(6000m, expected[0].Sum);
            Assert.AreEqual(Currency.Dollar, expected[0].Currency);

            expected = Helper.ParseSumAndCurrency("$ 6 000");
            Assert.AreEqual(1, expected.Count);
            Assert.AreEqual(6000m, expected[0].Sum);
            Assert.AreEqual(Currency.Dollar, expected[0].Currency);


            expected = Helper.ParseSumAndCurrency("$6000 тест");
            Assert.AreEqual(1, expected.Count);
            Assert.AreEqual(6000m, expected[0].Sum);
            Assert.AreEqual(Currency.Dollar, expected[0].Currency);

            expected = Helper.ParseSumAndCurrency("$ 6000 тест");
            Assert.AreEqual(1, expected.Count);
            Assert.AreEqual(6000m, expected[0].Sum);
            Assert.AreEqual(Currency.Dollar, expected[0].Currency);

            expected = Helper.ParseSumAndCurrency("$ 6 000 тест");
            Assert.AreEqual(1, expected.Count);
            Assert.AreEqual(6000m, expected[0].Sum);
            Assert.AreEqual(Currency.Dollar, expected[0].Currency);

            expected = Helper.ParseSumAndCurrency("$ 6 000 тест");
            Assert.AreEqual(1, expected.Count);
            Assert.AreEqual(6000m, expected[0].Sum);
            Assert.AreEqual(Currency.Dollar, expected[0].Currency);


            expected = Helper.ParseSumAndCurrency("тест $6000");
            Assert.AreEqual(1, expected.Count);
            Assert.AreEqual(6000m, expected[0].Sum);
            Assert.AreEqual(Currency.Dollar, expected[0].Currency);

            expected = Helper.ParseSumAndCurrency("тест $ 6000");
            Assert.AreEqual(1, expected.Count);
            Assert.AreEqual(6000m, expected[0].Sum);
            Assert.AreEqual(Currency.Dollar, expected[0].Currency);

            expected = Helper.ParseSumAndCurrency("тест $ 6 000");
            Assert.AreEqual(1, expected.Count);
            Assert.AreEqual(6000m, expected[0].Sum);
            Assert.AreEqual(Currency.Dollar, expected[0].Currency);

            expected = Helper.ParseSumAndCurrency("тест $ 6 000");
            Assert.AreEqual(1, expected.Count);
            Assert.AreEqual(6000m, expected[0].Sum);
            Assert.AreEqual(Currency.Dollar, expected[0].Currency);


            expected = Helper.ParseSumAndCurrency("тест $6000 тест");
            Assert.AreEqual(1, expected.Count);
            Assert.AreEqual(6000m, expected[0].Sum);
            Assert.AreEqual(Currency.Dollar, expected[0].Currency);

            expected = Helper.ParseSumAndCurrency("тест $ 6000 тест");
            Assert.AreEqual(1, expected.Count);
            Assert.AreEqual(6000m, expected[0].Sum);
            Assert.AreEqual(Currency.Dollar, expected[0].Currency);

            expected = Helper.ParseSumAndCurrency("тест $ 6 000 тест");
            Assert.AreEqual(1, expected.Count);
            Assert.AreEqual(6000m, expected[0].Sum);
            Assert.AreEqual(Currency.Dollar, expected[0].Currency);

            expected = Helper.ParseSumAndCurrency("тест $ 6 000 тест");
            Assert.AreEqual(1, expected.Count);
            Assert.AreEqual(6000m, expected[0].Sum);
            Assert.AreEqual(Currency.Dollar, expected[0].Currency);
        }
    }
}