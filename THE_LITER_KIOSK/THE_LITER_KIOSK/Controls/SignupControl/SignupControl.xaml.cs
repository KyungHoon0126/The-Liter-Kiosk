using System.Windows;
using THE_LITER_KIOSK.UIManager;

namespace THE_LITER_KIOSK.Controls.SignupControl
{
    /// <summary>
    /// UserControl1.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SignupControl : CustomControlModel
    {
        public SignupControl()
        {
            InitializeComponent();
            Loaded += SignupControl_Loaded;
        }

        private void SignupControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = App.memberData.memberViewModel;
        }

        #region UserControl Transition
        private void btnMoveToLogin_Click(object sender, RoutedEventArgs e)
        {
            App.uIStateManager.SwitchCustomControl(CustomControlType.LOGIN);
        }
        #endregion
    }
}
