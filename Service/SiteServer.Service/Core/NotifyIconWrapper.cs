using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SiteServer.Service.Core
{
    public partial class NotifyIconWrapper : Component
    {
        public NotifyIconWrapper()
        {
            InitializeComponent();

            cmdShowWindow.Click += cmdShowWindow_Click;
            cmdSettings.Click += cmdSettings_Click;
            cmdShutdown.Click += cmdShutdown_Click;
        }

        public NotifyIconWrapper(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        void cmdShutdown_Click(object sender, EventArgs e)
        {
            var result = ModernDialog.ShowMessage("此操作将关闭 SiteServer Service 服务，确认吗?", "关闭服务确认", MessageBoxButton.YesNo);

            if (result.ToString() == "True")
            {
                System.Windows.Application.Current.Shutdown();
            }
        }

        void cmdSettings_Click(object sender, EventArgs e)
        {
            MainWindow mainWindow = System.Windows.Application.Current.MainWindow as MainWindow;
            if (mainWindow == null)
            {
                mainWindow = new MainWindow();
            }

            if (mainWindow.WindowState == System.Windows.WindowState.Minimized) mainWindow.WindowState = System.Windows.WindowState.Normal;
            mainWindow.ContentSource = new Uri("/Pages/Settings.xaml", UriKind.Relative);
            mainWindow.Show();
            mainWindow.Activate();
        }

        void cmdShowWindow_Click(object sender, EventArgs e)
        {
            MainWindow mainWindow = System.Windows.Application.Current.MainWindow as MainWindow;
            if (mainWindow == null)
            {
                mainWindow = new MainWindow();
            }

            if (mainWindow.WindowState == System.Windows.WindowState.Minimized) mainWindow.WindowState = System.Windows.WindowState.Normal;
            mainWindow.ContentSource = new Uri("/Pages/Introduction.xaml", UriKind.Relative);
            mainWindow.Show();
            mainWindow.Activate();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            cmdShowWindow_Click(sender, e);
        }
    }
}
