using Xamarin.Forms;
using TransGps.Interfaces;
using System.Globalization;

[assembly:Dependency(typeof(TransGps.Droid.Localize))]
namespace TransGps.Droid
{
    [Android.Runtime.Preserve(AllMembers = true)]
    class Localize : ILocalize
    {
        public CultureInfo GetCurrentCiltureInfo()
        {
            var localization = Java.Util.Locale.Default;
            var prefLanguage = "uk-UA";

            var netLangauge = localization.ToString().Replace("_", "-").Substring(0, 2);

            var cultureName = string.Empty;

            if (netLangauge == "ru")
                cultureName = "ru-RU";
            else if (netLangauge == "en")
                cultureName = "en-US";
            else
                cultureName = prefLanguage;

            CultureInfo ci = null;

            try
            {
                ci = new CultureInfo(cultureName);
            }
            catch (System.Exception ex)
            {
                ci = new CultureInfo(prefLanguage);
                System.Diagnostics.Debug.WriteLine($"Error: Android - Can't create CultureInfo. ErrorMessage - {ex.Message}");
            }

            return ci;
        }
    }
}