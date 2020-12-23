using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;

namespace TransGps.Droid
{
    static class PermissionsHandler
    {
        private static TaskCompletionSource<bool> requestCompletion = null;

        private static readonly string[] RequiredPermissions = new string []
        {
            Android.Manifest.Permission.AccessFineLocation,
            Android.Manifest.Permission.AccessCoarseLocation
        };

        public static Task PermissionRequestTask => requestCompletion?.Task ?? Task.CompletedTask;

        public static bool NeedsPermissionRequest(Context context)
        {
            return RequiredPermissions.Any(p => IsPermissionInManifest(context, p) && !IsPermissionGranted(context, p));
        }

        public static Task<bool> RequestPermissionAsync(Activity activity)
        {
            if (requestCompletion != null && requestCompletion.Task.IsCompleted)
                return requestCompletion.Task;

            var permissionToRequest = RequiredPermissions.Where(p => IsPermissionInManifest(activity, p) && !IsPermissionGranted(activity, p)).ToArray();

            if (permissionToRequest.Length > 0)
            {
                DoRequestPermissions(activity, permissionToRequest, 101);
                requestCompletion = new TaskCompletionSource<bool>();
                return requestCompletion.Task;
            }

            return Task.FromResult<bool>(true);
        }

        public static void OnRequestPermissionResult(int requestCode, string[] permissions, Permission[] grantResult)
        {
            if (requestCompletion != null && !requestCompletion.Task.IsCompleted)
            {
                var success = true;

                foreach (var gr in grantResult)
                {
                    if (gr == Permission.Denied)
                    {
                        success = false;
                        break;
                    }
                }

                requestCompletion.TrySetResult(success);
            }
        }

        private static bool IsPermissionInManifest(Context context, string permission)
        {
            try
            {
                var info = context.PackageManager.GetPackageInfo(context.PackageName, Android.Content.PM.PackageInfoFlags.Permissions);
                return info.RequestedPermissions.Contains(permission);
            }
            catch
            {
                return false;
            }
        }

        private static bool IsPermissionGranted(Context context, string permission)
        {
            return ContextCompat.CheckSelfPermission(context, permission) == Android.Content.PM.Permission.Granted;
        }

        private static bool DoRequestPermissions(Activity activity, string[] permissions, int requestCode)
        {
            var permissionsToRequest = new List<string>();

            foreach (var permission in permissions)
            {
                if (ContextCompat.CheckSelfPermission(activity, permission) != Android.Content.PM.Permission.Granted)
                {
                    permissionsToRequest.Add(permission);
                }
            }

            if (permissionsToRequest.Count > 0)
            {
                ActivityCompat.RequestPermissions(activity, permissionsToRequest.ToArray(), requestCode);
                return true;
            }

            return false;
        }
    }
}