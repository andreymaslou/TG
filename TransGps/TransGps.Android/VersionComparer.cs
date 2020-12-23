using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Xamarin.Forms;
using TransGps.Interfaces;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.RegularExpressions;

[assembly: Dependency(typeof(TransGps.Droid.VersionComparer))]
namespace TransGps.Droid
{
    class VersionComparer : IVersionComparer
    {
        public Task<bool> IsUsingLatestVersion()
        {
            var task = Task.Run(() =>
            {
                var context = Android.App.Application.Context;
                var currentVersion = context.PackageManager.GetPackageInfo(context.PackageName, 0).VersionName;

                using (var webClient = new System.Net.WebClient())
                {
                    try
                    {
                        var webUri = "https://play.google.com/store/apps/details?id=com.companyname.TransGps&hl=en";
                        var stringResult = webClient.DownloadString(webUri);

                        if (!String.IsNullOrEmpty(stringResult))
                        {
                            var pattern = @"<div[^>]*>Current Version</div><span[^>]*><div[^>]*><span[^>]*>(.*?)<";
                            var matchResult = Regex.Match(stringResult, pattern).Groups[1];
                            if (matchResult.Success)
                            {
                                var version = matchResult.Value.Trim();

                                if (Version.TryParse(version, out Version latestVersion) && Version.TryParse(currentVersion, out Version installedVersion))
                                {
                                    return latestVersion.CompareTo(installedVersion) <= 0; 
                                }
                            }
                        }

                        return true;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error while load info from GoolglePlay. {ex.Message}");
                        return true;
                    }
                }
            });

            return task;
        }

        public Task OpenAppInStore()
        {
            try
            {
                var intent = new Intent(Intent.ActionView, global::Android.Net.Uri.Parse($"market:\\details?id=com.companyname.TransGps"));
                intent.SetPackage("com.android.vending");
                intent.SetFlags(ActivityFlags.NewTask);
                global::Android.App.Application.Context.StartActivity(intent);
            }
            catch (Exception)
            {
                var intent = new Intent(Intent.ActionView, global::Android.Net.Uri.Parse($"https://play.google.com/store/apps/details?id=com.companyname.TransGps"));
                global::Android.App.Application.Context.StartActivity(intent);
            }

            return Task.FromResult(true);
        }
    }
}