using System;
using Xamarin.Forms.Maps;
using System.Collections.Generic;

namespace Wapps.Forms
{
	public class WPin
	{
		public Pin Pin { get; set; }

		public string Id { get; set; }

		public Dictionary <string, object> Atts { get; set; }

		//public WeakReference <object> WindowInfo { get; set; }
	}
}

