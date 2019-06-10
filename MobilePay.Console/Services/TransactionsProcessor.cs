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
            if (!ValidateFileExist())
            {
                return;
            }

            var fileReader = GetFileReader();
            string transaction;
            while ((transaction = GetTransaction(fileReader)) != null)
            {
                try
                {
                    var output = BuildOutput(transaction);
                    System.Console.WriteLine(output);
                }
                catch (Exception ex)
                {
                    // Add to log file or in database log table
                }
            }

            DestroyFileReader(fileReader);
        }

        protected internal virtual string BuildOutput(string transaction)
        {
            var transactionItems = StringHelper.Split(transaction);
            var transactionDate = transactionItems.ElementAtOrDefault(TransactionDateIndex);
            var transactionMerchant = transactionItems.ElementAtOrDefault(TransactionMerchantIndex);
            var transactionAmount = transactionItems.ElementAtOrDefault(TransactionAmountIndex);
            var amount = StringHelper.ParseToDecimal(transactionAmount);
            var transactionFee = GetTransactionFee(transactionMerchant, amount);
            transactionFee += GetInvoiceFee(transactionMerchant, transactionDate);

            var output = $"{transactionDate} {transactionMerchant.PadRight(9)} {transactionFee:0.00}";
            return output;
        }

        protected internal virtual bool ValidateFileExist()
        {
            if (!File.Exists(TransactionsFile))
            {
                System.Console.WriteLine("Couldn't find the transaction.txt file.");
                return false;
            }

            return true;
        }

        protected internal virtual StreamReader GetFileReader()
        {
            return new StreamReader(TransactionsFile);
        }
        
        protected internal virtual string GetTransaction(StreamReader fileReader)
        {
            return fileReader.ReadLine();
        }

        protected internal virtual void DestroyFileReader(StreamReader fileReader)
        {
            fileReader.Close();
            fileReader.Dispose();
        }

        protected internal virtual decimal GetTransactionFee(string transactionMerchant, decimal transactionAmount)
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
