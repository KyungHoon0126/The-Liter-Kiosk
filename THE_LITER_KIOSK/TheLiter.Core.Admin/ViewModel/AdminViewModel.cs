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
using System.Windows;
using TheLiter.Core.Admin.Database;
using TheLiter.Core.DBManager;
using TheLiter.Core.Order.Model;
using SalesModel = TheLiter.Core.Admin.Model.SalesModel;

namespace TheLiter.Core.Admin.ViewModel
{
    public class AdminViewModel : MySqlDBConnectionManager, INotifyPropertyChanged
    {
        private DBManager<MeasureModel> measureDBManager = new DBManager<MeasureModel>();
        private DBManager<SalesModel> salesDBManager = new DBManager<SalesModel>();

        public event PropertyChangedEventHandler PropertyChanged;

        #region Properties
        delegate Tuple<TimeSpan, int> ReturnEmptyValueDelegate();

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

        public Func<double, string> Formatter { get; set; }
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

        private List<string> _cbSaleFilter;
        public List<string> CbSaleFilter
        {
            get => _cbSaleFilter;
            set
            {
                _cbSaleFilter = value;
                NotifyPropertyChanged(nameof(CbSaleFilter));
            }
        }

        private int _totalSales;
        public int TotalSales
        {
            get => _totalSales;
            set
            {
                _totalSales = value;
                NotifyPropertyChanged(nameof(TotalSales));
            }
        }

        private int _totalNetSales;
        public int TotalNetSales
        {
            get => _totalNetSales;
            set
            {
                _totalNetSales = value;
                NotifyPropertyChanged(nameof(TotalNetSales));
            }
        }

        private int _totalDiscountAmount;
        public int TotalDiscountAmount
        {
            get => _totalDiscountAmount;
            set
            {
                _totalDiscountAmount = value;
                NotifyPropertyChanged(nameof(TotalDiscountAmount));
            }
        }

        #region 메뉴별
        private List<SalesModel> _salesByMenus;
        public List<SalesModel> SalesByMenus
        {
            get => _salesByMenus;
            set
            {
                _salesByMenus = value;
                NotifyPropertyChanged(nameof(SalesByMenus));
            }
        }
        #endregion

        #region 카테고리별
        private List<SalesModel> _salesByCategories;
        public List<SalesModel> SalesByCategories
        {
            get => _salesByCategories;
            set
            {
                 _salesByCategories = value;
                NotifyPropertyChanged(nameof(SalesByCategories));
            }
        }
        #endregion

        #region 좌석별
        private List<SalesModel> _salesByTableMenus;
        public List<SalesModel> SalesByTableMenus
        {
            get => _salesByTableMenus;
            set
            {
                _salesByTableMenus = value;
                NotifyPropertyChanged(nameof(SalesByTableMenus));
            }
        }

        private List<SalesModel> _salesByTableCategories;
        public List<SalesModel> SalesByTableCategories
        {
            get => _salesByTableCategories;
            set
            {
                _salesByTableCategories = value;
                NotifyPropertyChanged(nameof(SalesByTableCategories));
            }
        }
        #endregion

        #region 일(하루)별
        private List<SalesModel> _salesByDaily;
        public List<SalesModel> SalesByDaily
        {
            get => _salesByDaily;
            set
            {
                _salesByDaily = value;
                NotifyPropertyChanged(nameof(SalesByDaily));
            }
        }

        private List<SalesModel> _salesByTime;
        public List<SalesModel> SalesByTime
        {
            get => _salesByTime;
            set
            {
                _salesByTime = value;
                NotifyPropertyChanged(nameof(SalesByTime));
            }
        }
        #endregion

        #region 회원별
        private List<SalesModel> _salesByMembers;
        public List<SalesModel> SalesByMembers
        {
            get => _salesByMembers;
            set
            {
                _salesByMembers = value;
                NotifyPropertyChanged(nameof(SalesByMembers));
            }
        }
        #endregion
        #endregion

        #region Constructor
        public AdminViewModel()
        {
            InitVariables();
            LoadCbSalesFilter();
        }
        #endregion

        #region Init
        private void InitVariables()
        {
            SalesItems = new List<SalesModel>();
            CbSaleFilter = new List<string>();

            SalesByMenus = new List<SalesModel>();
            SalesByCategories = new List<SalesModel>();
            SalesByTableMenus = new List<SalesModel>();
            SalesByTableCategories = new List<SalesModel>();
            SalesByDaily = new List<SalesModel>();
            SalesByTime = new List<SalesModel>();
            SalesByMembers = new List<SalesModel>();
            
            Formatter = value => ConvertPriceToString(value);
        }

