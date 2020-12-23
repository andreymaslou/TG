using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

using Xamarin.Forms;
using TransGps.Interfaces;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

[assembly:Dependency(typeof(TransGps.iOS.VersionComparer))]
namespace TransGps.iOS
{
    class VersionComparer : IVersionComparer
    {
        public Task<bool> IsUsingLatestVersion()
        {
            var task = Task.Run(() =>
            {
                try
                {
                    var bundleIdentifier = NSBundle.MainBundle.ObjectForInfoDictionary("CFBundleIdentifier").ToString();
                    var bundleVersion = NSBundle.MainBundle.ObjectForInfoDictionary("CFBundleShortVersionString").ToString();
                    
                    var uri = new NSUrl($@"http://itunes.apple.com/lookup?bundleId={bundleIdentifier}");
                    var data = NSData.FromUrl(uri);
                    var lookup = NSJsonSerialization.Deserialize(data, 0, out NSError error) as NSDictionary;

                    if (lookup != null && lookup[new NSString("results")].ToString() == "1")
                    {
                        var appStoreArray = lookup[new NSString("results")] as NSArray;
                        var appStoreVersion = appStoreArray.GetItem<NSMutableDictionary>(0)[new NSString("version")].ToString();

                        if (Version.TryParse(bundleVersion, out Version currentVersion) && Version.TryParse(appStoreVersion, out Version latestVersion))
                        {
                            return latestVersion.CompareTo(currentVersion) <= 0;
                        }
                    }

                    return true;
                }
                catch (Exception)
                {
                    return true;
                }
            });

            return task;
        }

        public Task OpenAppInStore()
        {
            try
            {
                var bundleName = NSBundle.MainBundle.ObjectForInfoDictionary("CFBundleName").ToString();
                var appName = GetSafeForAppStoreShortLinkUrl(bundleName);

                UIApplication.SharedApplication.OpenUrl(new NSUrl($"http://appstore.com/{appName}"));

            }
            catch (Exception ex)
            {

                System.Diagnostics.Debug.WriteLine($"Open app in AppStore error: {ex.Message}");
            }
            return Task.FromResult(true);
        }

        private string GetSafeForAppStoreShortLinkUrl(string appName)
        {
            var regex = new Regex(@"[©™®!¡""#$%'()*+,\\\-.\/:;<=>¿?@[\]^_`{|}~]*");
            return regex.Replace(appName, "")
                        .Replace(" ", "")
                        .Replace("&", "and")
                        .ToLower();
        }
    }
}