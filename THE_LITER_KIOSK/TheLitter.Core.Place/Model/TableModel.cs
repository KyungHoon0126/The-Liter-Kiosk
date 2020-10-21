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
            DispatcherTimer.Tick += DispatcherTimer_Tick;
        }

        public void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            RemainTime = $"사용시간이 {LeftTime}초 남았습니다.";
            LeftTime = LeftTime - 1;
            if (LeftTime < 0)
            {
                (sender as DispatcherTimer).Stop();
                RemainTime = string.Empty;
                LeftTime = 60;
            }
        }

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

        private string _remainTime;
        public string RemainTime
        {
            get => _remainTime;
            set => SetProperty(ref _remainTime, value);
        }

        public int LeftTime { get; set; } = 60;

        public DispatcherTimer DispatcherTimer = new DispatcherTimer();
    }
}
