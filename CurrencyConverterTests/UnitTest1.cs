using CurrencyConverter;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CurrencyConverterTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Test006()
        {
            var expected = Helper.ParseSumAndCurrency("5000�");
            Assert.AreEqual(expected.Count, 1);
            Assert.AreEqual(expected[0].Sum, 5000m);
            Assert.AreEqual(expected[0].Currency, Currency.Rub);

            expected = Helper.ParseSumAndCurrency("5 000�");
            Assert.AreEqual(expected.Count, 1);
            Assert.AreEqual(expected[0].Sum, 5000m);
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

            //todo
            //expected = ParseSumAndCurrency("���� 5 000 - 7000 � ����");
            //Assert.AreEqual(expected.Count, 2);
            //Assert.AreEqual(expected[0].Sum, 5000m);
            //Assert.AreEqual(expected[0].Currency, Currency.Rub);
            //Assert.AreEqual(expected[1].Sum, 7000m);
            //Assert.AreEqual(expected[1].Currency, Currency.Rub);

            //expected = ParseSumAndCurrency("���� 5 5 000 - 7000 � ����");
            //Assert.AreEqual(expected.Count, 2);
            //Assert.AreEqual(expected[0].Sum, 5000m);
            //Assert.AreEqual(expected[0].Currency, Currency.Rub);
            //Assert.AreEqual(expected[1].Sum, 7000m);
            //Assert.AreEqual(expected[1].Currency, Currency.Rub);

            expected = Helper.ParseSumAndCurrency("���� 5 000� - 7000 ��� ����");
            Assert.AreEqual(expected.Count, 2);
            Assert.AreEqual(expected[0].Sum, 5000m);
            Assert.AreEqual(expected[0].Currency, Currency.Rub);
            Assert.AreEqual(expected[1].Sum, 7000m);
            Assert.AreEqual(expected[1].Currency, Currency.Rub);
        }

        [TestMethod]
        public void Test007()
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
    }
}