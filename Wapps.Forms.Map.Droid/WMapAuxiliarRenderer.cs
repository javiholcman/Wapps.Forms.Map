using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Wapps.Forms.Droid;

[assembly:ExportRenderer (typeof(Wapps.Forms.WMapAuxiliar), typeof(WMapAuxiliarRenderer))]

namespace Wapps.Forms.Droid
{
	public class WMapAuxiliarRenderer : VisualElementRenderer <StackLayout>
	{
		public static WMapAuxiliarRenderer LiveMapRenderer { get; set; }

		public WMapAuxiliarRenderer ()
		{
			LiveMapRenderer = this;
		}

		public Android.Views.View GetNativeView (View element)
		{
			this.Element.Children.Add (element);

			Android.Views.View targetView = null;

			for (int i = 0; i < ViewGroup.ChildCount; i++) {
				var view = ViewGroup.GetChildAt (i);

				var property = view.GetType ().GetProperty ("Element");
				if (property != null) {
					var elem = property.GetValue (view);
					if (elem == element) {
						targetView = view;
						break;
					}
				}
			}

			if (targetView == null) {
				return null;
			}

			((Android.Views.ViewGroup)targetView.Parent).RemoveView (targetView);

			var container = new Android.Widget.FrameLayout (this.Context);
			container.LayoutParameters = new LayoutParams (LayoutParams.WrapContent, LayoutParams.WrapContent);
			targetView.LayoutParameters = new Android.Widget.FrameLayout.LayoutParams (Utils.DpToPx ((float)element.WidthRequest), Utils.DpToPx((float)element.HeightRequest));
			container.AddView(targetView);

			return container;
		}
	}
}

