using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation;
using UIKit;

using Xamarin.Forms;
using TransGps.Interfaces;

[assembly:Dependency(typeof(TransGps.iOS.PermissionListener))]
namespace TransGps.iOS
{
    class PermissionListener : IPermissionListener
    {
        public Task AwaitPermissionsRequest()
        {
            throw new NotImplementedException();
        }
    }
}