using TheLiter.Core.Order.ViewModel;

namespace TheLiter.Core.Order
{
    public class OrderData
    {
        public OrderViewModel orderViewModel = new OrderViewModel();

        public void LoadData()
        {
            orderViewModel.LoadOrderData();
        }

        public void InitData()
        {
            orderViewModel.ClearMenuItems();
            orderViewModel.IsEnabledOrderAndClearAllMenuBtn();
        }

        public void SetPagingMenuItems()
        {
            orderViewModel.SetPagingMenuItems();
        }
    }
}
