using System.Windows;
using Caliburn.Micro;
using MahApps.Metro.Controls;

namespace LeagueRecorder.Windows.Caliburn
{
    public class MetroWindowManager : WindowManager
    {
        protected override Window EnsureWindow(object model, object view, bool isDialog)
        {
            Window window = view as CaliburnMetroWindow;

            if (window == null)
            {
                window = new CaliburnMetroWindow
                {
                    Content = view, 
                    SizeToContent = SizeToContent.WidthAndHeight
                };
                window.SetValue(View.IsGeneratedProperty, true);

                Window owner = this.InferOwnerOf(window);
                if (owner != null)
                {
                    window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    window.Owner = owner;
                }
                else
                { 
                    window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                }
            }
            else
            {
                Window owner = this.InferOwnerOf(window);
                if (owner != null && isDialog)
                    window.Owner = owner;
            }

            return window;
        }
    }
}