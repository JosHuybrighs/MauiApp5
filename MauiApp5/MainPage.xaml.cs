using CommunityToolkit.Maui.Extensions;
using MauiApp5.Services;
using MauiApp5.View;

namespace MauiApp5
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            /*
            // Choose the color you want the bars to match:
            Color pageColor = this.BackgroundColor;
            if (pageColor == null || pageColor.Equals(Colors.Transparent))
            {
                // fallback
                pageColor = Colors.White;
            }

            bool darkIcons = ShouldUseDarkIcons(pageColor);

            BarColorService.Instance.SetBarColors(pageColor, darkStatusBarIcons: darkIcons, darkNavigationBarIcons: darkIcons);
            */
        }

        static bool ShouldUseDarkIcons(Color c)
        {
            // simple luminance test (0..1)
            double lum = 0.299 * c.Red + 0.587 * c.Green + 0.114 * c.Blue;
            // if luminance is high -> use dark icons
            return lum > 0.5;
        }

        private async void OnCounterClicked(object? sender, EventArgs e)
        {
            /*
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
            */

            //var modalPage = new MyModalPage();
            //await Application.Current.MainPage.Navigation.PushModalAsync(modalPage);

            var dialog = new MyPopup();
            await this.ShowPopupAsync(dialog);
        }
    }
}
