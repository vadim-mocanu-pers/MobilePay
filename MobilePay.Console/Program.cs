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
                System.Console.WriteLine("Press Y to start");
                System.Console.ReadKey();

                var transactionsFileName = "";
                if (System.IO.File.Exists(transactionsFileName))
                {
                    var discountsProcessor = new DiscountsProcessor();
                    discountsProcessor.Execute();

                    var transactionsProcessor = new TransactionsProcessor();
                    transactionsProcessor.MerchantsDiscounts = discountsProcessor.MerchantsDiscounts;
                    transactionsProcessor.Execute();
                }
            }
            catch (Exception ex)
            {
                // Add to log file or in database log table
            }
        }
    }
}
