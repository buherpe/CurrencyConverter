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
            var actual = Helper.ParseSumAndCurrency("5000�");
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(5000m, actual[0].Sum);
            Assert.AreEqual(Currency.Rub, actual[0].Currency);

            actual = Helper.ParseSumAndCurrency("5 000�");
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(5000m, actual[0].Sum);
            Assert.AreEqual(Currency.Rub, actual[0].Currency);

            actual = Helper.ParseSumAndCurrency("5000 �");
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(5000m, actual[0].Sum);
            Assert.AreEqual(Currency.Rub, actual[0].Currency);

            actual = Helper.ParseSumAndCurrency("5 000 �");
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(5000m, actual[0].Sum);
            Assert.AreEqual(Currency.Rub, actual[0].Currency);

            actual = Helper.ParseSumAndCurrency("���� 5000�");
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(5000m, actual[0].Sum);
            Assert.AreEqual(Currency.Rub, actual[0].Currency);

            actual = Helper.ParseSumAndCurrency("5000� ����");
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(5000m, actual[0].Sum);
            Assert.AreEqual(Currency.Rub, actual[0].Currency);

            actual = Helper.ParseSumAndCurrency("���� 5 000� - 7000 � ����");
            Assert.AreEqual(2, actual.Count);
            Assert.AreEqual(5000m, actual[0].Sum);
            Assert.AreEqual(Currency.Rub, actual[0].Currency);
            Assert.AreEqual(7000m, actual[1].Sum);
            Assert.AreEqual(Currency.Rub, actual[1].Currency);

            actual = Helper.ParseSumAndCurrency("���� 5 000� - 7000 ��� ����");
            Assert.AreEqual(2, actual.Count);
            Assert.AreEqual(5000m, actual[0].Sum);
            Assert.AreEqual(Currency.Rub, actual[0].Currency);
            Assert.AreEqual(7000m, actual[1].Sum);
            Assert.AreEqual(Currency.Rub, actual[1].Currency);

            actual = Helper.ParseSumAndCurrency("���� 5 5 000� - 7000 ��� ����");
            Assert.AreEqual(2, actual.Count);
            Assert.AreEqual(5000m, actual[0].Sum);
            Assert.AreEqual(Currency.Rub, actual[0].Currency);
            Assert.AreEqual(7000m, actual[1].Sum);
            Assert.AreEqual(Currency.Rub, actual[1].Currency);

            actual = Helper.ParseSumAndCurrency("���� 5 000� - 7 7000 ��� ����");
            Assert.AreEqual(2, actual.Count);
            Assert.AreEqual(5000m, actual[0].Sum);
            Assert.AreEqual(Currency.Rub, actual[0].Currency);
            Assert.AreEqual(7000m, actual[1].Sum);
            Assert.AreEqual(Currency.Rub, actual[1].Currency);

            actual = Helper.ParseSumAndCurrency("���� 5 5 000� - 7 7000 ��� ����");
            Assert.AreEqual(2, actual.Count);
            Assert.AreEqual(5000m, actual[0].Sum);
            Assert.AreEqual(Currency.Rub, actual[0].Currency);
            Assert.AreEqual(7000m, actual[1].Sum);
            Assert.AreEqual(Currency.Rub, actual[1].Currency);

            actual = Helper.ParseSumAndCurrency("���� 5 5000� - 7 7 000 ��� ����");
            Assert.AreEqual(2, actual.Count);
            Assert.AreEqual(5000m, actual[0].Sum);
            Assert.AreEqual(Currency.Rub, actual[0].Currency);
            Assert.AreEqual(7000m, actual[1].Sum);
            Assert.AreEqual(Currency.Rub, actual[1].Currency);

            actual = Helper.ParseSumAndCurrency("�7000");
            Assert.AreEqual(0, actual.Count);

            actual = Helper.ParseSumAndCurrency("� 7000");
            Assert.AreEqual(0, actual.Count);

            actual = Helper.ParseSumAndCurrency("�7 000");
            Assert.AreEqual(0, actual.Count);

            actual = Helper.ParseSumAndCurrency("� 7 000");
            Assert.AreEqual(0, actual.Count);


            actual = Helper.ParseSumAndCurrency("5000�.");
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(5000m, actual[0].Sum);
            Assert.AreEqual(Currency.Rub, actual[0].Currency);
        }

        [TestMethod]
        public void Test002_Todo()
        {
            var expected = Helper.ParseSumAndCurrency("���� 5 000 - 7000 � ����");
            Assert.AreEqual(2, expected.Count);
            Assert.AreEqual(5000m, expected[0].Sum);
            Assert.AreEqual(Currency.Rub, expected[0].Currency);
            Assert.AreEqual(7000m, expected[1].Sum);
            Assert.AreEqual(Currency.Rub, expected[1].Currency);

            expected = Helper.ParseSumAndCurrency("���� 5 5 000 - 7000 � ����");
            Assert.AreEqual(2, expected.Count);
            Assert.AreEqual(5000m, expected[0].Sum);
            Assert.AreEqual(Currency.Rub, expected[0].Currency);
            Assert.AreEqual(7000m, expected[1].Sum);
            Assert.AreEqual(Currency.Rub, expected[1].Currency);

            expected = Helper.ParseSumAndCurrency("1.00� - 5.8 � ����");
            Assert.AreEqual(2, expected.Count);
            Assert.AreEqual(1m, expected[0].Sum);
            Assert.AreEqual(Currency.Rub, expected[0].Currency);
            Assert.AreEqual(5.8m, expected[1].Sum);
            Assert.AreEqual(Currency.Tenge, expected[1].Currency);

            expected = Helper.ParseSumAndCurrency("1,00� - 5,8 � ����");
            Assert.AreEqual(2, expected.Count);
            Assert.AreEqual(1m, expected[0].Sum);
            Assert.AreEqual(Currency.Rub, expected[0].Currency);
            Assert.AreEqual(5.8m, expected[1].Sum);
            Assert.AreEqual(Currency.Tenge, expected[1].Currency);
        }

        [TestMethod]
        public void Test003()
        {
            Assert.AreEqual(Currency.Rub, Helper.DetectCurrency("�"));
            Assert.AreEqual(Currency.Rub, Helper.DetectCurrency("�."));
            Assert.AreEqual(Currency.None, Helper.DetectCurrency("��"));
            Assert.AreEqual(Currency.Rub, Helper.DetectCurrency("���"));
            Assert.AreEqual(Currency.Rub, Helper.DetectCurrency("���."));
            Assert.AreEqual(Currency.None, Helper.DetectCurrency("����"));
            Assert.AreEqual(Currency.None, Helper.DetectCurrency("��"));
            Assert.AreEqual(Currency.Rub, Helper.DetectCurrency("������"));
            Assert.AreEqual(Currency.Rub, Helper.DetectCurrency("�����"));
            Assert.AreEqual(Currency.None, Helper.DetectCurrency("������"));

            Assert.AreEqual(Currency.Tenge, Helper.DetectCurrency("�"));
            Assert.AreEqual(Currency.Tenge, Helper.DetectCurrency("�."));
            Assert.AreEqual(Currency.None, Helper.DetectCurrency("��"));
            Assert.AreEqual(Currency.Tenge, Helper.DetectCurrency("���"));
            Assert.AreEqual(Currency.Tenge, Helper.DetectCurrency("���."));
            Assert.AreEqual(Currency.None, Helper.DetectCurrency("����"));
            Assert.AreEqual(Currency.None, Helper.DetectCurrency("��"));
            Assert.AreEqual(Currency.Tenge, Helper.DetectCurrency("�����"));
            Assert.AreEqual(Currency.None, Helper.DetectCurrency("������"));
        }

        [TestMethod]
        public void Test004()
        {
            var actualTargetCurrencies = Helper.GetGroupSetting(66227).GetTargetCurrencies(Currency.Dollar);
            Assert.AreEqual(2, actualTargetCurrencies.Count);
            Assert.AreEqual(Currency.Rub, actualTargetCurrencies[0]);
            Assert.AreEqual(Currency.Tenge, actualTargetCurrencies[1]);

            actualTargetCurrencies = Helper.GetGroupSetting(66227).GetTargetCurrencies(Currency.Rub);
            Assert.AreEqual(1, actualTargetCurrencies.Count);
            Assert.AreEqual(Currency.Tenge, actualTargetCurrencies[0]);

            actualTargetCurrencies = Helper.GetGroupSetting(66227).GetTargetCurrencies(Currency.Tenge);
            Assert.AreEqual(1, actualTargetCurrencies.Count);
            Assert.AreEqual(Currency.Rub, actualTargetCurrencies[0]);
        }

        [TestMethod]
        public void Test005()
        {
            var actual = Helper.ParseSumAndCurrency("���� � ����� ���������...");
            Assert.AreEqual(0, actual.Count);

            actual = Helper.ParseSumAndCurrency(" ����� ���������...");
            Assert.AreEqual(0, actual.Count);

            actual = Helper.ParseSumAndCurrency("����� ���������...");
            Assert.AreEqual(0, actual.Count);

            actual = Helper.ParseSumAndCurrency("����� ");
            Assert.AreEqual(0, actual.Count);

            actual = Helper.ParseSumAndCurrency("�����");
            Assert.AreEqual(0, actual.Count);

            actual = Helper.ParseSumAndCurrency("�, �����");
            Assert.AreEqual(0, actual.Count);

            actual = Helper.ParseSumAndCurrency("� �����");
            Assert.AreEqual(0, actual.Count);

            actual = Helper.ParseSumAndCurrency("������");
            Assert.AreEqual(0, actual.Count);

            actual = Helper.ParseSumAndCurrency("100 ������");
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void Test006()
        {
            var actual = Helper.ParseSumAndCurrency("6000$");
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(6000m, actual[0].Sum);
            Assert.AreEqual(Currency.Dollar, actual[0].Currency);
            
            actual = Helper.ParseSumAndCurrency("6000 $");
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(6000m, actual[0].Sum);
            Assert.AreEqual(Currency.Dollar, actual[0].Currency);

            actual = Helper.ParseSumAndCurrency("6 000$");
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(6000m, actual[0].Sum);
            Assert.AreEqual(Currency.Dollar, actual[0].Currency);
            
            actual = Helper.ParseSumAndCurrency("6 000 $");
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(6000m, actual[0].Sum);
            Assert.AreEqual(Currency.Dollar, actual[0].Currency);


            actual = Helper.ParseSumAndCurrency("6000$ ����");
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(6000m, actual[0].Sum);
            Assert.AreEqual(Currency.Dollar, actual[0].Currency);
            
            actual = Helper.ParseSumAndCurrency("6000 $ ����");
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(6000m, actual[0].Sum);
            Assert.AreEqual(Currency.Dollar, actual[0].Currency);

            actual = Helper.ParseSumAndCurrency("6 000$ ����");
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(6000m, actual[0].Sum);
            Assert.AreEqual(Currency.Dollar, actual[0].Currency);
            
            actual = Helper.ParseSumAndCurrency("6 000 $ ����");
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(6000m, actual[0].Sum);
            Assert.AreEqual(Currency.Dollar, actual[0].Currency);


            actual = Helper.ParseSumAndCurrency("���� 6000$");
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(6000m, actual[0].Sum);
            Assert.AreEqual(Currency.Dollar, actual[0].Currency);

            actual = Helper.ParseSumAndCurrency("���� 6000 $");
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(6000m, actual[0].Sum);
            Assert.AreEqual(Currency.Dollar, actual[0].Currency);

            actual = Helper.ParseSumAndCurrency("���� 6 000$");
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(6000m, actual[0].Sum);
            Assert.AreEqual(Currency.Dollar, actual[0].Currency);

            actual = Helper.ParseSumAndCurrency("���� 6 000 $");
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(6000m, actual[0].Sum);
            Assert.AreEqual(Currency.Dollar, actual[0].Currency);


            actual = Helper.ParseSumAndCurrency("���� 6000$ ����");
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(6000m, actual[0].Sum);
            Assert.AreEqual(Currency.Dollar, actual[0].Currency);

            actual = Helper.ParseSumAndCurrency("���� 6000 $ ����");
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(6000m, actual[0].Sum);
            Assert.AreEqual(Currency.Dollar, actual[0].Currency);

            actual = Helper.ParseSumAndCurrency("���� 6 000$ ����");
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(6000m, actual[0].Sum);
            Assert.AreEqual(Currency.Dollar, actual[0].Currency);

            actual = Helper.ParseSumAndCurrency("���� 6 000 $ ����");
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(6000m, actual[0].Sum);
            Assert.AreEqual(Currency.Dollar, actual[0].Currency);
        }
        
        [TestMethod]
        public void Test007_Todo()
        {
            var actual = Helper.ParseSumAndCurrency("$6000");
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(6000m, actual[0].Sum);
            Assert.AreEqual(Currency.Dollar, actual[0].Currency);

            actual = Helper.ParseSumAndCurrency("$ 6000");
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(6000m, actual[0].Sum);
            Assert.AreEqual(Currency.Dollar, actual[0].Currency);

            actual = Helper.ParseSumAndCurrency("$ 6 000");
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(6000m, actual[0].Sum);
            Assert.AreEqual(Currency.Dollar, actual[0].Currency);

            actual = Helper.ParseSumAndCurrency("$ 6 000");
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(6000m, actual[0].Sum);
            Assert.AreEqual(Currency.Dollar, actual[0].Currency);


            actual = Helper.ParseSumAndCurrency("$6000 ����");
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(6000m, actual[0].Sum);
            Assert.AreEqual(Currency.Dollar, actual[0].Currency);

            actual = Helper.ParseSumAndCurrency("$ 6000 ����");
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(6000m, actual[0].Sum);
            Assert.AreEqual(Currency.Dollar, actual[0].Currency);

            actual = Helper.ParseSumAndCurrency("$ 6 000 ����");
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(6000m, actual[0].Sum);
            Assert.AreEqual(Currency.Dollar, actual[0].Currency);

            actual = Helper.ParseSumAndCurrency("$ 6 000 ����");
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(6000m, actual[0].Sum);
            Assert.AreEqual(Currency.Dollar, actual[0].Currency);


            actual = Helper.ParseSumAndCurrency("���� $6000");
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(6000m, actual[0].Sum);
            Assert.AreEqual(Currency.Dollar, actual[0].Currency);

            actual = Helper.ParseSumAndCurrency("���� $ 6000");
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(6000m, actual[0].Sum);
            Assert.AreEqual(Currency.Dollar, actual[0].Currency);

            actual = Helper.ParseSumAndCurrency("���� $ 6 000");
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(6000m, actual[0].Sum);
            Assert.AreEqual(Currency.Dollar, actual[0].Currency);

            actual = Helper.ParseSumAndCurrency("���� $ 6 000");
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(6000m, actual[0].Sum);
            Assert.AreEqual(Currency.Dollar, actual[0].Currency);


            actual = Helper.ParseSumAndCurrency("���� $6000 ����");
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(6000m, actual[0].Sum);
            Assert.AreEqual(Currency.Dollar, actual[0].Currency);

            actual = Helper.ParseSumAndCurrency("���� $ 6000 ����");
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(6000m, actual[0].Sum);
            Assert.AreEqual(Currency.Dollar, actual[0].Currency);

            actual = Helper.ParseSumAndCurrency("���� $ 6 000 ����");
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(6000m, actual[0].Sum);
            Assert.AreEqual(Currency.Dollar, actual[0].Currency);

            actual = Helper.ParseSumAndCurrency("���� $ 6 000 ����");
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(6000m, actual[0].Sum);
            Assert.AreEqual(Currency.Dollar, actual[0].Currency);
        }

        //[TestMethod]
        //public void Test008()
        //{
        //    var actual = Helper.GetExchangeRate(Currency.Dollar, Currency.Rub);
        //    Assert.AreEqual(72.47m, actual);

        //    actual = Helper.GetExchangeRate(Currency.Dollar, Currency.Tenge);
        //    Assert.AreEqual(423.98m, actual);

        //    actual = Helper.GetExchangeRate(Currency.Rub, Currency.Tenge);
        //    Assert.AreEqual(5.8504208638057127087070511936m, actual);

        //    actual = Helper.GetExchangeRate(Currency.Tenge, Currency.Rub);
        //    Assert.AreEqual(0.1709278739563186942780319826m, actual);

        //    actual = Helper.GetExchangeRate(Currency.Tenge, Currency.Dollar);
        //    Assert.AreEqual(0.0023586018208406056889475919m, actual);

        //    actual = Helper.GetExchangeRate(Currency.Rub, Currency.Dollar);
        //    Assert.AreEqual(0.0137988133020560231820063475m, actual);
        //}

        [TestMethod]
        public void Test009_Todo()
        {
            var actual = Helper.ParseSumAndCurrency("5� ���");
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(5000m, actual[0].Sum);
            Assert.AreEqual(Currency.Rub, actual[0].Currency);

            actual = Helper.ParseSumAndCurrency("150� ���");
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(150000m, actual[0].Sum);
            Assert.AreEqual(Currency.Rub, actual[0].Currency);
        }
    }
}