using Microsoft.WindowsAPICodePack.Dialogs;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

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
            cbPaymentType.SelectedIndex = 0;
            cbSalesType.SelectedIndex = 0;
        }

        private void PaymentType_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((sender as ComboBox).SelectedItem as ComboBoxItem).Content.ToString().Contains("카드"))
                SetTotalAndNetSales("CARD");
            else
                SetTotalAndNetSales("CASH");
        }

        private void SetTotalAndNetSales(string paymentType)
        {
            App.adminData.adminViewModel.SetTotalAndNetSales(paymentType);
        }

        private void SalesType_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedSalesType = (sender as ComboBox).SelectedItem.ToString();
            if (IsMatchedSalesType(selectedSalesType, "▶ 전체 정보"))
            {
                App.adminData.SyncSaleItems();
                ShowDataGrid(dgSalesStat);
            }
            else if (IsMatchedSalesType(selectedSalesType, "▶ 메뉴 별 판매 수 / 총액")) 
            {
                App.adminData.SyncSaleItems();
                ShowDataGrid(dgSalesByMenu);
            }
            else if (IsMatchedSalesType(selectedSalesType, "▶ 카테고리 별 판매 수 / 총액"))
            {
                App.adminData.SyncSaleItems();
                ShowDataGrid(dgSalesByCategory);
            }
            else if (IsMatchedSalesType(selectedSalesType, "▶ 좌석 별 메뉴 별 판매 수 / 총액"))
            {
                App.adminData.SyncSaleItems();
                ShowDataGrid(dgSalesByTables);
                dtcTableIdx.Visibility = Visibility.Visible; // 테이블 번호
                dtcTableMenuName.Visibility = Visibility.Visible; // 메뉴
                dtcTableMenuCategory.Visibility = Visibility.Collapsed; // 카테고리
            }
            else if (IsMatchedSalesType(selectedSalesType, "▶ 좌석 별 카테고리 별 판매 수 / 총액"))
            {
                App.adminData.SyncSaleItems();
                ShowDataGrid(dgSalesByTables);
                dtcTableIdx.Visibility = Visibility.Visible; // 테이블 번호
                dtcTableMenuName.Visibility = Visibility.Collapsed; // 메뉴
                dtcTableMenuCategory.Visibility = Visibility.Visible; // 카테고리
            }
            else if (IsMatchedSalesType(selectedSalesType, "▶ 일별 총 매출액"))
            {
                App.adminData.SyncSaleItems();
                ShowDataGrid(dgSalesByDaily);
            }
            else if (IsMatchedSalesType(selectedSalesType, "▶ 시간대 별 총 매출액"))
            {
                App.adminData.SyncSaleItems();
                ShowDataGrid(dgSalesByTimes);
            }
            else if (IsMatchedSalesType(selectedSalesType, "▶ 회원별 주문 메뉴 / 총 매출액"))
            {
                App.adminData.SyncSaleItems();
                ShowDataGrid(dgSalesByMembers);
            }
        }

        private void ShowDataGrid(DataGrid dataGrid)
        {
            if (dataGrid != null && dataGrid is FrameworkElement element)
            {
                CollapseDataGrids();
                element.Visibility = Visibility.Visible;
            }
        }

        private void CollapseDataGrids()
        {
            foreach (FrameworkElement element in gdSales.Children)
            {
                element.Visibility = Visibility.Collapsed;
            }
        }

        private bool IsMatchedSalesType(string selectedSalesType, string salesType)
        {
            return selectedSalesType.Equals(salesType);
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            btnExport.CommandParameter = App.saveFileManager.GetFilePath();
        }
    }
}
