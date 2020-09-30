using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;
using Treatment_Wim.ViewModels;

namespace Treatment_Wim.ViewModels
{
    class ExpandBehavior : Behavior<TaskbarIcon>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            this.AssociatedObject.TrayMouseDoubleClick += AssociatedObject_TrayMouseDoubleClick;
        }

        private void AssociatedObject_TrayMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            WindowBehavior.window.Show();
            WindowBehavior.window.WindowState = WindowBehavior.prevState;
        }
    }
}
