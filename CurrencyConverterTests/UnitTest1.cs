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
        }

        [TestMethod]
        public void Test003()
        {
            Assert.AreEqual(Helper.DetectCurrency("р"), Currency.Rub);
            Assert.AreEqual(Helper.DetectCurrency("р."), Currency.Rub);
            Assert.AreEqual(Helper.DetectCurrency("ру"), Currency.None);
            Assert.AreEqual(Helper.DetectCurrency("руб"), Currency.Rub);
            Assert.AreEqual(Helper.DetectCurrency("руб."), Currency.Rub);
            Assert.AreEqual(Helper.DetectCurrency("сруб"), Currency.None);
            Assert.AreEqual(Helper.DetectCurrency("ср"), Currency.None);
            Assert.AreEqual(Helper.DetectCurrency("рублей"), Currency.Rub);
            Assert.AreEqual(Helper.DetectCurrency("рубля"), Currency.Rub);
            Assert.AreEqual(Helper.DetectCurrency("срубля"), Currency.None);

            Assert.AreEqual(Helper.DetectCurrency("т"), Currency.Tenge);
            Assert.AreEqual(Helper.DetectCurrency("т."), Currency.Tenge);
            Assert.AreEqual(Helper.DetectCurrency("те"), Currency.None);
            Assert.AreEqual(Helper.DetectCurrency("тнг"), Currency.Tenge);
            Assert.AreEqual(Helper.DetectCurrency("тнг."), Currency.Tenge);
            Assert.AreEqual(Helper.DetectCurrency("стнг"), Currency.None);
            Assert.AreEqual(Helper.DetectCurrency("ст"), Currency.None);
            Assert.AreEqual(Helper.DetectCurrency("тенге"), Currency.Tenge);
            Assert.AreEqual(Helper.DetectCurrency("стенге"), Currency.None);
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
        }
    }
}