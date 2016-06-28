using System;
using Xamarin.Forms.Maps;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Wapps.Forms
{
	public delegate View GetPinViewDelegateHandler (WPin pin);

	public class WMap : Map
	{
		public event EventHandler <WPin> RendererNeedToRefreshWindow;

		public List<WPin> ExtendedPins { get; private set; }

		public GetPinViewDelegateHandler GetPinViewDelegate { get; set; }

		public GetPinViewDelegateHandler GetPinWindowDelegate { get; set; }

		public event EventHandler<WPin> WindowClicked;

		public WMap ()
		{
			this.ExtendedPins = new List<WPin> ();
		}

		public void AddPin (WPin pin)
		{
			this.ExtendedPins.Add (pin);
			this.Pins.Add (pin.Pin);
		}

		public void FireWindowClicked (WPin pin)
		{
			if (this.WindowClicked != null) {
				this.WindowClicked (this, pin);
			}
		}

		public void RefreshWindowForPin (WPin pin)
		{
			if (this.RendererNeedToRefreshWindow != null) {
				this.RendererNeedToRefreshWindow (this, pin);
			}
		}

	}
}

