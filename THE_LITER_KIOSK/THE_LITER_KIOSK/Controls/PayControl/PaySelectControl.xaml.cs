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
    }
}
