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
            Assert.AreEqual(Currency.Rub, expected[0].Currency);

            expected = Helper.ParseSumAndCurrency("5000 �");
            Assert.AreEqual(1, expected.Count);
            Assert.AreEqual(5000m, expected[0].Sum);
            Assert.AreEqual(Currency.Rub, expected[0].Currency);

            expected = Helper.ParseSumAndCurrency("5 000 �");
            Assert.AreEqual(1, expected.Count);
            Assert.AreEqual(5000m, expected[0].Sum);
            Assert.AreEqual(Currency.Rub, expected[0].Currency);

            expected = Helper.ParseSumAndCurrency("���� 5000�");
            Assert.AreEqual(1, expected.Count);
            Assert.AreEqual(5000m, expected[0].Sum);
            Assert.AreEqual(Currency.Rub, expected[0].Currency);

            expected = Helper.ParseSumAndCurrency("5000� ����");
            Assert.AreEqual(1, expected.Count);
            Assert.AreEqual(5000m, expected[0].Sum);
            Assert.AreEqual(Currency.Rub, expected[0].Currency);

            expected = Helper.ParseSumAndCurrency("���� 5 000� - 7000 � ����");
            Assert.AreEqual(2, expected.Count);
            Assert.AreEqual(5000m, expected[0].Sum);
            Assert.AreEqual(Currency.Rub, expected[0].Currency);
            Assert.AreEqual(7000m, expected[1].Sum);
            Assert.AreEqual(Currency.Rub, expected[1].Currency);

            expected = Helper.ParseSumAndCurrency("���� 5 000� - 7000 ��� ����");
            Assert.AreEqual(2, expected.Count);
            Assert.AreEqual(5000m, expected[0].Sum);
            Assert.AreEqual(Currency.Rub, expected[0].Currency);
            Assert.AreEqual(7000m, expected[1].Sum);
            Assert.AreEqual(Currency.Rub, expected[1].Currency);

            expected = Helper.ParseSumAndCurrency("���� 5 5 000� - 7000 ��� ����");
            Assert.AreEqual(2, expected.Count);
            Assert.AreEqual(5000m, expected[0].Sum);
            Assert.AreEqual(Currency.Rub, expected[0].Currency);
            Assert.AreEqual(7000m, expected[1].Sum);
            Assert.AreEqual(Currency.Rub, expected[1].Currency);

            expected = Helper.ParseSumAndCurrency("���� 5 000� - 7 7000 ��� ����");
            Assert.AreEqual(2, expected.Count);
            Assert.AreEqual(5000m, expected[0].Sum);
            Assert.AreEqual(Currency.Rub, expected[0].Currency);
            Assert.AreEqual(7000m, expected[1].Sum);
            Assert.AreEqual(Currency.Rub, expected[1].Currency);

            expected = Helper.ParseSumAndCurrency("���� 5 5 000� - 7 7000 ��� ����");
            Assert.AreEqual(2, expected.Count);
            Assert.AreEqual(5000m, expected[0].Sum);
            Assert.AreEqual(Currency.Rub, expected[0].Currency);
            Assert.AreEqual(7000m, expected[1].Sum);
            Assert.AreEqual(Currency.Rub, expected[1].Currency);

            expected = Helper.ParseSumAndCurrency("���� 5 5000� - 7 7 000 ��� ����");
            Assert.AreEqual(2, expected.Count);
            Assert.AreEqual(5000m, expected[0].Sum);
            Assert.AreEqual(Currency.Rub, expected[0].Currency);
            Assert.AreEqual(7000m, expected[1].Sum);
            Assert.AreEqual(Currency.Rub, expected[1].Currency);

            expected = Helper.ParseSumAndCurrency("�7000");
            Assert.AreEqual(0, expected.Count);

            expected = Helper.ParseSumAndCurrency("� 7000");
            Assert.AreEqual(0, expected.Count);

            expected = Helper.ParseSumAndCurrency("�7 000");
            Assert.AreEqual(0, expected.Count);

            expected = Helper.ParseSumAndCurrency("� 7 000");
            Assert.AreEqual(0, expected.Count);
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
            Helper.Q();
        }

        [TestMethod]
        public void Test005()
        {
            var expected = Helper.ParseSumAndCurrency("���� � ����� ���������...");
            Assert.AreEqual(0, expected.Count);

            expected = Helper.ParseSumAndCurrency(" ����� ���������...");
            Assert.AreEqual(0, expected.Count);

            expected = Helper.ParseSumAndCurrency("����� ���������...");
            Assert.AreEqual(0, expected.Count);

            expected = Helper.ParseSumAndCurrency("����� ");
            Assert.AreEqual(0, expected.Count);

            expected = Helper.ParseSumAndCurrency("�����");
            Assert.AreEqual(0, expected.Count);

            expected = Helper.ParseSumAndCurrency("�, �����");
            Assert.AreEqual(0, expected.Count);

            expected = Helper.ParseSumAndCurrency("� �����");
            Assert.AreEqual(0, expected.Count);

            expected = Helper.ParseSumAndCurrency("������");
            Assert.AreEqual(0, expected.Count);

            expected = Helper.ParseSumAndCurrency("100 ������");
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


            expected = Helper.ParseSumAndCurrency("6000$ ����");
            Assert.AreEqual(1, expected.Count);
            Assert.AreEqual(6000m, expected[0].Sum);
            Assert.AreEqual(Currency.Dollar, expected[0].Currency);
            
            expected = Helper.ParseSumAndCurrency("6000 $ ����");
            Assert.AreEqual(1, expected.Count);
            Assert.AreEqual(6000m, expected[0].Sum);
            Assert.AreEqual(Currency.Dollar, expected[0].Currency);

            expected = Helper.ParseSumAndCurrency("6 000$ ����");
            Assert.AreEqual(1, expected.Count);
            Assert.AreEqual(6000m, expected[0].Sum);
            Assert.AreEqual(Currency.Dollar, expected[0].Currency);
            
            expected = Helper.ParseSumAndCurrency("6 000 $ ����");
            Assert.AreEqual(1, expected.Count);
            Assert.AreEqual(6000m, expected[0].Sum);
            Assert.AreEqual(Currency.Dollar, expected[0].Currency);


            expected = Helper.ParseSumAndCurrency("���� 6000$");
            Assert.AreEqual(1, expected.Count);
            Assert.AreEqual(6000m, expected[0].Sum);
            Assert.AreEqual(Currency.Dollar, expected[0].Currency);

            expected = Helper.ParseSumAndCurrency("���� 6000 $");
            Assert.AreEqual(1, expected.Count);
            Assert.AreEqual(6000m, expected[0].Sum);
            Assert.AreEqual(Currency.Dollar, expected[0].Currency);

            expected = Helper.ParseSumAndCurrency("���� 6 000$");
            Assert.AreEqual(1, expected.Count);
            Assert.AreEqual(6000m, expected[0].Sum);
            Assert.AreEqual(Currency.Dollar, expected[0].Currency);

            expected = Helper.ParseSumAndCurrency("���� 6 000 $");
            Assert.AreEqual(1, expected.Count);
            Assert.AreEqual(6000m, expected[0].Sum);
            Assert.AreEqual(Currency.Dollar, expected[0].Currency);


            expected = Helper.ParseSumAndCurrency("���� 6000$ ����");
            Assert.AreEqual(1, expected.Count);
            Assert.AreEqual(6000m, expected[0].Sum);
            Assert.AreEqual(Currency.Dollar, expected[0].Currency);

            expected = Helper.ParseSumAndCurrency("���� 6000 $ ����");
            Assert.AreEqual(1, expected.Count);
            Assert.AreEqual(6000m, expected[0].Sum);
            Assert.AreEqual(Currency.Dollar, expected[0].Currency);

            expected = Helper.ParseSumAndCurrency("���� 6 000$ ����");
            Assert.AreEqual(1, expected.Count);
            Assert.AreEqual(6000m, expected[0].Sum);
            Assert.AreEqual(Currency.Dollar, expected[0].Currency);

            expected = Helper.ParseSumAndCurrency("���� 6 000 $ ����");
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


            expected = Helper.ParseSumAndCurrency("$6000 ����");
            Assert.AreEqual(1, expected.Count);
            Assert.AreEqual(6000m, expected[0].Sum);
            Assert.AreEqual(Currency.Dollar, expected[0].Currency);

            expected = Helper.ParseSumAndCurrency("$ 6000 ����");
            Assert.AreEqual(1, expected.Count);
            Assert.AreEqual(6000m, expected[0].Sum);
            Assert.AreEqual(Currency.Dollar, expected[0].Currency);

            expected = Helper.ParseSumAndCurrency("$ 6 000 ����");
            Assert.AreEqual(1, expected.Count);
            Assert.AreEqual(6000m, expected[0].Sum);
            Assert.AreEqual(Currency.Dollar, expected[0].Currency);

            expected = Helper.ParseSumAndCurrency("$ 6 000 ����");
            Assert.AreEqual(1, expected.Count);
            Assert.AreEqual(6000m, expected[0].Sum);
            Assert.AreEqual(Currency.Dollar, expected[0].Currency);


            expected = Helper.ParseSumAndCurrency("���� $6000");
            Assert.AreEqual(1, expected.Count);
            Assert.AreEqual(6000m, expected[0].Sum);
            Assert.AreEqual(Currency.Dollar, expected[0].Currency);

            expected = Helper.ParseSumAndCurrency("���� $ 6000");
            Assert.AreEqual(1, expected.Count);
            Assert.AreEqual(6000m, expected[0].Sum);
            Assert.AreEqual(Currency.Dollar, expected[0].Currency);

            expected = Helper.ParseSumAndCurrency("���� $ 6 000");
            Assert.AreEqual(1, expected.Count);
            Assert.AreEqual(6000m, expected[0].Sum);
            Assert.AreEqual(Currency.Dollar, expected[0].Currency);

            expected = Helper.ParseSumAndCurrency("���� $ 6 000");
            Assert.AreEqual(1, expected.Count);
            Assert.AreEqual(6000m, expected[0].Sum);
            Assert.AreEqual(Currency.Dollar, expected[0].Currency);


            expected = Helper.ParseSumAndCurrency("���� $6000 ����");
            Assert.AreEqual(1, expected.Count);
            Assert.AreEqual(6000m, expected[0].Sum);
            Assert.AreEqual(Currency.Dollar, expected[0].Currency);

            expected = Helper.ParseSumAndCurrency("���� $ 6000 ����");
            Assert.AreEqual(1, expected.Count);
            Assert.AreEqual(6000m, expected[0].Sum);
            Assert.AreEqual(Currency.Dollar, expected[0].Currency);

            expected = Helper.ParseSumAndCurrency("���� $ 6 000 ����");
            Assert.AreEqual(1, expected.Count);
            Assert.AreEqual(6000m, expected[0].Sum);
            Assert.AreEqual(Currency.Dollar, expected[0].Currency);

            expected = Helper.ParseSumAndCurrency("���� $ 6 000 ����");
            Assert.AreEqual(1, expected.Count);
            Assert.AreEqual(6000m, expected[0].Sum);
            Assert.AreEqual(Currency.Dollar, expected[0].Currency);
        }
    }
}