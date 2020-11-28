using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace THE_LITER_KIOSK.Controls.AdminControl.Management
{
    /// <summary>
    /// Interaction logic for MenuSettingWIndow.xaml
    /// </summary>
    public partial class MenuSettingWindow : Window
    {
        public MenuSettingWindow()
        {
            InitializeComponent();
            Loaded += MenuSettingWindow_Loaded;
        }

        private void MenuSettingWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = App.orderData.orderViewModel;
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void btnCloseMenuSettingWindow_Click(object sender, RoutedEventArgs e)
        {
            if (tbDiscountRate.Text == "" || tbDiscountRate.Text == " ")
            {
                this.Close();
                return;
            }

            App.orderData.orderViewModel.IsSoldOutChecked = (bool)cbSoldOut.IsChecked;
            App.orderData.orderViewModel.DisCountRate = int.Parse(tbDiscountRate.Text);
            App.orderData.orderViewModel.SaveMenuDiscountRateAndIsSoldOut();

            this.Close();
        }

        private void tbDisCountRate_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
