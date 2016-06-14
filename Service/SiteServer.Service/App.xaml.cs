using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using SiteServer.Task.Core;
using SiteServer.Task.View;

namespace SiteServer.Task
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private NotifyIconWrapper component;
        private DispatcherTimer dt = new DispatcherTimer();

        protected override void OnStartup(System.Windows.StartupEventArgs e)
        {
            base.OnStartup(e);

            this.ShutdownMode = System.Windows.ShutdownMode.OnExplicitShutdown;

            component = new NotifyIconWrapper();

            // Create and show the application's main window
            MainWindow window = new MainWindow();
            //Window1 window = new Window1();
            window.Show();
        }

        public void Activate()
        {
            // Reactivate application's main window
            this.MainWindow.Show();
            this.MainWindow.Activate();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            component.Dispose();
        }

        void dt_Tick(object sender, EventArgs e)
        {
            string sourcePath = @"Z:\Web\upload";
            string targetPath = @"D:\Utils\Backup\img";

            FileSystemUtils.CopyDirectory(sourcePath, targetPath, false);
        }
    }
}
