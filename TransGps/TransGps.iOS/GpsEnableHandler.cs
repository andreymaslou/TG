using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

using Xamarin.Forms;
using TransGps.Interfaces;
using System.Threading.Tasks;

[assembly:Dependency(typeof(TransGps.iOS.GpsEnableHandler))]
namespace TransGps.iOS
{
    class GpsEnableHandler : IGpsEnable
    {
        public async Task<bool> EnableGpsAsync()
        {
            await Task.Run(() => { });

            return false;
        }
    }
}