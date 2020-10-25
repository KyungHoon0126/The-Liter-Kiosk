using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace TheLiter.Core.Admin.ViewModel
{
    public class AdminViewModel
    {
        #region Properties
        public SeriesCollection SeriesCollection { get; set; }
        #endregion

        public AdminViewModel()
        {

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
