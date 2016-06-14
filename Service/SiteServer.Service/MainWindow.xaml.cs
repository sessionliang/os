using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.ComponentModel;

namespace SiteServer.Service
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ModernWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
        }

        private void window_Closing(object sender, CancelEventArgs e)
        {
            //// Don't close the window, because we might need it later.
            //// Just conceal it.
            //e.Cancel = true;
            //this.WindowState = WindowState.Minimized;
            //this.ShowInTaskbar = false;

            this.Hide();
        }
    }
}
