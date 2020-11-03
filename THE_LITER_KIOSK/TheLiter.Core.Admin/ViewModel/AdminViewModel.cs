using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TheLiter.Core.Admin.Database;
using TheLiter.Core.DBManager;

namespace TheLiter.Core.Admin.ViewModel
{
    public class AdminViewModel : MySqlDBConnectionManager, INotifyPropertyChanged
    {
        private DBManager<MeasureModel> measureDBManager = new DBManager<MeasureModel>();

        #region Properties
        private DateTime _startTime;
        public DateTime StartTime
        {
            get => _startTime;
            set
            {
                _startTime = value;
                NotifyPropertyChanged(nameof(StartTime));
            }
        }

        private TimeSpan _operationTime;
        public TimeSpan OperationTime
        {
            get => _operationTime;
            set
            {
                _operationTime = value;
                NotifyPropertyChanged(nameof(OperationTime));
            }
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
        }

        // TODO : 리펙토링
        #region Database
        private async Task<MeasureModel> GetProgramTotalUsageTime()
        {
            try
            {
                var measureItems = new List<MeasureModel>();

                using (IDbConnection db = GetConnection())
                {
                    db.Open();


                    string selectSql = @"
SELECT
    *
FROM
    measure_tb
";
                    measureItems = await measureDBManager.GetListAsync(db, selectSql, "");
                }

                var todayMeasureItem =  measureItems.Where(x => x.MeasureDate.ToString("yyyy-MM-dd") == DateTime.Now.ToString("yyyy-MM-dd")).FirstOrDefault();
                if (todayMeasureItem != null)
                    return todayMeasureItem;
                else
                    return new MeasureModel();
            }
            catch (Exception e)
            {
                Debug.WriteLine("GET PROGRAM TOTAL USAGE TIME ERROR : " + e.Message);
            }

            return new MeasureModel();
        }

        public async void SaveProgramTotalUsageTime()
        {
            try
            {
                using (IDbConnection db = GetConnection())
                {
                    db.Open();

                    var measureModel = new MeasureModel();
                    measureModel = await GetProgramTotalUsageTime();
                    
                    if (measureModel != null && measureModel.Idx != 0)
                    {
                        measureModel.TotalUsageTime += OperationTime;
                        string updateSql = $@"
UPDATE
    measure_tb
SET
    totalUsageTime = '{measureModel.TotalUsageTime}'
WHERE
    idx = '{measureModel.Idx}'
;";
                        if (await measureDBManager.UpdateAsync(db, updateSql, measureModel) == 1)
                        {
                            Debug.WriteLine("SUCCESS UPDATE PROGRAM TOTAL USAGE TIME");
                        }
                        else
                        {
                            Debug.WriteLine("FAILURE UPDATE PROGRAM TOTAL USAGE TIME");
                        }
                    }
                    else
                    {
                        measureModel.TotalUsageTime = OperationTime;
                        string insertSql = @"
INSERT INTO measure_tb(
    totalUsageTime
)
VALUES(
    @totalUsageTime
);";
                        if (await measureDBManager.InsertAsync(db, insertSql, measureModel) == 1)
                        {
                            Debug.WriteLine("SUCCESS SAVE PROGRAM TOTAL USAGE TIME");
                        }
                        else
                        {
                            Debug.WriteLine("FAILURE SAVE PROGRAM TOTAL USAGE TIME");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("SAVE PROGRAM TOTAL USAGE TIME ERROR : " + e.Message);
            }
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
