using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Prism.Mvvm;
using System;

namespace TheLiter.Core.Admin.ViewModel
{
    public class AdminViewModel : BindableBase
    {
        #region Properties
        private DateTime _startTime;
        public DateTime StartTime
        {
            get => _startTime;
            set => SetProperty(ref _startTime, value);
        }

        private TimeSpan _operationTime;
        public TimeSpan OperationTime
        {
            get => _operationTime;
            set => SetProperty(ref _operationTime, value);
        }

        public SeriesCollection SeriesCollection { get; set; }
        #endregion

        public AdminViewModel()
        {

        }

        public void SynchronizationOperationTime()
        {
            OperationTime = DateTime.Now.Subtract(StartTime);
        }

        public void LoadChartDatas()
        {
            SeriesCollection = new SeriesCollection
            {
                new PieSeries
                {
                    Title = "블루베리 요거트",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(30) },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "자바칩 프라페",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(40) },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "S1 Soda",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(30) },
                    DataLabels = true 
                }
            };

            //adding values or series will update and animate the chart automatically
            //SeriesCollection.Add(new PieSeries());
            //SeriesCollection[0].Values.Add(5);
        }
    }
}
