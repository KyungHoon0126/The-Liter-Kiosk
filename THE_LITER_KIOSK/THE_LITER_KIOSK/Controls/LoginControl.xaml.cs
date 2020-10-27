using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using THE_LITER_KIOSK.UIManager;

namespace THE_LITER_KIOSK.Controls
{
    /// <summary>
    /// Interaction logic for LoginControl.xaml
    /// </summary>
    public partial class LoginControl : CustomControlModel
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

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return && btnLogin.IsEnabled)
            {
                OnLoginResultRecieved?.Invoke(this, true);
            }
        }

        #region UserControl Transition
        private void btnSignup_Click(object sender, RoutedEventArgs e)
        {
            App.uIStateManager.SwitchCustomControl(CustomControlType.SIGNUP);
        }
        #endregion
    }

    #region PasswordBoxMonitor
    public class PasswordBoxMonitor : DependencyObject
    {
        public static bool GetIsMonitoring(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsMonitoringProperty);
        }

        public static void SetIsMonitoring(DependencyObject obj, bool value)
        {
            obj.SetValue(IsMonitoringProperty, value);
        }

        public static readonly DependencyProperty IsMonitoringProperty =
            DependencyProperty.RegisterAttached("IsMonitoring", typeof(bool), typeof(PasswordBoxMonitor), new UIPropertyMetadata(false, OnIsMonitoringChanged));


        public static int GetPasswordLength(DependencyObject obj)
        {
            return (int)obj.GetValue(PasswordLengthProperty);
        }

        public static void SetPasswordLength(DependencyObject obj, int value)
        {
            obj.SetValue(PasswordLengthProperty, value);
        }

        public static readonly DependencyProperty PasswordLengthProperty =
            DependencyProperty.RegisterAttached("PasswordLength", typeof(int), typeof(PasswordBoxMonitor), new UIPropertyMetadata(0));

        private static void OnIsMonitoringChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pb = d as PasswordBox;
            if (pb == null)
            {
                return;
            }
            if ((bool)e.NewValue)
            {
                pb.PasswordChanged += PasswordChanged;
            }
            else
            {
                pb.PasswordChanged -= PasswordChanged;
            }
        }

        static void PasswordChanged(object sender, RoutedEventArgs e)
        {
            var pb = sender as PasswordBox;
            if (pb == null)
            {
                return;
            }
            SetPasswordLength(pb, pb.Password.Length);
        }
    }
    #endregion

    #region PasswordHelper
    public class PasswordHelper : DependencyObject
    {
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.RegisterAttached("Password",
            typeof(string), typeof(PasswordHelper),
            new FrameworkPropertyMetadata(string.Empty, OnPasswordPropertyChanged));

        public static readonly DependencyProperty AttachProperty =
            DependencyProperty.RegisterAttached("Attach",
            typeof(bool), typeof(PasswordHelper), new PropertyMetadata(false, Attach));

        private static readonly DependencyProperty IsUpdatingProperty =
            DependencyProperty.RegisterAttached("IsUpdating", typeof(bool),
            typeof(PasswordHelper));


        public static void SetAttach(DependencyObject dp, bool value)
        {
            dp.SetValue(AttachProperty, value);
        }

        public static bool GetAttach(DependencyObject dp)
        {
            return (bool)dp.GetValue(AttachProperty);
        }

        public static string GetPassword(DependencyObject dp)
        {
            return (string)dp.GetValue(PasswordProperty);
        }

        public static void SetPassword(DependencyObject dp, string value)
        {
            dp.SetValue(PasswordProperty, value);
        }

        private static bool GetIsUpdating(DependencyObject dp)
        {
            return (bool)dp.GetValue(IsUpdatingProperty);
        }

        private static void SetIsUpdating(DependencyObject dp, bool value)
        {
            dp.SetValue(IsUpdatingProperty, value);
        }

        private static void OnPasswordPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            passwordBox.PasswordChanged -= PasswordChanged;

            if (!(bool)GetIsUpdating(passwordBox))
            {
                passwordBox.Password = (string)e.NewValue;
            }

            passwordBox.PasswordChanged += PasswordChanged;
        }

        private static void Attach(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;

            if (passwordBox == null)
            {
                return;
            }

            if ((bool)e.OldValue)
            {
                passwordBox.PasswordChanged -= PasswordChanged;
            }

            if ((bool)e.NewValue)
            {
                passwordBox.PasswordChanged += PasswordChanged;
            }
        }

        private static void PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            SetIsUpdating(passwordBox, true);
            SetPassword(passwordBox, passwordBox.Password);
            SetIsUpdating(passwordBox, false);
        }
    }
    #endregion
}
