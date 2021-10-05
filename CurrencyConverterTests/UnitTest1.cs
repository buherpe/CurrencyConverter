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
            Assert.AreEqual(expected.Count, 1);
            Assert.AreEqual(expected[0].Sum, 5000m);
            Assert.AreEqual(expected[0].Currency, Currency.Rub);

            expected = Helper.ParseSumAndCurrency("5 000р");
            Assert.AreEqual(expected.Count, 1);
            Assert.AreEqual(expected[0].Sum, 5000m);
            Assert.AreEqual(expected[0].Currency, Currency.Rub);

            expected = Helper.ParseSumAndCurrency("5000 р");
            Assert.AreEqual(expected.Count, 1);
            Assert.AreEqual(expected[0].Sum, 5000m);
            Assert.AreEqual(expected[0].Currency, Currency.Rub);

            expected = Helper.ParseSumAndCurrency("5 000 р");
            Assert.AreEqual(expected.Count, 1);
            Assert.AreEqual(expected[0].Sum, 5000m);
            Assert.AreEqual(expected[0].Currency, Currency.Rub);

            expected = Helper.ParseSumAndCurrency("тест 5000р");
            Assert.AreEqual(expected.Count, 1);
            Assert.AreEqual(expected[0].Sum, 5000m);
            Assert.AreEqual(expected[0].Currency, Currency.Rub);

            expected = Helper.ParseSumAndCurrency("5000р тест");
            Assert.AreEqual(expected.Count, 1);
            Assert.AreEqual(expected[0].Sum, 5000m);
            Assert.AreEqual(expected[0].Currency, Currency.Rub);

            expected = Helper.ParseSumAndCurrency("тест 5 000р - 7000 р тест");
            Assert.AreEqual(expected.Count, 2);
            Assert.AreEqual(expected[0].Sum, 5000m);
            Assert.AreEqual(expected[0].Currency, Currency.Rub);
            Assert.AreEqual(expected[1].Sum, 7000m);
            Assert.AreEqual(expected[1].Currency, Currency.Rub);

            expected = Helper.ParseSumAndCurrency("тест 5 000р - 7000 руб тест");
            Assert.AreEqual(expected.Count, 2);
            Assert.AreEqual(expected[0].Sum, 5000m);
            Assert.AreEqual(expected[0].Currency, Currency.Rub);
            Assert.AreEqual(expected[1].Sum, 7000m);
            Assert.AreEqual(expected[1].Currency, Currency.Rub);
        }

        [TestMethod]
        public void Test002_Todo()
        {
            var expected = Helper.ParseSumAndCurrency("тест 5 000 - 7000 р тест");
            Assert.AreEqual(expected.Count, 2);
            Assert.AreEqual(expected[0].Sum, 5000m);
            Assert.AreEqual(expected[0].Currency, Currency.Rub);
            Assert.AreEqual(expected[1].Sum, 7000m);
            Assert.AreEqual(expected[1].Currency, Currency.Rub);

            expected = Helper.ParseSumAndCurrency("тест 5 5 000 - 7000 р тест");
            Assert.AreEqual(expected.Count, 2);
            Assert.AreEqual(expected[0].Sum, 5000m);
            Assert.AreEqual(expected[0].Currency, Currency.Rub);
            Assert.AreEqual(expected[1].Sum, 7000m);
            Assert.AreEqual(expected[1].Currency, Currency.Rub);
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
    }
}