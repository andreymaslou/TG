using Foundation;
using Xamarin.Forms;

using TransGps.Interfaces;
using System.Globalization;

[assembly: Dependency(typeof(TransGps.iOS.Localize))]
namespace TransGps.iOS
{
    class Localize : ILocalize
    {
        public CultureInfo GetCurrentCiltureInfo()
        {
            var netLanguage = "uk";
            var prefLanguage = "uk-UA";

            if (NSLocale.PreferredLanguages.Length > 0)
            {
                var pref = NSLocale.PreferredLanguages[0];
                netLanguage = pref.Replace("_", "-").Substring(0, 2);
            }

            var cultureName = string.Empty;

            if (netLanguage == "ru")
                cultureName = "ru-RU";
            else if (netLanguage == "en")
                cultureName = "en-US";
            else
                cultureName = prefLanguage;

            CultureInfo ci = null;

            try
            {
                ci = new CultureInfo(cultureName);
            }
            catch(System.Exception ex)
            {
                ci = new CultureInfo(prefLanguage);
                System.Diagnostics.Debug.WriteLine($"Error: iOS - Can't create CultureInfo. ErrorMessage - {ex.Message}");
            }

            return ci;
        }
    }
}