        private void LoadCbSalesFilter()
        {
            CbSaleFilter.Add("▶ 전체 정보");

            CbSaleFilter.Add("▶ 메뉴 별 판매 수 / 총액");
            
            CbSaleFilter.Add("▶ 카테고리 별 판매 수 / 총액");
            
            CbSaleFilter.Add("▶ 좌석 별 메뉴 별 판매 수 / 총액");
            CbSaleFilter.Add("▶ 좌석 별 카테고리 별 판매 수 / 총액");
            
            CbSaleFilter.Add("▶ 일별 총 매출액");
            CbSaleFilter.Add("▶ 시간대 별 총 매출액");
            
            CbSaleFilter.Add("▶ 회원별 총 매출액");
            // 회원이 주문한 총 메뉴 표시
        }
        #endregion

        #region Chart
        public void LoadSalesByMenus()
        {
            SyncGetAllSalesInformation();

            SalesByMenuSeriesCollection = new SeriesCollection();
            for (int i = 0; i < SalesItems.Count; i++)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    SalesByMenuSeriesCollection.Add(new PieSeries()
                    {
                        // Title = SalesItems[i].Name,
                        Title = ConvertPriceToString(Convert.ToDouble(SalesItems[i].TotalPrice)),
                        Values = new ChartValues<double> { Convert.ToDouble(SalesItems[i].DiscountTotalPrice) },
                        DataLabels = true,
                        LabelPoint = PointLabel => string.Format(SalesItems[i].Name)
                    });
                });                
            }
        }

        public void LoadSalesByCategories()
        {
            SyncGetAllSalesInformation();

            SalesByCategorySeriesCollection = new SeriesCollection();
            for (int i = 0; i < CategoryModel.EnumItems.Count; i++)
            {
                var menuByCategoryItems = SalesItems.Where(x => x.Category == CategoryModel.EnumItems[i].ToString()).ToList();
                double menuByCategoryTotalPrice = 0;

                menuByCategoryItems.ForEach(x =>
                {
                    menuByCategoryTotalPrice += Convert.ToDouble(x.DiscountTotalPrice);
                });

                Application.Current.Dispatcher.Invoke(() =>
                {
                    SalesByCategorySeriesCollection.Add(new PieSeries()
                    {
                        Title = CategoryModel.EnumItems[i].ToString(),
                        Values = new ChartValues<ObservableValue> { new ObservableValue(menuByCategoryTotalPrice) },
                        DataLabels = true
                    });
                });
            }
        }
        #endregion

        private void SyncGetAllSalesInformation()
        {
            if (SalesItems.Count > 0)
            {
                SalesItems.Clear();
            }

            GetAllSalesInformation();
        }

        public void SetTotalAndNetSales(string paymentType)
        {
            TotalSales = 0;
            TotalNetSales = 0;
            TotalDiscountAmount = 0;

            SalesItems.Where(x => x.PayType == paymentType).ToList().ForEach(x =>
            {
                TotalSales += x.TotalPrice;
                TotalNetSales += x.DiscountTotalPrice;
                TotalDiscountAmount += x.DiscountAmount;
            });
        }
        
        public void SetSaleItems()
        {
            //SalesByMenus = new List<SalesModel>(); // 메뉴 별 판매 수 / 총액
            //SalesByCategories = new List<SalesModel>(); // 카테고리 별 판매수 / 총액
            //SalesByTableMenus = new List<SalesModel>(); // 좌석 별 메뉴 별 판매 수 / 총액
            //SalesByTableCategories = new List<SalesModel>(); // 좌석 별 카테고리 별 판매 수 / 총액
            //SalesByDaily = new List<SalesModel>(); // 일별 총 매출액
            //SalesByTime = new List<SalesModel>(); // 시간대 별 총 매출액
            //SalesByMembers = new List<SalesModel>(); // 회원별 총 매출액

            SalesByCategories.Clear();

            List<string> menuNames = CategoryModel.EnumItems;

            for (int i = 0; i < menuNames.Count; i++)
            {
                var specificMenuItems = SalesItems.Where(x => x.Category == menuNames[i]).ToList();
                var salesModel = new SalesModel();

                for (int j = 0; j < specificMenuItems.Count; j++)
                {
                    salesModel.Category = specificMenuItems[j].Category;
                    salesModel.Count += specificMenuItems[j].Count;
                    salesModel.TotalPrice += specificMenuItems[j].TotalPrice;
                    salesModel.DiscountTotalPrice += specificMenuItems[j].DiscountTotalPrice;
                }

                if (salesModel.Count > 0)
                {
                    SalesByCategories.Add(salesModel);
                }
            }
        }

        #region DataBase
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
            ReturnEmptyValueDelegate method = () => { return new Tuple<TimeSpan, int>(TimeSpan.Parse("00:00:00"), -1); };

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
        
        private string ConvertPriceToString(double value)
        {
            return string.Format("{0:#,0원}", value);
        }

        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
