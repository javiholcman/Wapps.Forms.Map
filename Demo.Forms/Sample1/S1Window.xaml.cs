using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Threading.Tasks;
using Wapps.Forms;

namespace Demo.Forms
{
	public partial class S1Window : StackLayout
	{
		public event EventHandler <WPin> ImageLoaded;

		WPin Pin { get; set; }

		public S1Window (WPin pin)
		{
			InitializeComponent ();
			this.Pin = pin;


			this.IvImage.Success += delegate(object sender, FFImageLoading.Forms.CachedImageEvents.SuccessEventArgs e) {
				if (this.ImageLoaded != null) {
					this.ImageLoaded (this, this.Pin);
				}
			};
		}

		public string ImageUrl { 
			set { 
				this.IvImage.Source = value;
			} 
		}

		public string Title { 
			set { 
				this.LblTitle.Text = value;
			} 
		}
			
		public string Description { 
			set { 
				this.LblDescription.Text = value;
			} 
		}

	}
}

