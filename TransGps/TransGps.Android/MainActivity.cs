using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content;
using Xamarin.Forms;

namespace TransGps.Droid
{
    [Activity(Label = "TransGps", Icon = "@mipmap/icon", Theme = "@style/splashscreen", MainLauncher = true, ConfigurationChanges = ConfigChanges.Touchscreen | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public static Context Context;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.Window.RequestFeature(WindowFeatures.ActionBar);
            base.SetTheme(Resource.Style.MainTheme);

            base.OnCreate(savedInstanceState);

            Context = this;

            Forms.SetFlags("CarouselView_Experimental");

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }

        protected override void OnResume()
        {
            base.OnResume();

            if (PermissionsHandler.NeedsPermissionRequest(this))
            {
                try
                {
                    PermissionsHandler.RequestPermissionAsync(this);
                }
                catch (Exception)
                {

                }
            }

        }

        public override Android.Content.Res.Resources Resources
        {
            get
            {
                var config = base.Resources.Configuration;
                if (config == null)
                {
                    config = new Android.Content.Res.Configuration();
                }

                config.FontScale = 1f;

                return CreateConfigurationContext(config).Resources;
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            PermissionsHandler.OnRequestPermissionResult(requestCode, permissions, grantResults);

            PermissionsRequested?.Invoke(this, null);
            
        }

        public const int REQUEST_LOCATION = 199;

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == REQUEST_LOCATION)
            {
                LocationEnablingComfirmed?.Invoke(this, null);
            }
        }

        public event EventHandler LocationEnablingComfirmed;
        public event EventHandler PermissionsRequested;
    }
}