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
        
        private decimal TransactionPercentageFee
        {
            get
            {
                return ConfigReader.GetIntAppSetting("TransactionPercentageFee");
            }
        }    
        
        private int InvoiceFixedFee
        {
            get
            {
                return ConfigReader.GetIntAppSetting("InvoiceFixedFee");
            }
        }

        public Dictionary<string, int> MerchantsDiscounts { get; set; } = new Dictionary<string, int>();
        public List<string> MerchantsInvoiceFee { get; set; } = new List<string>();

        public void Execute()
        {
            if (!File.Exists(TransactionsFile))
            {
                System.Console.WriteLine("Couldn't find the transaction.txt file.");
                return;
            }

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
                        var amount = StringHelper.ParseToDecimal(transactionAmount);
                        var transactionFee = GetTransactionFee(transactionMerchant, amount);
                        transactionFee += GetInvoiceFee(transactionMerchant, transactionDate);
                        System.Console.WriteLine($"{transactionDate} {transactionMerchant.PadRight(9)} {transactionFee:0.00}");
                    }
                    catch (Exception ex)
                    {
                        // Add to log file or in database log table
                    }
                }
                file.Close();
            }
        }

        private decimal GetTransactionFee(string transactionMerchant, decimal transactionAmount)
        {
            var transactionFee = Math.Round((TransactionPercentageFee / 100) * transactionAmount, 2, MidpointRounding.AwayFromZero);
            var percentageFeeDiscount = GetPercentageFeeDiscount(transactionMerchant);
            if (percentageFeeDiscount > 0)
            {
                var feeDiscount = Math.Round((percentageFeeDiscount / 100) * transactionFee, 2, MidpointRounding.AwayFromZero);
                transactionFee = transactionFee - feeDiscount;
            }

            return transactionFee;
        }

        private decimal GetPercentageFeeDiscount(string transactionMerchant)
        {
            var percentageFeeDiscount = 0;
            MerchantsDiscounts.TryGetValue(transactionMerchant, out percentageFeeDiscount);
            
            return percentageFeeDiscount;
        }

        private int GetInvoiceFee(string transactionMerchant, string transactionDate)
        {
            try
            {
                var transactionMonth = GetTransactionMonth(transactionDate);
                if (transactionMonth > 0)
                {
                    var key = transactionMerchant + transactionMonth;
                    if (!MerchantsInvoiceFee.Contains(key))
                    {
                        MerchantsInvoiceFee.Add(key);
                        return InvoiceFixedFee;
                    }
                }
            }
            catch (Exception ex)
            {
                // Add to log file or in database log table
            }

            return 0;
        }

        private int GetTransactionMonth(string transactionDate)
        {
            try
            {
                var dateTime = StringHelper.ParseToDateTime(transactionDate);
                if (dateTime.HasValue)
                {
                    return dateTime.Value.Month;
                }
            }
            catch (Exception ex)
            {
                // Add to log file or in database log table
            }

            return 0;
        }
    }
}
