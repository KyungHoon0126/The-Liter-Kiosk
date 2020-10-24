using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace THE_LITER_KIOSK.Controls
{
    /// <summary>
    /// Interaction logic for LoginControl.xaml
    /// </summary>
    public partial class LoginControl : UserControl
    {
        public delegate void OnLoginResultRecievedHandler(object sender, bool success);
        public event OnLoginResultRecievedHandler OnLoginResultRecieved;

        public LoginControl()
        {
            InitializeComponent();
            Loaded += LoginControl_Loaded;
        }

        private void LoginControl_Loaded(object sender, RoutedEventArgs e)
        {
            btnLogin.IsEnabled = false;
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (btnLogin.IsEnabled)
            {
                // if () => 로그인 성공, true
                // else => false

                // 일단은 true
                OnLoginResultRecieved?.Invoke(this, true);
            }
        }

        private void tb_TextChanged(object sender, RoutedEventArgs e)
        {
            if (!CheckyEmpty())
            {
                btnLogin.IsEnabled = true;
            }
            else
            {
                btnLogin.IsEnabled = false;
            }
        }

        private bool CheckyEmpty()
        {
            string id = tbId.Text.Trim();
            string pw = pbPw.Password.Trim();

            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(pw))
            {
                return true;
            }
            return false;
        }

        private void UserControl_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Return && btnLogin.IsEnabled)
            {
                OnLoginResultRecieved?.Invoke(this, true);
            }
        }
    }
}
