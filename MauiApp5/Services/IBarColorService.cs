// Services/IBarColorService.cs
namespace MauiApp5.Services
{
    public interface IBarColorService
    {
        /// <summary>
        /// Set status bar and navigation bar color on Android.
        /// </summary>
        /// <param name="color">MAUI color to apply.</param>
        /// <param name="darkStatusBarIcons">true => dark icons/text in status bar (requires API 23+)</param>
        /// <param name="darkNavigationBarIcons">true => dark icons for navigation bar (requires API 26+)</param>
        void SetBarColors(Microsoft.Maui.Graphics.Color color, bool darkStatusBarIcons = false, bool darkNavigationBarIcons = false);
    }
}