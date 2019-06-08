using MobilePay.Console.Services;
using System;

namespace MobilePay.Console
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                System.Console.WriteLine("Output:");

                var discountsProcessor = new DiscountsProcessor();
                discountsProcessor.Execute();

                var transactionsProcessor = new TransactionsProcessor();
                transactionsProcessor.MerchantsDiscounts = discountsProcessor.MerchantsDiscounts;
                transactionsProcessor.Execute();

                System.Console.ReadKey();
            }
            catch (Exception ex)
            {
                // Add to log file or in database log table
            }
        }
    }
}
