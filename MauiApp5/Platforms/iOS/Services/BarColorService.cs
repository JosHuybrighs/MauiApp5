
namespace MauiApp5.Services
{
    public class BarColorService : IBarColorService
    {
        public static BarColorService Instance { get; } = new BarColorService();

        public void SetBarColors(Microsoft.Maui.Graphics.Color color, bool darkStatusBarIcons = false, bool darkNavigationBarIcons = false)
        {
        }
    }
}