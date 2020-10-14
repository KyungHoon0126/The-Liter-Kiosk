using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Windows.Threading;
using TheLiter.Core.Order.Model;

namespace TheLitter.Core.Place.Model
{
    public class TableModel: BindableBase
    {
        private int _tableIdx;
        public int TableIdx
        {
            get => _tableIdx;
            set => SetProperty(ref _tableIdx, value);
        }

        private int _totalPrice;
        public int TotalPrice
        {
            get => _totalPrice;
            set => SetProperty(ref _totalPrice, value);
        }

        private DateTime _payTime;
        public DateTime PayTime
        {
            get => _payTime;
            set => SetProperty(ref _payTime, value);
        }

        private List<MenuModel> _menuList;
        public List<MenuModel> MenuList
        {
            get => _menuList;
            set => SetProperty(ref _menuList, value);
        }

        private string _remainTime;
        public string RemainTime
        {
            get => _remainTime;
            set => SetProperty(ref _remainTime, value);
        }
         
    }
}
