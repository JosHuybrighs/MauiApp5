using MauiApp5.Services;

namespace MauiApp5.View;

public partial class MyModalPage : ContentPage
{
	public MyModalPage()
	{
		InitializeComponent();
	}

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        // Choose the color you want the bars to match:
        Color pageColor = this.BackgroundColor;
        if (pageColor == null || pageColor.Equals(Colors.Transparent))
        {
            // fallback
            pageColor = Colors.White;
        }

        bool darkIcons = ShouldUseDarkIcons(pageColor);

        BarColorService.Instance.SetBarColors(pageColor, darkStatusBarIcons: darkIcons, darkNavigationBarIcons: darkIcons);
    }


    static bool ShouldUseDarkIcons(Color c)
    {
        // simple luminance test (0..1)
        double lum = 0.299 * c.Red + 0.587 * c.Green + 0.114 * c.Blue;
        // if luminance is high -> use dark icons
        return lum > 0.5;
    }


}