using System.Windows;
using THE_LITER_KIOSK.UIManager;

namespace THE_LITER_KIOSK.Controls.PayControl
{
    /// <summary>
    /// Interaction logic for PayControl.xaml
    /// </summary>
    public partial class PaySelectionControl : CustomControlModel
    {
        public PaySelectionControl()
        {
            InitializeComponent();
            Loaded += PaySelectionControl_Loaded;
        }

        private void PaySelectionControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = App.orderData.orderViewModel;
        }

        #region UserControl Transition
        private void btnMoveToPrevCtrl_Click(object sender, RoutedEventArgs e)
        {
            if (App.ctrlName == "TABLE")
                App.uIStateManager.SwitchCustomControl(CustomControlType.TABLE);
            else
                App.uIStateManager.SwitchCustomControl(CustomControlType.PLACE);
        }

        private void btnPayByCash_Click(object sender, RoutedEventArgs e)
        {
            App.uIStateManager.SwitchCustomControl(CustomControlType.PAYCASH);
        }

        private void btnPayByCard_Click(object sender, RoutedEventArgs e)
        {
            App.uIStateManager.SwitchCustomControl(CustomControlType.PAYCARD);
        }
        #endregion
    }
}
