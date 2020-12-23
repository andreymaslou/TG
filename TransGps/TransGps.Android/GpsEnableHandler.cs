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
using Android.Gms.Common.Apis;
using Android.Gms.Location;
using System.Threading.Tasks;
using System.Threading;

[assembly:Dependency(typeof(TransGps.Droid.GpsEnableHandler))]
namespace TransGps.Droid
{
    class GpsEnableHandler : IGpsEnable
    {
        public async Task<bool> EnableGpsAsync()
        {
            Android.Locations.LocationManager manager = (Android.Locations.LocationManager)Android.App.Application.Context.GetSystemService(Context.LocationService);

            if (!manager.IsProviderEnabled(Android.Locations.LocationManager.GpsProvider))
            {
                await ShowMessageNoGps();

                cancellationTokenSource = new CancellationTokenSource();
                var cancellationToken = cancellationTokenSource.Token;
                await GetConfirmation(cancellationToken);
            }

            return manager.IsProviderEnabled(Android.Locations.LocationManager.GpsProvider);
        }

        private async Task ShowMessageNoGps()
        {
            try
            {
                var activity = MainActivity.Context as MainActivity;
                var googleApiClient = new GoogleApiClient.Builder(activity)
                                                         .AddApi(LocationServices.API).Build();

                googleApiClient.Connect();
                var locationRequest = LocationRequest.Create();
                locationRequest.SetPriority(LocationRequest.PriorityHighAccuracy);
                locationRequest.SetInterval(10000);
                locationRequest.SetFastestInterval(10000 / 2);

                var locationSettingsRequestBuilder = new LocationSettingsRequest.Builder().AddLocationRequest(locationRequest);
                locationSettingsRequestBuilder.SetAlwaysShow(false);

                var locationSettingsResult = await LocationServices.SettingsApi.CheckLocationSettingsAsync(googleApiClient, locationSettingsRequestBuilder.Build());

                if (locationSettingsResult.Status.StatusCode == LocationSettingsStatusCodes.ResolutionRequired)
                {
                    activity.LocationEnablingComfirmed += Activity_LocationEnablingComfirmed;

                    locationSettingsResult.Status.StartResolutionForResult(activity, MainActivity.REQUEST_LOCATION);
                }



            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error through turn on GPS - {ex.Message}");

                if (ex.InnerException != null)
                    System.Console.WriteLine($"Inner exception - {ex.InnerException.Message}");
            }
        }

        private CancellationTokenSource cancellationTokenSource;
        private async Task GetConfirmation(CancellationToken cancellationToken)
        {
            var activity = MainActivity.Context as MainActivity;

            await Task.Run(() =>
            {
                while (true)
                {
                    Console.WriteLine($"Token cancellation requested - {cancellationToken.IsCancellationRequested}");

                    Thread.Sleep(1000);

                    if (cancellationToken.IsCancellationRequested)
                        break;

                    Console.WriteLine("Waitnig for confirmation");
                }

            }, cancellationToken);

            Console.WriteLine("Confirmation is recieved");
            activity.LocationEnablingComfirmed -= Activity_LocationEnablingComfirmed;

        }

        private void Activity_LocationEnablingComfirmed(object sender, EventArgs e)
        {
            Console.WriteLine("Confirmation event is fired");
            cancellationTokenSource.Cancel();
        }
    }
}