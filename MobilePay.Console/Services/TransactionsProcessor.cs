using MobilePay.Console.Helpers;
using System;
using System.IO;
using System.Linq;

namespace MobilePay.Console.Services
{
    public class TransactionsProcessor
    {
        private int TransactionAmountIndex
        {
            get
            {
                return ConfigReader.GetIntAppSetting("TransactionElementIndex");
            }
        }

        private int TransactionMerchantIndex
        {
            get
            {
                return ConfigReader.GetIntAppSetting("TransactionMerchantIndex");
            }
        }

        private int TransactionDateIndex
        {
            get
            {
                return ConfigReader.GetIntAppSetting("TransactionDateIndex");
            }
        }
        
        public void Execute(string fileName)
        {
            using (StreamReader file = new StreamReader(fileName))
            {
                string transaction;

                while ((transaction = file.ReadLine()) != null)
                {
                    try
                    {
                        var stringHelper = new StringHelper();
                        var transactionItems = stringHelper.Splitter(transaction);
                        var transactionDate = transactionItems.ElementAtOrDefault(TransactionDateIndex);
                        var transactionMerchant = transactionItems.ElementAtOrDefault(TransactionMerchantIndex);
                        var transactionAmount = transactionItems.ElementAtOrDefault(TransactionAmountIndex);
                        var amount = ProcessTransactionAmount(transactionAmount);
                        var transactionFee = GetTransactionFee(amount);
                        System.Console.WriteLine($"{transactionDate} {transactionMerchant} {transactionFee}");
                    }
                    catch (Exception ex)
                    {
                        // Add to log file or in database log table
                    }
                }
                file.Close();
            }
        }

        private double ProcessTransactionAmount(string transactionAmount)
        {
            double amount = 0;
            if (!string.IsNullOrWhiteSpace(transactionAmount))
            {
                double.TryParse(transactionAmount, out amount);
            }

            return amount;
        }

        private double GetTransactionFee(double transactionAmount)
        {

        }
    }
}
