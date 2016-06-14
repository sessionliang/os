using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;
using Microsoft.VisualBasic.ApplicationServices;
using SiteServer.Service.ClickOnce;
using SiteServer.Service.Core;

namespace SiteServer.Service
{
    public class Startup
    {
        [STAThread]
        public static void Main(string[] args)
        {
            bool isHide = false;
            foreach (string arg in args)
            {
                if (arg == "-hide")
                {
                    isHide = true;
                }
            }
            SingleInstanceManager manager = new SingleInstanceManager(isHide);
            manager.Run(args);
        }
    }

    // Using VB bits to detect single instances and process accordingly:
    //  * OnStartup is fired when the first instance loads
    //  * OnStartupNextInstance is fired when the application is re-run again
    //    NOTE: it is redirected to this instance thanks to IsSingleInstance
    public sealed class SingleInstanceManager : WindowsFormsApplicationBase, IDisposable
    {
        SingleInstanceApplication app;
        bool isHide = false;

        public SingleInstanceManager(bool isHide)
        {
            this.isHide = isHide;
            this.IsSingleInstance = true;
        }

        protected override bool OnStartup(Microsoft.VisualBasic.ApplicationServices.StartupEventArgs e)
        {
            if (!this.isHide)
            {
                SplashScreen splashScreen = new System.Windows.SplashScreen("splash.png");
                splashScreen.Show(true);
            }

            // First time app is launched
            app = new SingleInstanceApplication(isHide);
            app.Run();
            return false;
        }

        protected override void OnStartupNextInstance(StartupNextInstanceEventArgs eventArgs)
        {
            // Subsequent launches
            base.OnStartupNextInstance(eventArgs);
            app.Activate();
        }

        public void Dispose()
        {
            app.Dispose();
            GC.SuppressFinalize(this);
        }
    }

    public sealed class SingleInstanceApplication : Application, IDisposable
    {
        private NotifyIconWrapper component;
        private bool isHide = false;

        public SingleInstanceApplication(bool isHide)
        {
            this.isHide = isHide;
            ResourceDictionary rd1 = new ResourceDictionary();
            rd1.Source = new Uri("FirstFloor.ModernUI;component/Assets/ModernUI.xaml", UriKind.Relative);
            this.Resources.MergedDictionaries.Add(rd1);
            ResourceDictionary rd2 = new ResourceDictionary();
            rd2.Source = new Uri("FirstFloor.ModernUI;component/Assets/ModernUI.Light.xaml", UriKind.Relative);
            this.Resources.MergedDictionaries.Add(rd2);

            this.ShutdownMode = System.Windows.ShutdownMode.OnExplicitShutdown;
        }

        protected override void OnStartup(System.Windows.StartupEventArgs e)
        {
            try
            {
                var clickOnceHelper = new ClickOnceHelper(Globals.PublisherName, Globals.ProductName);
                clickOnceHelper.UpdateUninstallParameters();
                clickOnceHelper.AddShortcutToStartup();
                clickOnceHelper.AddStat();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            base.OnStartup(e);

            component = new NotifyIconWrapper();

            if (!this.isHide)
            {
                MainWindow window = new MainWindow();
                window.Show();
            }

            ExecuteManager executeManager = new ExecuteManager();
            executeManager.Execute();
        }

        public void Activate()
        {
            // Reactivate application's main window
            if (this.MainWindow != null)
            {
                this.MainWindow.Show();
                this.MainWindow.Activate();
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            component.Dispose();
        }

        public void Dispose()
        {
            component.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}