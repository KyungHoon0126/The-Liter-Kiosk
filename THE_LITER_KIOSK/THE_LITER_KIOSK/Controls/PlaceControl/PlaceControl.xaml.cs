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

namespace THE_LITER_KIOSK.Controls.PlaceControl
{
    /// <summary>
    /// Interaction logic for PlaceControl.xaml
    /// </summary>
    public partial class PlaceControl : UserControl
    {
        public PlaceControl()
        {
            InitializeComponent();
            Loaded += PlaceControl_Loaded;
        }

        private void PlaceControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = App.placeData.tableViewModel;
        }
    }
}
