using Prism.Mvvm;
using System;

namespace TheLiter.Core.Admin.Database
{
    public class MeasureModel : BindableBase
    {
        private int _idx;
        public int Idx
        {
            get => _idx;
            set => SetProperty(ref _idx, value);
        }

        private DateTime _measureDate;
        public DateTime MeasureDate
        {
            get => _measureDate;
            set => SetProperty(ref _measureDate, value);
        }

        private TimeSpan _totalUsageTime;
        public TimeSpan TotalUsageTime
        {
            get => _totalUsageTime;
            set => SetProperty(ref _totalUsageTime, value);
        }
    }
}
