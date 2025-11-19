
using Android.App;
using Android.OS;
using Android.Views;
using AndroidX.Fragment.App;

namespace MauiApp5.Services
{
    public class BarColorService : IBarColorService
    {
        public static BarColorService Instance { get; } = new BarColorService();


        BarColorService()
        { }

        public void SetBarColors(Microsoft.Maui.Graphics.Color color, bool darkStatusBarIcons = false, bool darkNavigationBarIcons = false)
        {
            try
            {
                var activity = Platform.CurrentActivity as Activity;
                if (activity == null)
                    return;

                // Force fully opaque color to avoid blending with theme/scrim.
                int a = (int)(color.Alpha * 255f);
                int r = (int)(color.Red * 255f);
                int g = (int)(color.Green * 255f);
                int b = (int)(color.Blue * 255f);
                var androidColor = Android.Graphics.Color.Argb(255, r, g, b);

                // Apply to Activity window
                ApplyToWindowSafe(activity.Window, androidColor, darkStatusBarIcons, darkNavigationBarIcons);

                // If the Activity is a FragmentActivity, also search for DialogFragments and apply to their windows.
                if (activity is FragmentActivity fragActivity)
                {
                    foreach (var frag in fragActivity.SupportFragmentManager.Fragments)
                    {
                        if (frag is AndroidX.Fragment.App.DialogFragment df && df.Dialog?.Window != null)
                        {
                            ApplyToWindowSafe(df.Dialog.Window, androidColor, darkStatusBarIcons, darkNavigationBarIcons);
                        }
                    }
                }
            }
            catch
            {
                // swallow - don't crash the app for UI color changes
            }
        }

        // Apply color/flags to a specific native Window safely (posts to its decor view).
        void ApplyToWindowSafe(Android.Views.Window window, Android.Graphics.Color androidColor, bool darkStatusBarIcons, bool darkNavigationBarIcons)
        {
            if (window == null) return;

            var decor = window.DecorView;
            if (decor == null) return;

            void Apply()
            {
                try
                {
                    if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
                    {
                        window.ClearFlags(WindowManagerFlags.TranslucentStatus | WindowManagerFlags.TranslucentNavigation);
                        window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
                        window.SetStatusBarColor(androidColor);
                        window.SetNavigationBarColor(androidColor);
                    }

                    int flags = (int)decor.SystemUiVisibility;

                    if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
                    {
                        if (darkStatusBarIcons)
                            flags |= (int)SystemUiFlags.LightStatusBar;
                        else
                            flags &= ~(int)SystemUiFlags.LightStatusBar;
                    }

                    if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
                    {
                        if (darkNavigationBarIcons)
                            flags |= (int)SystemUiFlags.LightNavigationBar;
                        else
                            flags &= ~(int)SystemUiFlags.LightNavigationBar;
                    }

                    decor.SystemUiVisibility = (StatusBarVisibility)flags;
                }
                catch
                {
                    // ignore per-window failures
                }
            }

            // Post so it runs after view attachment/layout; fallback to GlobalLayout or RunOnUiThread.
            bool posted = false;
            try { posted = decor.Post(() => Apply()); } catch { posted = false; }

            if (!posted)
            {
                try
                {
                    void OnGlobalLayout(object? s, EventArgs e)
                    {
                        try { Apply(); }
                        finally
                        {
                            try
                            {
                                if (decor.ViewTreeObserver.IsAlive)
                                    decor.ViewTreeObserver.GlobalLayout -= OnGlobalLayout;
                            }
                            catch { }
                        }
                    }

                    if (decor.ViewTreeObserver.IsAlive)
                    {
                        decor.ViewTreeObserver.GlobalLayout += OnGlobalLayout;
                    }
                    else
                    {
                        // last resort: run on UI thread
                        Platform.CurrentActivity?.RunOnUiThread(() => Apply());
                    }
                }
                catch
                {
                    // swallow
                }
            }
        }
    }
}