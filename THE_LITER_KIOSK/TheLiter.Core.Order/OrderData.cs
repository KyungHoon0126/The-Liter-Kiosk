using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheLiter.Core.Order.ViewModel;

namespace TheLiter.Core.Order
{
    public class OrderData
    {
        public OrderViewModel orderViewModel = new OrderViewModel();

        public void LoadData()
        {
            orderViewModel.LoadData();
        }
    }
}
