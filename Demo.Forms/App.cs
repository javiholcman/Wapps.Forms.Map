using System;
using Xamarin.Forms;

namespace Demo.Forms
{
	public class App : Application
	{
		public App ()
		{
			MainPage = new NavigationPage (new S1Page ());
		}
	}
}

