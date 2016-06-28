using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using System.Collections.Generic;
using MapKit;
using CoreGraphics;
using Xamarin.Forms.Maps;
using System.Linq;
using Wapps.Forms;
using Xamarin.Forms.Maps.iOS;
using Wapps.Forms.iOS;

[assembly:ExportRenderer (typeof(WMap), typeof(WMapRenderer))]
namespace Wapps.Forms.iOS
{
	public class WMapRenderer : MapRenderer
	{
		WCalloutView CurrentCalloutView;

		MKMapView MapView { get; set; }
		WMap MapControl { get; set; }

		protected override void OnElementChanged (ElementChangedEventArgs<View> e)
		{
			base.OnElementChanged (e);

			if (e.OldElement != null) {
				var nativeMap = Control as MKMapView;
				nativeMap.GetViewForAnnotation = null;
				nativeMap.CalloutAccessoryControlTapped -= OnCalloutAccessoryControlTapped;
				nativeMap.DidSelectAnnotationView -= OnDidSelectAnnotationView;
				nativeMap.DidDeselectAnnotationView -= OnDidDeselectAnnotationView;
			}

			if (e.NewElement != null) {
				var formsMap = (WMap)e.NewElement;
				MapView = Control as MKMapView;
				MapControl = (WMap)e.NewElement;
				MapView.GetViewForAnnotation = GetViewForAnnotation;
				MapView.CalloutAccessoryControlTapped += OnCalloutAccessoryControlTapped;
				MapView.DidSelectAnnotationView += OnDidSelectAnnotationView;
				MapView.DidDeselectAnnotationView += OnDidDeselectAnnotationView;
			}
		}
			
		public MKAnnotationView GetViewForAnnotation (MKMapView mapView, IMKAnnotation annotation)
		{
			MKAnnotationView annotationView = null;

			if (annotation is MKUserLocation)
				return null;

			var pin = GetPin (annotation as MKPointAnnotation);
			if (pin == null) {
				throw new Exception ("Custom pin not found");
			}

			if (annotation is WCalloutAnnotation) {
				var formsView = MapControl.GetPinWindowDelegate (pin);
				var nativeView = Utils.ConvertFormsToNative (formsView, new CGRect (0, 0, formsView.WidthRequest, formsView.HeightRequest));
				nativeView.TranslatesAutoresizingMaskIntoConstraints = false;
				nativeView.Frame = new CGRect (0, 0, formsView.WidthRequest, formsView.HeightRequest);

				CurrentCalloutView = new WCalloutView (annotation);
				CurrentCalloutView.CenterOffset = new CGPoint (0, -1 * (formsView.HeightRequest));
				CurrentCalloutView.Frame = new CGRect (0, 0, formsView.WidthRequest, formsView.HeightRequest + 20);
				CurrentCalloutView.View = nativeView;
				CurrentCalloutView.Touched += delegate(object sender, EventArgs e) {
					MapControl.FireWindowClicked (pin);
				};
				return CurrentCalloutView;
			}

			annotationView = mapView.DequeueReusableAnnotation (pin.Id);
			if (annotationView == null) {

				UIImage image = null;
				if (MapControl.GetPinViewDelegate != null) {
					var formsView = MapControl.GetPinViewDelegate (pin);
					var nativeView = Utils.ConvertFormsToNative (formsView, new CGRect (0, 0, formsView.WidthRequest, formsView.HeightRequest));
					nativeView.BackgroundColor = UIColor.Clear;
					image = Utils.ConvertViewToImage (nativeView);
				}

				annotationView = new WPinAnnotationView (annotation, pin.Id);
				annotationView.CanShowCallout = false;
				annotationView.Image = image;
				((WPinAnnotationView)annotationView).Id = pin.Id;
			}

			return annotationView;
		}
			
		public WPin GetPin (MKPointAnnotation point) 
		{
			if (point is WCalloutAnnotation) {
				point = ((WCalloutAnnotation)point).OriginalAnnotation as MKPointAnnotation;
			}

			var position = new Position (point.Coordinate.Latitude, point.Coordinate.Longitude);
			foreach (var pin in MapControl.ExtendedPins) {
				if (pin.Pin.Position == position && point.GetType () != typeof(WCalloutAnnotation)) {
					return pin;
				}
			}
			return null;
		}

		void OnDidSelectAnnotationView (object sender, MKAnnotationViewEventArgs e)
		{
			if (e.View is WPinAnnotationView) {
				var calloutAnnotation = new WCalloutAnnotation {
					Coordinate = e.View.Annotation.Coordinate,
					OriginalAnnotation = e.View.Annotation
				};
				MapView.AddAnnotation (calloutAnnotation);
				MapView.SelectAnnotation (calloutAnnotation, true);
				CurrentCalloutView = null;
			}
		}
			
		void OnDidDeselectAnnotationView (object sender, MKAnnotationViewEventArgs e)
		{
			if (e.View is WCalloutView) {
				MapView.RemoveAnnotation ((e.View as WCalloutView).Annotation);
			}
		}

		void OnCalloutAccessoryControlTapped (object sender, MKMapViewAccessoryTappedEventArgs e)
		{

		}

	}
}

