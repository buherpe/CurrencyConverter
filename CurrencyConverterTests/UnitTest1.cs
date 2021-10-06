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
            var expected = Helper.ParseSumAndCurrency("5000�");
            Assert.AreEqual(1, expected.Count);
            Assert.AreEqual(5000m, expected[0].Sum);
            Assert.AreEqual(Currency.Rub, expected[0].Currency);

            expected = Helper.ParseSumAndCurrency("5 000�");
            Assert.AreEqual(1, expected.Count);
            Assert.AreEqual(5000m, expected[0].Sum);
            Assert.AreEqual(expected[0].Currency, Currency.Rub);

            expected = Helper.ParseSumAndCurrency("5000 �");
            Assert.AreEqual(expected.Count, 1);
            Assert.AreEqual(expected[0].Sum, 5000m);
            Assert.AreEqual(expected[0].Currency, Currency.Rub);

            expected = Helper.ParseSumAndCurrency("5 000 �");
            Assert.AreEqual(expected.Count, 1);
            Assert.AreEqual(expected[0].Sum, 5000m);
            Assert.AreEqual(expected[0].Currency, Currency.Rub);

            expected = Helper.ParseSumAndCurrency("���� 5000�");
            Assert.AreEqual(expected.Count, 1);
            Assert.AreEqual(expected[0].Sum, 5000m);
            Assert.AreEqual(expected[0].Currency, Currency.Rub);

            expected = Helper.ParseSumAndCurrency("5000� ����");
            Assert.AreEqual(expected.Count, 1);
            Assert.AreEqual(expected[0].Sum, 5000m);
            Assert.AreEqual(expected[0].Currency, Currency.Rub);

            expected = Helper.ParseSumAndCurrency("���� 5 000� - 7000 � ����");
            Assert.AreEqual(expected.Count, 2);
            Assert.AreEqual(expected[0].Sum, 5000m);
            Assert.AreEqual(expected[0].Currency, Currency.Rub);
            Assert.AreEqual(expected[1].Sum, 7000m);
            Assert.AreEqual(expected[1].Currency, Currency.Rub);

            expected = Helper.ParseSumAndCurrency("���� 5 000� - 7000 ��� ����");
            Assert.AreEqual(expected.Count, 2);
            Assert.AreEqual(expected[0].Sum, 5000m);
            Assert.AreEqual(expected[0].Currency, Currency.Rub);
            Assert.AreEqual(expected[1].Sum, 7000m);
            Assert.AreEqual(expected[1].Currency, Currency.Rub);
        }

        [TestMethod]
        public void Test002_Todo()
        {
            var expected = Helper.ParseSumAndCurrency("���� 5 000 - 7000 � ����");
            Assert.AreEqual(expected.Count, 2);
            Assert.AreEqual(expected[0].Sum, 5000m);
            Assert.AreEqual(expected[0].Currency, Currency.Rub);
            Assert.AreEqual(expected[1].Sum, 7000m);
            Assert.AreEqual(expected[1].Currency, Currency.Rub);

            expected = Helper.ParseSumAndCurrency("���� 5 5 000 - 7000 � ����");
            Assert.AreEqual(expected.Count, 2);
            Assert.AreEqual(expected[0].Sum, 5000m);
            Assert.AreEqual(expected[0].Currency, Currency.Rub);
            Assert.AreEqual(expected[1].Sum, 7000m);
            Assert.AreEqual(expected[1].Currency, Currency.Rub);
        }

        [TestMethod]
        public void Test003()
        {
            Assert.AreEqual(Helper.DetectCurrency("�"), Currency.Rub);
            Assert.AreEqual(Helper.DetectCurrency("�."), Currency.Rub);
            Assert.AreEqual(Helper.DetectCurrency("��"), Currency.None);
            Assert.AreEqual(Helper.DetectCurrency("���"), Currency.Rub);
            Assert.AreEqual(Helper.DetectCurrency("���."), Currency.Rub);
            Assert.AreEqual(Helper.DetectCurrency("����"), Currency.None);
            Assert.AreEqual(Helper.DetectCurrency("��"), Currency.None);
            Assert.AreEqual(Helper.DetectCurrency("������"), Currency.Rub);
            Assert.AreEqual(Helper.DetectCurrency("�����"), Currency.Rub);
            Assert.AreEqual(Helper.DetectCurrency("������"), Currency.None);

            Assert.AreEqual(Helper.DetectCurrency("�"), Currency.Tenge);
            Assert.AreEqual(Helper.DetectCurrency("�."), Currency.Tenge);
            Assert.AreEqual(Helper.DetectCurrency("��"), Currency.None);
            Assert.AreEqual(Helper.DetectCurrency("���"), Currency.Tenge);
            Assert.AreEqual(Helper.DetectCurrency("���."), Currency.Tenge);
            Assert.AreEqual(Helper.DetectCurrency("����"), Currency.None);
            Assert.AreEqual(Helper.DetectCurrency("��"), Currency.None);
            Assert.AreEqual(Helper.DetectCurrency("�����"), Currency.Tenge);
            Assert.AreEqual(Helper.DetectCurrency("������"), Currency.None);
        }

        [TestMethod]
        public void Test004()
        {
            Helper.Q();
        }

        [TestMethod]
        public void Test005()
        {
            var expected = Helper.ParseSumAndCurrency("���� � ����� ���������...");
            Assert.AreEqual(expected.Count, 0);
        }
    }
}