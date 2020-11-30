using System.Windows;
using System.Windows.Controls;
using THE_LITER_KIOSK.Common;
using THE_LITER_KIOSK.Network;
using THE_LITER_KIOSK.UIManager;

namespace THE_LITER_KIOSK.Controls.AdminControl
{
    /// <summary>
    /// Interaction logic for OptionControl.xaml
    /// </summary>
    public partial class OptionControl : UserControl
    {
        public OptionControl()
        {
            InitializeComponent();
            Loaded += OptionControl_Loaded;
        } 

        private void OptionControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = App.autoLoginProxy;
        }

        private void cbAutoLogin_Checked(object sender, RoutedEventArgs e)
        {
            if ((sender as CheckBox).IsChecked ?? false)
            {
                App.autoLoginProxy.IsAutoLogin = true;
            }
            else
            {
                App.autoLoginProxy.IsAutoLogin = false;
            }

            Setting.Save();
        }

        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            if (TcpHelper.SocketClient.Connected)
            {
                App.networkManager.DisconnectSocket();
            }

            App.autoLoginProxy.IsAutoLogin = false;
            App.uIStateManager.SwitchCustomControl(CustomControlType.LOGIN);
        }
    }
}
