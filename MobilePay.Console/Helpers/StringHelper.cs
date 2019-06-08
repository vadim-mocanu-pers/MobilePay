
using System;

namespace MobilePay.Console.Helpers
{
    public class StringHelper
    {
        private static string[] stringSeparators = new string[] { " " };

        public static string[] Split(string initialString)
        {
            return initialString.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
        }

        public static decimal ParseToDecimal(string transactionAmount)
        {
            decimal amount = 0;
            if (!string.IsNullOrWhiteSpace(transactionAmount))
            {
                decimal.TryParse(transactionAmount, out amount);
            }

            return amount;
        }

        public static int ParseToInt(string transactionAmount)
        {
            int amount = 0;
            if (!string.IsNullOrWhiteSpace(transactionAmount))
            {
                int.TryParse(transactionAmount, out amount);
            }

            return amount;
        }

        public static DateTime? ParseToDateTime(string transactionDate)
        {
            DateTime dateTime;
            if (!string.IsNullOrWhiteSpace(transactionDate))
            {
                if(DateTime.TryParse(transactionDate, out dateTime))
                {
                    return dateTime;
                }
            }

            return null;
        }
    }
}
