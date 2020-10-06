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

namespace THE_LITER_KIOSK.Controls.TableControl
{
    /// <summary>
    /// Interaction logic for TableControl.xaml
    /// </summary>
    public partial class TableControl : UserControl
    {
        public TableControl()
        {
            InitializeComponent();
            Loaded += TableControl_Loaded;
        }

        private async void TableControl_Loaded(object sender, RoutedEventArgs e)
        {
            await App.placeData.tableViewModel.LoadTableData();
            this.DataContext = App.placeData.tableViewModel;
        }
    }
}
