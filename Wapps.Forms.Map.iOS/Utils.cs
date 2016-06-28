using System;
using UIKit;
using Xamarin.Forms.Platform.iOS;
using CoreGraphics;

namespace Wapps.Forms.iOS
{
	public static class Utils
	{
		public static UIView ConvertFormsToNative (Xamarin.Forms.View view, CGRect size)
		{
			var renderer = Platform.CreateRenderer(view);

			renderer.NativeView.Frame = size;

			renderer.NativeView.AutoresizingMask = UIViewAutoresizing.All;
			renderer.NativeView.ContentMode = UIViewContentMode.ScaleToFill;

			renderer.Element.Layout (size.ToRectangle());

			var nativeView = renderer.NativeView;

			nativeView.SetNeedsLayout ();

			return nativeView;
		}

		public static UIImage ConvertViewToImage (UIView view)
		{
			UIGraphics.BeginImageContextWithOptions(view.Bounds.Size, false, 0);
			view.Layer.RenderInContext(UIGraphics.GetCurrentContext());
			UIImage img = UIGraphics.GetImageFromCurrentImageContext();
			UIGraphics.EndImageContext();

			return img;
		}
	}
}

