using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;


#if WINDOWS
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Windows.Graphics;
using WinRT.Interop;
#endif

#if MACCATALYST
using UIKit;
using CoreGraphics;
#endif

namespace KalkulatorRatalnyMAUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("Poppins-Regular.ttf", "PoppinsRegular");
                    fonts.AddFont("Poppins-Semibold.ttf", "PoppinsSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

#if WINDOWS
            builder.ConfigureLifecycleEvents(events =>
            {
                events.AddWindows(windows =>
                {
                    windows.OnWindowCreated(window =>
                    {
                        var app = Microsoft.Maui.Controls.Application.Current;

                        if (app == null)
                        {
                            return;
                        }

                        var windowsCollection = app.Windows;

                        if (windowsCollection == null || windowsCollection.Count == 0)
                        {
                            return;
                        }

                        var mauiWindow = windowsCollection[0];

                        if (mauiWindow?.Handler == null)
                        {
                            return;
                        }

                        var nativeWindow = mauiWindow.Handler.PlatformView;

                        if (nativeWindow == null)
                        {
                            return;
                        }

                        IntPtr hwnd = WinRT.Interop.WindowNative.GetWindowHandle(nativeWindow);
                        var windowId = Win32Interop.GetWindowIdFromWindow(hwnd);
                        var appWindow = AppWindow.GetFromWindowId(windowId);

                        if (appWindow == null)
                        {
                            return;
                        }

                        int width = 1220;
                        int height = 780;

                        appWindow.Resize(new SizeInt32(width, height));

                        var displayArea = DisplayArea.GetFromWindowId(windowId, DisplayAreaFallback.Primary);

                        if (displayArea == null)
                        {
                            return;
                        }

                        int screenWidth = displayArea.WorkArea.Width;
                        int screenHeight = displayArea.WorkArea.Height;

                        int posX = (screenWidth - width) / 2 + displayArea.WorkArea.X;
                        int posY = (screenHeight - height) / 2 + displayArea.WorkArea.Y;

                        appWindow.Move(new PointInt32(posX, posY));
                    });
                });
            });
#endif

#if MACCATALYST
            builder.ConfigureLifecycleEvents(events =>
            {
                events.AddiOS(iOS =>
                {
                    iOS.SceneWillConnect((scene, session, options) =>
                    {
                        if (scene is UIWindowScene windowScene)
                        {
                            var window = windowScene.Windows.FirstOrDefault();

                            if (window != null)
                            {
                                int width = 1220;
                                int height = 980;

                                var screenBounds = UIScreen.MainScreen.Bounds;

                                double posX = (screenBounds.Width - width) / 2;
                                double posY = (screenBounds.Height - height) / 2;

                                window.Frame = new CGRect(posX, posY, width, height);
                            }
                        }
                    });
                });
            });
#endif

            return builder.Build();
        }
    }
}
