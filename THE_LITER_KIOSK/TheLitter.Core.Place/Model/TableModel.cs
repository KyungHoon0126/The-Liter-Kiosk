using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Windows.Threading;
using TheLiter.Core.Order.Model;

namespace TheLitter.Core.Place.Model
{
    public class TableModel : BindableBase
    {
        public TableModel()
        {
            //Timer.Interval = TimeSpan.FromSeconds(1);
            //Timer.Tick += DispatcherTimer_Tick;
        }

        //private void DispatcherTimer_Tick(object sender, EventArgs e)
        //{
        //    RemainTime = $"사용시간이 {LeftTime}초 남았습니다.";
        //    LeftTime = LeftTime - 1;

        //    if (LeftTime == 0)
        //    {
        //        Timer.Stop();
        //        RemainTime = string.Empty;
        //    }
        //}

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

        public int LeftTime { get; set; } = 60;

        //private DispatcherTimer _dispatcherTimer = new DispatcherTimer();
        //public DispatcherTimer Timer
        //{
        //    get => _dispatcherTimer;
        //    set => SetProperty(ref _dispatcherTimer, value);
        //}

        public DispatcherTimer DispatcherTimer { get; set; }
    }
}
