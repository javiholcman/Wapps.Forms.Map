using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Demo.Forms 
{
	public partial class S1PinView : StackLayout
	{
		public S1PinView ()
		{
			InitializeComponent ();
		}

		public string Price {
			set {
				this.LblPrice.Text = value;
			}
		}
	}
}