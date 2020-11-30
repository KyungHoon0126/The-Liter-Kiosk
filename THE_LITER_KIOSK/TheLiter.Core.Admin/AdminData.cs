using TheLiter.Core.Admin.ViewModel;

namespace TheLiter.Core.Admin
{
    public class AdminData
    {
        public AdminViewModel adminViewModel = new AdminViewModel();

        public void LoadSalesData()
        {
            adminViewModel.GetAllSalesInformation();
        }

        public void LoadChartData()
        {
            adminViewModel.LoadSalesByCategoryChartData();
        }

        public void SetStatisticData()
        {
            adminViewModel.SetSalesStatItems();
            adminViewModel.SetTotalAndNetSales("CARD");
            adminViewModel.SetSaleItems();
        }

        public void SyncSaleItems()
        {
            adminViewModel.SyncSaleItems();
        }
    }
}
