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
using TheLiter.Core.Admin.Model;
using TheLiter.Core.DBManager;
using TheLiter.Core.Order.Model;

namespace TheLiter.Core.Admin.ViewModel
{
    public class AdminViewModel : MySqlDBConnectionManager, INotifyPropertyChanged
    {
        private DBManager<MeasureModel> measureDBManager = new DBManager<MeasureModel>();
        private DBManager<SalesModel> salesDBManager = new DBManager<SalesModel>();

        public event PropertyChangedEventHandler PropertyChanged;

        #region Properties
        delegate Tuple<TimeSpan, int> ReturnEmptyDelegate();

        private string _operationTimeDesc;
        public string OperationTimeDesc
        {
            get => _operationTimeDesc;
            set
            {
                _operationTimeDesc = value;
                NotifyPropertyChanged(nameof(OperationTimeDesc));
            }
        }

        private DateTime _operationTime;
        public DateTime OperationTime
        {
            get => _operationTime;
            set
            {
                _operationTime = value;
                NotifyPropertyChanged(nameof(OperationTime));
            }
        }

        private List<SalesModel> _salesItems;
        public List<SalesModel> SalesItems
        {
            get => _salesItems;
            set
            {
                _salesItems = value;
                NotifyPropertyChanged(nameof(SalesItems));
            }
        }

        private SeriesCollection _salesByCategorySeriesCollection;
        public SeriesCollection SalesByCategorySeriesCollection 
        {
            get => _salesByCategorySeriesCollection;
            set
            {
                _salesByCategorySeriesCollection = value;
                NotifyPropertyChanged(nameof(SalesByCategorySeriesCollection));
            }
        }

        private SeriesCollection _salesByMenuSeriesCollection;
        public SeriesCollection SalesByMenuSeriesCollection
        {
            get => _salesByMenuSeriesCollection;
            set
            {
                _salesByMenuSeriesCollection = value;
                NotifyPropertyChanged(nameof(SalesByMenuSeriesCollection));
            }
        }
        #endregion

        #region Constructor
        public AdminViewModel()
        {
            SalesItems = new List<SalesModel>();
        }
        #endregion

        #region Chart
        public void LoadSalesByMenuDatas()
        {
            SyncGetAllSalesInformation();

            SalesByMenuSeriesCollection = new SeriesCollection();
            for (int i = 0; i < SalesItems.Count; i++)
            {
                SalesByMenuSeriesCollection.Add(new PieSeries()
                {
                    Title = SalesItems[i].Name,
                    Values = new ChartValues<ObservableValue> { new ObservableValue(Convert.ToDouble(SalesItems[i].Price)) },
                    DataLabels = false
                });
            }
        }

        public void LoadSalesByCategory()
        {
            SyncGetAllSalesInformation();

            SalesByCategorySeriesCollection = new SeriesCollection();
            for (int i = 0; i < CategoryModel.EnumItems.Count; i++)
            {
                var menuByCategoryItems = SalesItems.Where(x => x.Category == CategoryModel.EnumItems[i].ToString()).ToList();
                double menuByCategoryTotalPrice = 0;

                menuByCategoryItems.ForEach(x =>
                {
                    menuByCategoryTotalPrice += Convert.ToDouble(x.Price);
                });

                SalesByCategorySeriesCollection.Add(new PieSeries()
                {
                    Title = CategoryModel.EnumItems[i].ToString(),
                    Values = new ChartValues<ObservableValue> { new ObservableValue(menuByCategoryTotalPrice) },
                    DataLabels = true
                });
            }
        }

        private void SyncGetAllSalesInformation()
        {
            if (SalesItems.Count > 0)
            {
                SalesItems.Clear();
            }

            GetAllSalesInformation();
        }
        #endregion

        #region Program OperationTime 
        public async void SynchronizationOpertaionTime()
        {
            var measureItem = await GetProgramTotalUsageTime();
            if (measureItem.ToString() != "00:00:00")
            {
                OperationTime = OperationTime.Add(measureItem.Item1);
            }
        }

        public void IncreaseOperationTime()
        {
            OperationTime = OperationTime.AddSeconds(1);
        }

        public async Task<Tuple<TimeSpan, int>> GetProgramTotalUsageTime()
        {
            ReturnEmptyDelegate method = () => { return new Tuple<TimeSpan, int>(TimeSpan.Parse("00:00:00"), -1); };

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

                var todayMeasureItem =  measureItems.Where(x => x.MeasureDate == DateTime.Today).FirstOrDefault();
                
                if (todayMeasureItem != null)
                    return new Tuple<TimeSpan, int>(todayMeasureItem.TotalUsageTime, todayMeasureItem.Idx);
                else
                    return method();
            }
            catch (Exception e)
            {
                Debug.WriteLine("GET PROGRAM TOTAL USAGE TIME ERROR : " + e.Message);
            }

            return method();
        }

        public async void SaveProgramTotalUsageTime()
        {
            try
            {
                using (IDbConnection db = GetConnection())
                {
                    db.Open();

                    var measureModel = new MeasureModel();
                    var measure = await GetProgramTotalUsageTime();
                    measureModel.TotalUsageTime = measure.Item1;
                    
                    if (measureModel.TotalUsageTime.ToString() != "00:00:00" && measureModel.Idx != -1)
                    {
                        measureModel.TotalUsageTime = TimeSpan.Parse(OperationTimeDesc);

                        string updateSql = $@"
UPDATE
    measure_tb
SET
    totalUsageTime = '{measureModel.TotalUsageTime}'
WHERE
    idx = '{measure.Item2}'
;";
                        if (await measureDBManager.UpdateAsync(db, updateSql, measureModel) == 1)
                            Debug.WriteLine("SUCCESS UPDATE PROGRAM TOTAL USAGE TIME");
                        else
                            Debug.WriteLine("FAILURE UPDATE PROGRAM TOTAL USAGE TIME");
                    }
                    else
                    {
                        measureModel.TotalUsageTime += TimeSpan.Parse(OperationTimeDesc);

                        string insertSql = @"
INSERT INTO measure_tb(
    totalUsageTime
)
VALUES(
    @totalUsageTime
);";
                        if (await measureDBManager.InsertAsync(db, insertSql, measureModel) == 1)
                            Debug.WriteLine("SUCCESS SAVE PROGRAM TOTAL USAGE TIME");
                        else
                            Debug.WriteLine("FAILURE SAVE PROGRAM TOTAL USAGE TIME");
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("SAVE PROGRAM TOTAL USAGE TIME ERROR : " + e.Message);
            }
        }

        public async void GetAllSalesInformation()
        {
            try
            {
                List<SalesModel> sales = new List<SalesModel>();

                using (var db = GetConnection())
                {
                    string selectSql = @"
SELECT
    *
FROM
    sales_tb
";
                    sales = await salesDBManager.GetListAsync(db, selectSql, "");
                    if (sales != null) SalesItems = sales;
                }
            }
            catch (Exception e)
            {
                Debug.Write("GET ALL SALES INFORMATION : " + e.Message);
            }
        }
        #endregion

        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
