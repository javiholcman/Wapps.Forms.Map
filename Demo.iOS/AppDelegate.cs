using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using UIKit;
using FFImageLoading.Forms.Touch;
using Demo.Forms;

namespace Demo.iOS
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			// Force to load renderers
			Console.WriteLine (typeof(Wapps.Forms.iOS.WMapRenderer).Name);

			CachedImageRenderer.Init();

			global::Xamarin.Forms.Forms.Init ();

			global::Xamarin.FormsMaps.Init();

			LoadApplication (new App ());

			return base.FinishedLaunching (app, options);
		}
	}
}

