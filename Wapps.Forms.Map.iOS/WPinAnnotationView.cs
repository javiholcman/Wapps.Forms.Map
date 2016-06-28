using System;
using MapKit;
using CoreGraphics;
using UIKit;

namespace Wapps.Forms.iOS
{
	public class WPinAnnotationView : MKAnnotationView
	{
		public string Id { get; set; }

		public WPinAnnotationView (IMKAnnotation annotation, string reuseIdentifier) : base (annotation, reuseIdentifier)
		{
			
		}
	}
}

