using MobilePay.Console.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace MobilePay.Console.Services
{
    public class DiscountsProcessor
    {
        private string MerchantsDiscountsFile
        {
            get
            {
                return ConfigReader.GetAppSetting("MerchantsDiscountsFile");
            }
        }

        private int MerchantDiscountIndex
        {
            get
            {
                return ConfigReader.GetIntAppSetting("MerchantDiscountIndex");
            }
        }

        private int DiscountValueIndex
        {
            get
            {
                return ConfigReader.GetIntAppSetting("DiscountValueIndex");
            }
        }

        public Dictionary<string, int> MerchantsDiscounts { get; } = new Dictionary<string, int>();

        public void Execute()
        {
            try
            {
                if (File.Exists(MerchantsDiscountsFile))
                {
                    string[] discountLines = File.ReadAllLines(MerchantsDiscountsFile);
                    foreach (string line in discountLines)
                    {
                        var discountItems = StringHelper.Split(line);
                        var merchant = discountItems.ElementAtOrDefault(MerchantDiscountIndex);
                        var discountValue = discountItems.ElementAtOrDefault(DiscountValueIndex);
                        var discount = StringHelper.ParseToInt(discountValue);

                        MerchantsDiscounts.Add(merchant, discount);
                    }
                }
            }
            catch (Exception ex)
            {
                // Add to log file or in database log table
            }
        }
    }
}
