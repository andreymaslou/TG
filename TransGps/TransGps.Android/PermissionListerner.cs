using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using TransGps.Interfaces;
using Xamarin.Forms;

[assembly:Dependency(typeof(TransGps.Droid.PermissionListerner))]
namespace TransGps.Droid
{
    class PermissionListerner : IPermissionListener
    {
        private CancellationTokenSource cancellationTokenSource;
        public async Task AwaitPermissionsRequest()
        {
            var activity = MainActivity.Context as MainActivity;

            if (activity == null)
                return;

            activity.PermissionsRequested += Activity_PermissionsRequested;

            cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;

            await Task.Run(() =>
            {
                while (true)
                {
                    Thread.Sleep(1000);

                    if (cancellationToken.IsCancellationRequested)
                        break;
                }

            }, cancellationToken);

            activity.PermissionsRequested -= Activity_PermissionsRequested;
        }

        private void Activity_PermissionsRequested(object sender, EventArgs e)
        {
            cancellationTokenSource.Cancel();
        }
    }
}