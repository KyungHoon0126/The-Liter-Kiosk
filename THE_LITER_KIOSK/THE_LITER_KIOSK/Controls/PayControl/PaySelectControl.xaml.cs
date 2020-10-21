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

        #region UserControl Transition
        private void PaySelectionControl_Loaded(object sender, RoutedEventArgs e)
        {
             this.DataContext = App.orderData.orderViewModel;
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
