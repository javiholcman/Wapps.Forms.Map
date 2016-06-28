using System;
using UIKit;
using CoreGraphics;
using MapKit;
using Foundation;

namespace Wapps.Forms.iOS
{
	public class WCalloutView : MKAnnotationView
	{
		public IMKAnnotation MyAnnotation { get; set; }

		public event EventHandler Touched;

		UIView RoundedView { get; set; }

		UIImageView ArrowImageView { get; set; }

		UIView _view;
		public UIView View { 
			set { 
				if (_view != null) {
					_view.RemoveFromSuperview ();
				}
				_view = value;
				this.RoundedView.AddSubview (_view);
			}
		}

		public WCalloutView (IMKAnnotation annotation)
		{
			this.MyAnnotation = annotation;

			this.RoundedView = new UIView ();
			this.RoundedView.Layer.CornerRadius = 14;
			this.RoundedView.ClipsToBounds = true;
			this.RoundedView.BackgroundColor = UIColor.White.ColorWithAlpha (0.95f);
			this.AddSubview (this.RoundedView);

			var fileName = NSBundle.MainBundle.PathForResource ("calloutArrow", "png");
			var image = UIImage.FromFile (fileName);
			this.ArrowImageView = new UIImageView (image);
			this.AddSubview (this.ArrowImageView);
		}
			
		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
			this.RoundedView.Frame = new CGRect (0, 0, this.Frame.Width, this.Frame.Height - 20);
			_view.Frame = this.RoundedView.Bounds;
			this.ArrowImageView.Frame = new CGRect ((this.Frame.Width - this.ArrowImageView.Frame.Width) / 2, this.Frame.Height - this.ArrowImageView.Frame.Height - 2, this.ArrowImageView.Frame.Width, this.ArrowImageView.Frame.Height);
		}
			
		public override void TouchesEnded (Foundation.NSSet touches, UIEvent evt)
		{
			base.TouchesEnded (touches, evt);
			if (this.Touched != null) {
				this.Touched (this, null);
			}
		}
	}

	public class WCalloutAnnotation : MKPointAnnotation
	{
		public IMKAnnotation OriginalAnnotation { get; set; }
	}

}

