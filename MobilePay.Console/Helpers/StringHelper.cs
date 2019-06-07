namespace MobilePay.Console.Helpers
{
    public class StringHelper
    {
        public static string[] Split(string initialString)
        {
            return initialString.Split(' ');
        }

        public static double ParseToDouble(string transactionAmount)
        {
            double amount = 0;
            if (!string.IsNullOrWhiteSpace(transactionAmount))
            {
                double.TryParse(transactionAmount, out amount);
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
    }
}
