using Microsoft.VisualStudio.TestTools.UnitTesting;
using MobilePay.Console.Services;
using Moq;
using System.IO;

namespace MobilePay.UnitTests.Services
{
    [TestClass]
    public class TransactionsProcessorUnitTests
    {
        public static TransactionsProcessor transactionsProcessor = new TransactionsProcessor();

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            transactionsProcessor.MerchantsDiscounts.Add("TELIA", 10);
            transactionsProcessor.MerchantsDiscounts.Add("CIRCLE_K", 20);
        }

        [TestMethod]
        public void Execute_ReturnTransaction_NoException()
        {
            // arrange
            var transaction = "2018-09-01 7-ELEVEN 100";
            var transactionsProcessorMoq = new Mock<TransactionsProcessor>();
            transactionsProcessorMoq.Setup(t => t.ValidateFileExist()).Returns(true);
            transactionsProcessorMoq.Setup(t => t.GetFileReader()).Returns(It.IsAny<StreamReader>());
            transactionsProcessorMoq.SetupSequence(t => t.GetTransaction(It.IsAny<StreamReader>())).Returns(transaction);
            transactionsProcessorMoq.Setup(t => t.BuildOutput(transaction)).Returns(It.IsAny<string>());

            // act
            transactionsProcessorMoq.Object.Execute();

            // assert
            transactionsProcessorMoq.Verify(x=>x.BuildOutput(transaction), Times.Once);
            transactionsProcessorMoq.VerifyAll();
        }

        [DataTestMethod]
        [DataRow("2018-09-01 7-ELEVEN 100", "2018-09-01 7-ELEVEN  30.00")]
        [DataRow("2018-09-04 CIRCLE_K 100", "2018-09-04 CIRCLE_K  29.80")]
        [DataRow("2018-09-07 TELIA    100", "2018-09-07 TELIA     29.90")]
        [DataRow("2018-09-09 NETTO    100", "2018-09-09 NETTO     30.00")]
        [DataRow("2018-09-13 CIRCLE_K 100", "2018-09-13 CIRCLE_K  0.80")]
        [DataRow("2018-09-16 TELIA    100", "2018-09-16 TELIA     0.90")]
        [DataRow("2018-09-19 7-ELEVEN 100", "2018-09-19 7-ELEVEN  1.00")]
        [DataRow("2018-09-22 CIRCLE_K 100", "2018-09-22 CIRCLE_K  0.80")]
        [DataRow("2018-09-25 TELIA    100", "2018-09-25 TELIA     0.90")]
        [DataRow("2018-09-28 7-ELEVEN 100", "2018-09-28 7-ELEVEN  1.00")]
        [DataRow("2018-09-30 CIRCLE_K 100", "2018-09-30 CIRCLE_K  0.80")]
        [DataRow("2018-10-01 7-ELEVEN 100", "2018-10-01 7-ELEVEN  30.00")]
        [DataRow("2018-10-04 CIRCLE_K 100", "2018-10-04 CIRCLE_K  29.80")]
        [DataRow("2018-10-07 TELIA    100", "2018-10-07 TELIA     29.90")]
        [DataRow("2018-10-10 NETTO    100", "2018-10-10 NETTO     30.00")]
        [DataRow("2018-10-13 CIRCLE_K 100", "2018-10-13 CIRCLE_K  0.80")]
        [DataRow("2018-10-16 TELIA    100", "2018-10-16 TELIA     0.90")]
        [DataRow("2018-10-19 7-ELEVEN 100", "2018-10-19 7-ELEVEN  1.00")]
        [DataRow("2018-10-22 CIRCLE_K 100", "2018-10-22 CIRCLE_K  0.80")]
        [DataRow("2018-10-25 TELIA    100", "2018-10-25 TELIA     0.90")]
        [DataRow("2018-10-28 7-ELEVEN 100", "2018-10-28 7-ELEVEN  1.00")]
        [DataRow("2018-10-30 CIRCLE_K 100", "2018-10-30 CIRCLE_K  0.80")]
        public void BuildOutput_Possitive_ExpectedResults(string transaction, string expectedOutput)
        {
            // act
            var output = transactionsProcessor.BuildOutput(transaction);

            // assert
            Assert.AreEqual(expectedOutput, output);
        }
    }
}
