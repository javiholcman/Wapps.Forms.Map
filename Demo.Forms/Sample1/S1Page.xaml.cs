using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Wapps.Forms;

namespace Demo.Forms
{
	public partial class S1Page : ContentPage
	{
		public S1Page ()
		{
			InitializeComponent ();
		
			// set the delegate to return the pinView
			this.Map.GetPinViewDelegate = GetPinView;

			// set the delegate to return the window
			this.Map.GetPinWindowDelegate = GetWindowView;

			this.Map.WindowClicked += Map_WindowClicked;

			LoadPins ();

			this.Map.MoveToRegion (MapSpan.FromCenterAndRadius (
				Map.Pins[0].Position, Distance.FromMiles (1.0))
			);
		}

		void Map_WindowClicked (object sender, WPin e)
		{
			DisplayAlert("Clicked", e.Pin.Label, "Ok");
		}

		/// <summary>
		/// for each pin, return the related view (userControl or whatever)
		/// </summary>
		/// <returns>The pin view.</returns>
		/// <param name="pin">Pin.</param>
		View GetPinView (WPin pin)
		{
			var pinView = new S1PinView();
			pinView.Price = pin.Atts ["Price"].ToString ();
			return pinView;
		}

		/// <summary>
		/// for each pin, return the related window (userControl or whatever)
		/// </summary>
		/// <returns>The window view.</returns>
		/// <param name="pin">Pin.</param>
		View GetWindowView (WPin pin)
		{
			var window = new S1Window (pin);
			window.ImageLoaded += delegate(object sender, WPin e) {
				Map.RefreshWindowForPin (pin);					
			};
			window.ImageUrl = pin.Atts ["ImageUrl"].ToString ();
			window.Title = pin.Pin.Label;
			window.Description = pin.Atts ["Description"].ToString ();
			return window;
		}

		void LoadPins ()
		{
			WPin pin;

			pin = new WPin {
				Pin = new Pin {
					Type = PinType.Place,
					Position = new Position (-34.6013, -58.3795),
					Label = "Javo"
				},
				Id = "1",
				// on the attributes, put the info you need to customize your pin/window
				Atts = new Dictionary<string, object> {
					{ "ImageUrl", "http://fmhvibe.co.uk/community/uploads/b095eb4e9b8fae5afef6fa79a509a1ab.jpg" },
					{ "Description", "Mi nombre es Javo !!" },
					{ "Price", 25 }
				}
			};
			Map.AddPin (pin);

			pin = new WPin {
				Pin = new Pin {
					Type = PinType.Place,
					Position = new Position (-34.603412, -58.377493),
					Label = "Maradona"
				},
				Id = "2",
				Atts = new Dictionary<string, object> {
					{ "ImageUrl", "https://julian095.files.wordpress.com/2010/03/16563maradona411mf11.jpeg" },
					{ "Description", "Mi nombre es Maradona !!" },
					{ "Price", 12 }
				}
			};
			Map.AddPin (pin);

			pin = new WPin {
				Pin = new Pin {
					Type = PinType.Place,
					Position = new Position (-34.603439, -58.387557),
					Label = "Messi"
				},
				Id = "3",
				Atts = new Dictionary<string, object> {
					{ "ImageUrl", "https://www.occrp.org/assets/panamapapers/persons/messi.png" },
					{ "Description", "Mi nombre es Messi !!" },
					{ "Price", 89 }
				}
			};
			Map.AddPin (pin);

			pin = new WPin {
				Pin = new Pin {
					Type = PinType.Place,
					Position = new Position (-34.610480, -58.384539),
					Label = "Pele"
				},
				Id = "4",
				Atts = new Dictionary<string, object> {
					{ "ImageUrl", "http://www.fourfourtwo.hu/admin/ckfinder/userfiles/images/Pele%20nagy.jpg" },
					{ "Description", "Mi nombre es Pele !!" },
					{ "Price", 76 }
				}
			};
			Map.AddPin (pin);
		}
	}
}

