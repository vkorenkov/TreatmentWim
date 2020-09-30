using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;
using Treatment_Wim.ViewModels;

namespace Treatment_Wim.ViewModels
{
    class WindowBehavior : Behavior<Window>
    {
        public static WindowState prevState;

        public static Window window;

        protected override void OnAttached()
        {
            base.OnAttached();

            this.AssociatedObject.StateChanged += Window_StateChanged;
            this.AssociatedObject.MouseLeftButtonDown += TopPanel_MouseLeftButtonDown;
        }

        private void TopPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Window)
            {
                window = sender as Window;
            }

            window.DragMove();
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            window = sender as Window;

            if (window.WindowState == WindowState.Minimized)
            {
                window.Hide();
            }
            else
            {
                prevState = window.WindowState;
            }
        }

        public static void MinimizedWindow(Window window)
        {
            window.WindowState = WindowState.Minimized;
        }
    }
}
