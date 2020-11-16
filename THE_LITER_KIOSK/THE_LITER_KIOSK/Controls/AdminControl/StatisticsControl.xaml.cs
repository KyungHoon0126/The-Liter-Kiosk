using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using THE_LITER_KIOSK.DataBase.Models;

namespace THE_LITER_KIOSK.Controls.AdminControl
{
    /// <summary>
    /// Interaction logic for StatisticsControl.xaml
    /// </summary>
    public partial class StatisticsControl : UserControl
    {
        public StatisticsControl()
        {
            InitializeComponent();
            Loaded += StatisticsControl_Loaded;
        }

        private void StatisticsControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = App.adminData.adminViewModel;
        }

        private void CollectionViewSource_Filter(object sender, FilterEventArgs e)
        {
            SalesModel sale = e.Item as SalesModel;


        }
    }
}
