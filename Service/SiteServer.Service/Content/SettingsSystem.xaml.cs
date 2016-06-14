using FirstFloor.ModernUI.Windows.Controls;
using SiteServer.Service.Core;
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

namespace SiteServer.Service.Content
{
    /// <summary>
    /// Interaction logic for ControlsStylesSampleForm.xaml
    /// </summary>
    public partial class SettingsSystem : UserControl
    {
        public SettingsSystem()
        {
            InitializeComponent();

            this.Loaded += OnLoaded;
        }

        void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.TextDirectoryPath.Text = ConfigurationManager.DirectoryPath;
            Keyboard.Focus(this.TextDirectoryPath);

            this.ButtonSubmit.Click += ButtonSubmit_Click;
        }

        void ButtonSubmit_Click(object sender, RoutedEventArgs e)
        {
            string directoryPath = this.TextDirectoryPath.Text;
            bool success = ConfigurationManager.SaveDirectoryPath(directoryPath);

            ModernDialog dialog = new ModernDialog
            {
                Title = "参数设置"
            };
            dialog.CloseButton.Content = "关 闭";

            if (success)
            {
                dialog.Content = "参数设置成功，SiteServer Service 服务将自动开启！";
            }
            else
            {
                dialog.Content = "参数设置失败，请设置正确的文件夹位置！";
                this.TextDirectoryPath.Text = string.Empty;
            }

            dialog.ShowDialog();
        }
    }
}
