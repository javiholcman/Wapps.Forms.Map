using System;
using Xamarin.Forms;
using Xamarin.Forms.Maps.Android;
using Android.Gms.Maps;
using System.Collections.Generic;
using System.ComponentModel;
using Android.Gms.Maps.Model;
using Android.Content;
using Android.Widget;
using Xamarin.Forms.Maps;
using Android.Graphics;
using Android.Graphics.Drawables;
using System.Linq;
using System.Threading.Tasks;
using Wapps.Forms;
using Wapps.Forms.Droid;

[assembly:ExportRenderer (typeof(WMap), typeof(WMapRenderer))]
namespace Wapps.Forms.Droid
{
	public class WMapRenderer : MapRenderer, GoogleMap.IInfoWindowAdapter, IOnMapReadyCallback
	{		
		bool isDrawn;
		bool _firstLoad = true;

		GoogleMap MapView { get; set; }
		WMap MapControl { get; set; }
		WPin CurrentPinWindow { get; set; }

		protected override void OnElementChanged (Xamarin.Forms.Platform.Android.ElementChangedEventArgs<View> e)
		{
			base.OnElementChanged (e);
		
			if (e.NewElement != null) {

				this.MapControl = (WMap)e.NewElement;
				if (_firstLoad) {
					this.MapControl.RendererNeedToRefreshWindow += Map_RendererNeedToRefreshWindow;
				}

				((MapView)Control).GetMapAsync (this);
			}
		}
			
		protected override void OnElementPropertyChanged (object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged (sender, e);

			if (e.PropertyName.Equals ("VisibleRegion") && !isDrawn) {
				MapView.Clear ();

				foreach (var pin in this.MapControl.ExtendedPins) {
					var marker = new MarkerOptions ();
					marker.SetPosition (new LatLng (pin.Pin.Position.Latitude, pin.Pin.Position.Longitude));
					marker.SetTitle (pin.Pin.Label);
					marker.SetSnippet (pin.Pin.Address);

					if (MapControl.GetPinViewDelegate != null) {
						var formsView = MapControl.GetPinViewDelegate (pin);
						//var nativeView = Utils.ConvertFormsToNative (formsView, new Rectangle (0, 0, (double)Utils.DpToPx ((float)formsView.WidthRequest), (double)Utils.DpToPx ((float)formsView.HeightRequest)));
						var nativeView = WMapAuxiliarRenderer.LiveMapRenderer.GetNativeView(formsView);
						Utils.FixImageSourceOfImageViews (nativeView as Android.Views.ViewGroup);

						var otherView = new FrameLayout (this.Context);
						nativeView.LayoutParameters = new FrameLayout.LayoutParams (Utils.DpToPx ((float)formsView.WidthRequest), Utils.DpToPx ((float)formsView.HeightRequest));
						otherView.AddView (nativeView);

						var bitmap = Utils.ConvertViewToBitmap (otherView);
						marker.SetIcon (BitmapDescriptorFactory.FromBitmap (bitmap));
					}

					MapView.AddMarker (marker);
				}
				isDrawn = true;
			}
		}

		protected void Map_RendererNeedToRefreshWindow (object sender, WPin e)
		{
			Task.Delay(500).ContinueWith(delegate(Task arg) {
				RefreshWindow(e);
			});
		}

		void RefreshWindow (WPin pin)
		{
			Device.BeginInvokeOnMainThread(delegate ()
			{
				try
				{
					var marker = pin.Atts["Marker"] as Marker;
					if (CurrentPinWindow == pin)
					{
						marker.HideInfoWindow();
						marker.ShowInfoWindow();
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
				}
			});
		}

		public WPin GetPin (Marker point) 
		{
			var position = new Position (point.Position.Latitude, point.Position.Longitude);
			foreach (var pin in this.MapControl.ExtendedPins) {
				if (pin.Pin.Position == position) {
					return pin;
				}
			}
			return null;
		}

		#region Native Map

		public void OnMapReady (GoogleMap googleMap)
		{
			MapView = googleMap;
			MapView.InfoWindowClick += OnInfoWindowClick;
			MapView.SetInfoWindowAdapter (this);
			this.MapView.InfoWindowClose += OnInfoWindowClose;
		}
			
		public Android.Views.View GetInfoContents (Marker marker)
		{
			var pin = GetPin (marker);
			if (pin == null) {
				throw new Exception ("Custom pin not found");
			}

			if (pin.Atts == null)
				pin.Atts = new Dictionary<string, object> ();

			pin.Atts ["Marker"] = marker;

			if (this.MapControl.GetPinWindowDelegate == null) {
				return new Android.Views.View (this.Context);
			}

			Android.Views.View view;
			object nativeView;

			WeakReference<object> windowInfo = null;
			if (pin.Atts.ContainsKey("windowInfo")) {
				windowInfo = pin.Atts["windowInfo"] as WeakReference<object>;
			}
			if (windowInfo == null || !windowInfo.TryGetTarget (out nativeView)) {
				var formsView = this.MapControl.GetPinWindowDelegate (pin);
				view = WMapAuxiliarRenderer.LiveMapRenderer.GetNativeView (formsView);
				windowInfo = new WeakReference <object> (view);
				pin.Atts["windowInfo"] = windowInfo;
			} else {
				view = (Android.Views.View)nativeView;
			}

			CurrentPinWindow = pin;

			return view;
		}

		void OnInfoWindowClose (object sender, GoogleMap.InfoWindowCloseEventArgs e)
		{
			CurrentPinWindow = null;
		}

		void OnInfoWindowClick (object sender, GoogleMap.InfoWindowClickEventArgs e)
		{
			var pin = GetPin (e.Marker);
			if (pin == null) {
				throw new Exception ("Custom pin not found");
			}
				
			MapControl.FireWindowClicked (pin);
		}

		public Android.Views.View GetInfoWindow (Marker marker)
		{
			return null;
		}

		#endregion

	}
}

