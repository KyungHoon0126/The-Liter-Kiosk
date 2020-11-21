using TheLiter.Core.Admin.ViewModel;

namespace TheLiter.Core.Admin
{
    public class AdminData
    {
        public AdminViewModel adminViewModel = new AdminViewModel();

        public void LoadData()
        {
            adminViewModel.LoadSalesByMenus();
            adminViewModel.LoadSalesByCategories();
            adminViewModel.SetTotalAndNetSales("CARD");
            adminViewModel.SetSaleItems();
        }
    }
}
