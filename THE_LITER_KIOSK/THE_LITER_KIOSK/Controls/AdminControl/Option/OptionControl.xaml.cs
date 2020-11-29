using System.ComponentModel;
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
    public partial class OptionControl : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool _autoLogin = Setting.IsAutoLogin;
        public bool AutoLogin
        {
            get => _autoLogin;
            set
            {
                _autoLogin = value;
                NotifyPropertyChanged(nameof(AutoLogin));
                Setting.IsAutoLogin = _autoLogin;
                Setting.Save();
            }
        }

        public OptionControl()
        {
            InitializeComponent();
            Loaded += OptionControl_Loaded;
        }

        private void OptionControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = this;
        }

        private void cbAutoLogin_Checked(object sender, RoutedEventArgs e)
        {
            if ((sender as CheckBox).IsChecked ?? false)
            {
                Setting.IsAutoLogin = true;
            }
            else
            {
                Setting.IsAutoLogin = false;
            }

            Setting.Save();
        }

        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            if (TcpHelper.isConnected)
            {
                App.networkManager.DisconnectSocket();
            }

            Setting.IsAutoLogin = false;
            Setting.Save();
            App.uIStateManager.SwitchCustomControl(CustomControlType.LOGIN);
        }
    }
}
