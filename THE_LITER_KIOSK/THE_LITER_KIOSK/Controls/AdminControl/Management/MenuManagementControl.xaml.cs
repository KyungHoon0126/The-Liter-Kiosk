using System.Windows;
using System.Windows.Controls;
using TheLiter.Core.Order.Model;

namespace THE_LITER_KIOSK.Controls.AdminControl
{
    /// <summary>
    /// Interaction logic for MenuManagementControl.xaml
    /// </summary>
    public partial class MenuManagementControl : UserControl
    {
        public delegate void OnLoadMenuSettingWindowHandler(object sender, RoutedEventArgs e);
        public event OnLoadMenuSettingWindowHandler LoadMenuSettingWindow;

        public MenuManagementControl()
        {
            InitializeComponent();
            Loaded += MenuManagementControl_Loaded;
        }

        private void MenuManagementControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = App.orderData.orderViewModel;
        }

        private void lvMenuList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadMenuSettingWindow?.Invoke(this, e);
        }
    }
}
