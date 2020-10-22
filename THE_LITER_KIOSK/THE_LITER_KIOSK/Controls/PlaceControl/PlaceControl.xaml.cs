using System.Windows;
using THE_LITER_KIOSK.UIManager;

namespace THE_LITER_KIOSK.Controls.PlaceControl
{
    /// <summary>
    /// Interaction logic for PlaceControl.xaml
    /// </summary>
    public partial class PlaceControl : CustomControlModel
    {
        public PlaceControl()
        {
            InitializeComponent();
        }

        #region UserControl Transition
        private void btnStoreMeal_Click(object sender, RoutedEventArgs e)
        {
            App.ctrlName = "TABLE";
            App.uIStateManager.SwitchCustomControl(CustomControlType.TABLE);
        }

        private void btnPackingMeal_Click(object sender, RoutedEventArgs e)
        {
            App.ctrlName = "PAY";
            App.uIStateManager.SwitchCustomControl(CustomControlType.PAY);
        }

        private void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            App.uIStateManager.SwitchCustomControl(CustomControlType.ORDER);
        }
        #endregion
    }
}
 