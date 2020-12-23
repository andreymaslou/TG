using Android.Content;
using TransGps.Controls;
using TransGps.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(MapsUIView), typeof(MapViewRenderer))]
namespace TransGps.Droid
{
    public class MapViewRenderer : ViewRenderer<MapsUIView, Mapsui.UI.Android.MapControl>
    {
        Mapsui.UI.Android.MapControl mapNativeControl;
        MapsUIView mapViewControl;

        public MapViewRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<MapsUIView> e)
        {
            base.OnElementChanged(e);

            if (mapViewControl == null && e.NewElement != null)
                mapViewControl = e.NewElement;

            if (mapNativeControl == null && mapViewControl != null)
            {
                mapNativeControl = new Mapsui.UI.Android.MapControl(Context, null);
                mapNativeControl.Map = mapViewControl.NativeMap;

                SetNativeControl(mapNativeControl);
            }
        }
    }
}