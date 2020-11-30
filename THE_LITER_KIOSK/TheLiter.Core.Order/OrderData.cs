using TheLiter.Core.Order.ViewModel;

namespace TheLiter.Core.Order
{
    public class OrderData
    {
        public OrderDBViewModel orderViewModel = new OrderDBViewModel();

        public void LoadData()
        {
            orderViewModel.LoadOrderData();
        }

        public void InitData()
        {
            orderViewModel.ClearMenuItems();
            orderViewModel.IsEnabledOrderAndClearAllMenuBtn();
        }

        public bool IsValidOrderedMenuItems()
        {
            return orderViewModel.IsValidOrderedMenuItems();
        }

        public void SetPagingMenuItems()
        {
            orderViewModel.SetPagingMenuItems();
        }
    }
}
