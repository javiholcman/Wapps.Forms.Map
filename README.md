# Wapps.Forms.Map
Control for Xamarin.Forms to show a Map with custom views on pins and windows without custom renders. For iOS &amp; Android.



### Basic Implementation
1) Add the nuget package: Wapps.Forms.Map on the 3 projects (ios, android and pcl)
2) On the iOS project, AppDelegate:
```c#
public override bool FinishedLaunching (UIApplication app, NSDictionary options)
{
	// Add this Add this line to use WMap
	// Is necesary to force load the assembly
	Console.WriteLine (typeof(Wapps.Forms.iOS.WMapRenderer).Name);

	global::Xamarin.Forms.Forms.Init ();

	global::Xamarin.FormsMaps.Init();

	LoadApplication (new App ());

	return base.FinishedLaunching (app, options);
}
```
2.b) On the Android project, MainActivity:
```c#
protected override void OnCreate (Bundle bundle)
{
	base.OnCreate(bundle);

	Wapps.Forms.Droid.WResources.DrawableType = typeof(Demo.Droid.Resource.Drawable);

	global::Xamarin.Forms.Forms.Init (this, bundle);

	LoadApplication (new App ());
}
```

3) Create some page and add the WMap control. Is necesary also to add a WMapAuxiliar.

```xml
<ContentPage xmlns="http://xamarin.com/schemas/2014/forms"
	xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
	xmlns:WappsMap="clr-namespace:Wapps.Forms;assembly=Wapps.Forms.Map"
	x:Class="Demo.Forms.S1Page">

 	<StackLayout>

 		<WappsMap:WMap x:Name="Map" />

 		<WappsMap:WMapAuxiliar />

 	</StackLayout>

</ContentPage>
```

4) Create a class for your Pin view either using Xaml or code:
```c#
<StackLayout xmlns="http://xamarin.com/schemas/2014/forms" 
			 xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml" 
			 WidthRequest="37" 
			 HeightRequest="48"
			 AnchorX="0.5"
			 AnchorY="1.0"
			 x:Class="Demo.Forms.S1PinView">
	<AbsoluteLayout VerticalOptions="FillAndExpand" HorizontalOptions="FillAndExpand">
		<Image Source="pin_red.png" 
			AbsoluteLayout.LayoutBounds="0, 0, 37, 48"
			AbsoluteLayout.LayoutFlags="None" />
	</AbsoluteLayout>
</StackLayout>
```

5) On the .cs of the page, when the map is created, setup the delegates to return the related views for the pin and the window.

```c#
public S1Page ()
{
	InitializeComponent ();

	// set the delegate to return the pinView
	this.Map.GetPinViewDelegate = delegate (WPin pin) {
		var pinView = new S1PinView(); // can be a user control or whatever
		return pinView;
	};


	// set the delegate to return the window
	this.Map.GetPinWindowDelegate = delegate (WPin pin) {
		var window = new Window(); // can be a user control or whatever
		window.Setup(pin);
		return window;
	};			
}
```

5) Add pins
```c#

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

```

### Warnings
* The pinView and windowView are rendered as bitmap because of the map sdk. So if you want to refresh the view after shown, call: 
```c#
Map.RefreshWindowForPin (pin);
```
* The Image control of Xamarin.Forms are not being rendered when source is of Uri type (on Android). Therefore use ffimageloading to remote image loading.

```xml
<ffimageloading:CachedImage x:Name="IvImage"
	HorizontalOptions="Start" 
	VerticalOptions="Start"
	WidthRequest="70" HeightRequest="70"
	DownsampleToViewSize="true" />
```
As mentioned before, when the ivImage has been loaded call refresh to the map:
```c#
this.IvImage.Success += delegate(object sender, FFImageLoading.Forms.CachedImageEvents.SuccessEventArgs e) {
	this.ParentPage.Map.RefreshWindowForPin (this.Pin);
};
```