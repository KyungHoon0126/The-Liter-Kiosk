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

        private void btnStoreMeal_Click(object sender, RoutedEventArgs e)
        {
            App.uIStateManager.SwitchCustomControl(CustomControlType.TABLE);
        }

        private void btnPackingMeal_Click(object sender, RoutedEventArgs e)
        {
            App.uIStateManager.SwitchCustomControl(CustomControlType.PAY);
        }

        private void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            App.uIStateManager.SwitchCustomControl(CustomControlType.ORDER);
        }
    }
}
