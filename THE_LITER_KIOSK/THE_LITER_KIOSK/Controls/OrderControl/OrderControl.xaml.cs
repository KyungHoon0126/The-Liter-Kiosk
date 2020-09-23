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
using TheLiter.Core.Order.Model;

namespace THE_LITER_KIOSK.Controls.OrderControl
{
    /// <summary>
    /// Interaction logic for OrderControl.xaml
    /// </summary>
    public partial class OrderControl : UserControl
    {
        public OrderControl()
        {
            InitializeComponent();
            Loaded += OrderControl_Loaded;
        }

        private void OrderControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = App.orderData.orderViewModel;
        }

        private void lvCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilteringMenuItems(((Category)lvCategory.SelectedItem).ECategory);
        }

        private void FilteringMenuItems(ECategory category)
        {
            if (category == ECategory.ALL)
            {
                lvMenuList.ItemsSource = App.orderData.orderViewModel.MenuItems;
            }
            else
            {
                lvMenuList.ItemsSource = App.orderData.orderViewModel.MenuItems.Where(x => x.MenuCategory == category);
            }
        }
    }
}
