using System;
using Xamarin.Forms;

namespace Wapps.Forms
{
	// this layout is used to render the android native views
	public class WMapAuxiliar : StackLayout
	{
		public static WMapAuxiliar LiveMap { get; set; }

		public WMapAuxiliar ()
		{
			LiveMap = this;
			this.Opacity = 0;
			this.WidthRequest = 1;
			this.HeightRequest = 1;
		}
	}
}

