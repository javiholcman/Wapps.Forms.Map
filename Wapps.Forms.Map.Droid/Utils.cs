using System;
using Android.App;
using Android.Graphics.Drawables;
using Android.Graphics;
using Android.Views;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Wapps.Forms.Map.Droid;

namespace Wapps.Forms.Droid
{
	class Utils
	{
		/// <summary>
		/// convert from dp to pixels
		/// </summary>
		/// <param name="dp">Dp.</param>
		public static int DpToPx (float dp)
		{
			var metrics = Android.App.Application.Context.Resources.DisplayMetrics;
			return (int)(dp * metrics.Density);
		}

		/// <summary>
		/// convert from px to dp
		/// </summary>
		/// <param name="px">Px.</param>
		public static float PxToDp (int px)
		{
			var metrics = Android.App.Application.Context.Resources.DisplayMetrics;
			return px / metrics.Density;
		}

		public static Bitmap DrawableToBitmap (Drawable drawable) 
		{
			if (drawable is BitmapDrawable) {
				return ((BitmapDrawable)drawable).Bitmap;
			}

			int width = drawable.IntrinsicWidth;
			width = width > 0 ? width : 1;
			int height = drawable.IntrinsicHeight;
			height = height > 0 ? height : 1;

			Bitmap bitmap = Bitmap.CreateBitmap (width, height, Bitmap.Config.Argb8888);
			Canvas canvas = new Canvas(bitmap); 
			drawable.SetBounds(0, 0, canvas.Width, canvas.Height);
			drawable.Draw (canvas);

			return bitmap;
		}

		public static int GetImageResource (String imageName)
		{
			if (imageName.Contains (".")) {
				imageName = imageName.Substring (0,imageName.IndexOf ('.'));
			}
				
			return (int)WResources.DrawableType.GetField (imageName).GetValue(null);
		}

		public static ViewGroup ConvertFormsToNative (Xamarin.Forms.View view, Rectangle size)
		{
			var vRenderer = Platform.CreateRenderer (view);
			var viewGroup = vRenderer.ViewGroup;
			vRenderer.Tracker.UpdateLayout ();
			var layoutParams = new ViewGroup.LayoutParams ((int)size.Width, (int)size.Height);
			viewGroup.LayoutParameters = layoutParams;
			view.Layout (size);
			viewGroup.Layout (0, 0, (int)view.WidthRequest, (int)view.HeightRequest);
			FixImageSourceOfImageViews (viewGroup as ViewGroup);
			return viewGroup; 
		}
			
		public static Bitmap ConvertViewToBitmap (Android.Views.View v)
		{
			v.SetLayerType (LayerType.Software, null);
			v.DrawingCacheEnabled = true;

			v.Measure (Android.Views.View.MeasureSpec.MakeMeasureSpec (0, MeasureSpecMode.Unspecified),
				Android.Views.View.MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified));
			v.Layout(0, 0, v.MeasuredWidth, v.MeasuredHeight);

			v.BuildDrawingCache (true);
			Bitmap b = Bitmap.CreateBitmap(v.GetDrawingCache(true));
			v.DrawingCacheEnabled =false; // clear drawing cache
			return b;
		}

		public static Android.Widget.FrameLayout AddViewOnFrameLayout (Android.Views.View view, int width, int height)
		{
			var layout = new Android.Widget.FrameLayout (view.Context);
			layout.LayoutParameters = new ViewGroup.LayoutParams (ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
			view.LayoutParameters = new Android.Widget.FrameLayout.LayoutParams (width, height);
			layout.AddView (view);
			return layout;
		}

		public static void FixImageSourceOfImageViews (ViewGroup parent)
		{
			try {
				for (var i = 0; i < parent.ChildCount; i++) {
					var view = parent.GetChildAt (i);
					if (view is Android.Widget.ImageView) {

						var imageView = view as Android.Widget.ImageView;
						var imageViewRenderer = imageView.OnFocusChangeListener as ImageRenderer;

						if (imageViewRenderer.Element.Source is FileImageSource) {
							var source = imageViewRenderer.Element.Source as FileImageSource;
							var resId = GetImageResource (source.File);
							imageView.SetImageResource (resId);
						}
						else if (imageViewRenderer.Element.Source is UriImageSource) {
							//var source = imageViewRenderer.Element.Source as UriImageSource;
							//var drawable = Drawable.CreateFromPath (source.Uri.LocalPath);

							//imageView.SetImageDrawable (drawable);
						}
					} 
					if (view is ViewGroup) {
						FixImageSourceOfImageViews (view as ViewGroup);
					}
				}
			} catch (Exception ex) {

			}
		}

	}
}

