using System.Windows;
using THE_LITER_KIOSK.UIManager;

namespace THE_LITER_KIOSK.Controls.AdminControl
{
    /// <summary>
    /// Interaction logic for AdminControl.xaml
    /// </summary>
    public partial class AdminControl : CustomControlModel
    {
        public AdminControl()
        {
            InitializeComponent();
            Loaded += AdminControl_Loaded;
        }

        private void AdminControl_Loaded(object sender, RoutedEventArgs e)
        {
            App.adminData.LoadData();
            this.DataContext = App.adminData.adminViewModel;
        }

        private void btnReload_Click(object sender, RoutedEventArgs e)
        {
            App.adminData.SynchronizationOperationTime();
        }
    }
}
