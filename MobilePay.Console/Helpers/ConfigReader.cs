using System.Configuration;

namespace MobilePay.Console.Helpers
{
    public class ConfigReader
    {
        public static int GetIntAppSetting(string key)
        {
            var value = GetAppSetting(key);

            var intValue = 1;
            int.TryParse(value, out intValue);

            return intValue;
        }

        public static string GetAppSetting(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}
