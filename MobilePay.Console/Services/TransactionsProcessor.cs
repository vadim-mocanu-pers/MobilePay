using MobilePay.Console.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MobilePay.Console.Services
{
    public class TransactionsProcessor
    {
        private string TransactionsFile
        {
            get
            {
                return ConfigReader.GetAppSetting("TransactionsFile");
            }
        }

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

        public Dictionary<string, int> MerchantsDiscounts { get; set; } = new Dictionary<string, int>();

        public void Execute()
        {
            using (StreamReader file = new StreamReader(TransactionsFile))
            {
                string transaction;

                while ((transaction = file.ReadLine()) != null)
                {
                    try
                    {
                        var transactionItems = StringHelper.Split(transaction);
                        var transactionDate = transactionItems.ElementAtOrDefault(TransactionDateIndex);
                        var transactionMerchant = transactionItems.ElementAtOrDefault(TransactionMerchantIndex);
                        var transactionAmount = transactionItems.ElementAtOrDefault(TransactionAmountIndex);
                        var amount = StringHelper.ParseToDouble(transactionAmount);
                        var transactionFee = GetTransactionFee(transactionMerchant, amount);
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

        private double GetTransactionFee(string transactionMerchant, double transactionAmount)
        {
            throw new NotImplementedException();
        }
    }
}
