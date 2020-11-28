using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TheLiter.Core.Admin.Database;
using TheLiter.Core.Admin.Model;
using TheLiter.Core.DBManager;
using TheLiter.Core.Network;
using TheLiter.Core.Network.Model;
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

        List<TimeSpan> TimeZoneItems { get; set; }

        #region 메뉴 별 판매 수 / 총액
        private List<SalesModel> _salesByMenuItems;
        public List<SalesModel> SalesByMenuItems
        {
            get => _salesByMenuItems;
            set
            {
                _salesByMenuItems = value;
                NotifyPropertyChanged(nameof(SalesByMenuItems));
            }
        }
        #endregion

        #region 카테고리 별 판매수 / 총액
        private List<SalesModel> _salesByCategoryItems;
        public List<SalesModel> SalesByCategoryItems
        {
            get => _salesByCategoryItems;
            set
            {
                _salesByCategoryItems = value;
                NotifyPropertyChanged(nameof(SalesByCategoryItems));
            }
        }
        #endregion

        #region 좌석 별 메뉴 별 판매 수 / 총액 & 좌석 별 카테고리 별 판매 수 / 총액
        private List<SalesModel> _salesByTableMenuAndCategoryItems;
        public List<SalesModel> SalesByTableMenuAndCategoryItems
        {
            get => _salesByTableMenuAndCategoryItems;
            set
            {
                _salesByTableMenuAndCategoryItems = value;
                NotifyPropertyChanged(nameof(SalesByTableMenuAndCategoryItems));
            }
        }
        #endregion

        #region 일별 총 매출액
        private List<SalesModel> _salesByDailyItems;
        public List<SalesModel> SalesByDailyItems
        {
            get => _salesByDailyItems;
            set
            {
                _salesByDailyItems = value;
                NotifyPropertyChanged(nameof(SalesByDailyItems));
            }
        }
        #endregion

        #region 시간대 별 총 매출액
        private List<SalesModel> _salesByTimeItems;
        public List<SalesModel> SalesByTimeItems
        {
            get => _salesByTimeItems;
            set
            {
                _salesByTimeItems = value;
                NotifyPropertyChanged(nameof(SalesByTimeItems));
            }
        }
        #endregion

        #region 회원별 주문 메뉴 / 총 매출
        private List<SalesModel> _salesByMemberItems;
        public List<SalesModel> SalesByMemberItems
        {
            get => _salesByMemberItems;
            set
            {
                _salesByMemberItems = value;
                NotifyPropertyChanged(nameof(SalesByMemberItems));
            }
        }
        #endregion

        public string MemberId { get; set; }

        private string _transMsg;
        public string TransMsg
        {
            get => _transMsg;
            set
            {
                _transMsg = value;
                NotifyPropertyChanged(nameof(TransMsg));
            }
        }

        private bool _isGroupMsg;
        public bool IsGroupMsg
        {
            get => _isGroupMsg;
            set
            {
                _isGroupMsg = value;
                NotifyPropertyChanged(nameof(IsGroupMsg));
            }
        }
        #endregion

        #region Constructor
        public AdminViewModel()
        {
            InitVariables();
            InitCommands();
            LoadCbSalesFilter();
            LoadTimeZoneItems();
        }
        #endregion

        #region Commands
        public ICommand ExportCommand { get; set; }
        #endregion

        #region Init
        private void InitVariables()
        {
            CbSaleFilter = new List<string>();
            
            SalesItems = new List<SalesModel>();
            SalesByMenuItems = new List<SalesModel>();
            SalesByCategoryItems = new List<SalesModel>();
            SalesByTableMenuAndCategoryItems = new List<SalesModel>();
            SalesByDailyItems = new List<SalesModel>();
            SalesByMemberItems = new List<SalesModel>();
            SalesByTimeItems = new List<SalesModel>();

            TimeZoneItems = new List<TimeSpan>();

            Formatter = value => ConvertPriceToString(value);
        }

        private void InitCommands()
        {
            ExportCommand = new DelegateCommand<string>(OnExport);
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
            CbSaleFilter.Add("▶ 회원별 주문 메뉴 / 총 매출액");
        }

        private void LoadTimeZoneItems()
        {
            TimeZoneItems.Add(new TimeSpan(24, 00, 00));
            TimeZoneItems.Add(new TimeSpan(01, 00, 00));
            TimeZoneItems.Add(new TimeSpan(02, 00, 00));
            TimeZoneItems.Add(new TimeSpan(03, 00, 00));
            TimeZoneItems.Add(new TimeSpan(04, 00, 00));
            TimeZoneItems.Add(new TimeSpan(05, 00, 00));
            TimeZoneItems.Add(new TimeSpan(06, 00, 00));
            TimeZoneItems.Add(new TimeSpan(07, 00, 00));
            TimeZoneItems.Add(new TimeSpan(08, 00, 00));
            TimeZoneItems.Add(new TimeSpan(09, 00, 00));
            TimeZoneItems.Add(new TimeSpan(10, 00, 00));
            TimeZoneItems.Add(new TimeSpan(11, 00, 00));
            TimeZoneItems.Add(new TimeSpan(13, 00, 00));
            TimeZoneItems.Add(new TimeSpan(14, 00, 00));
            TimeZoneItems.Add(new TimeSpan(16, 00, 00));
            TimeZoneItems.Add(new TimeSpan(18, 00, 00));
            TimeZoneItems.Add(new TimeSpan(20, 00, 00));
            TimeZoneItems.Add(new TimeSpan(21, 00, 00));
            TimeZoneItems.Add(new TimeSpan(22, 00, 00));
            TimeZoneItems.Add(new TimeSpan(23, 00, 00));
        }
        #endregion

        #region Command Method
        private void OnExport(string path)
        {
            if (path != null)
            {
                string csvFile = "주문 번호, 카테고리, 메뉴, 수량, 결제 시간, 결제 수단, 테이블 번호, 회원 아이디, 할인 금액, 순수 가격 총액" + "\n";

                for (int i = 0; i < SalesItems.Count; i++)
                {
                    csvFile += ConvertToSalesToCsv(SalesItems[i]);
                    csvFile += "\n";
                }

                if (File.Exists(path))
                {
                    try
                    {
                        File.Delete(path);
                    }
                    catch
                    {
                        return;
                    }
                }

                FileStream fs = File.Create(path);
                var str = Encoding.Default.GetBytes(csvFile);
                fs.Write(str, 0, str.Count());
                fs.Close();
            }
        }

        private string ConvertToSalesToCsv(SalesModel sale)
        {
            string retval = "";

            try
            {
                retval += sale.ReceiptIdx + ",";
                retval += sale.Category + ",";
                retval += sale.Name + ",";
                retval += sale.Count + ",";
                retval += sale.PayTime + ",";
                retval += sale.PayType + ",";
                retval += sale.TableIdx + ",";
                retval += sale.MemberId + ",";
                retval += sale.DiscountTotalPrice + ",";
                retval += sale.TotalPrice + ",";
            }
            catch (Exception e)
            {
                Debug.WriteLine("CONVERT TO SALES TO CSV ERROR : " + e.Message);
            }

            return retval;
        }
        #endregion

        #region Chart
        public void LoadSalesByMenuChartData()
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

        public void LoadSalesByCategoryChartData()
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

        internal void SyncSaleItems()
        {
            SalesByMenuItems.Clear();
            SalesByCategoryItems.Clear();
            SalesByTableMenuAndCategoryItems.Clear();
            SalesByDailyItems.Clear();
            SalesByMemberItems.Clear();
            SalesByTimeItems.Clear();

            LoadSalesByMenuChartData();
            LoadSalesByCategoryChartData();
            
            SetSaleItems();
        }

        public void SetSaleItems()
        {
            SetSalesByMenuItems();
            SetSalesByCategoryItems();
            SetSalesByTableMenuAndCategoryItems();
            SetSalesByDailyItems();
            SetSalesByMemberItems();
            SetSalesByTimeItems();
        }

        private void SetSalesByMenuItems()
        {
            var menuNames = SalesItems.Select(x => x.Name).ToList();
            var tempMenuNames = new List<string>(menuNames);

            for (int i = 0; i < tempMenuNames.Count; i++)
            {
                int cnt = 0;

                for (int j = 0; j < menuNames.Count; j++)
                {
                    if (tempMenuNames[i] == menuNames[j])
                    {
                        cnt++;
                    }

                    if (cnt == 2)
                    {
                        menuNames.Remove(tempMenuNames[i]);
                        cnt = 0;
                    }
                }
            }

            for (int i = 0; i < menuNames.Count; i++)
            {
                var specificMenuItems = SalesItems.Where(x => x.Name == menuNames[i]).ToList();
                var salesModel = new SalesModel();

                for (int j = 0; j < specificMenuItems.Count; j++)
                {
                    salesModel.Name = specificMenuItems[j].Name;
                    salesModel.Count += specificMenuItems[j].Count;
                    salesModel.TotalPrice += specificMenuItems[j].TotalPrice;
                    salesModel.DiscountTotalPrice += specificMenuItems[j].DiscountTotalPrice;
                }

                if (salesModel.Count > 0)
                {
                    SalesByMenuItems.Add(salesModel);
                }
            }
        }

        private void SetSalesByCategoryItems()
        {
            List<string> menuCategories = CategoryModel.EnumItems; 

            for (int i = 0; i < menuCategories.Count; i++)
            {
                var specificMenuItems = SalesItems.Where(x => x.Category == menuCategories[i]).ToList();
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
                    SalesByCategoryItems.Add(salesModel);
                }
            }
        }

        private void SetSalesByTableMenuAndCategoryItems()
        {
            for (int i = 1; i <= 9; i++)
            {
                var specificMenuItems = SalesItems.Where(x => x.TableIdx == i).ToList();
                var salesModel = new SalesModel();

                for (int j = 0; j < specificMenuItems.Count; j++)
                {
                    salesModel.TableIdx = i;
                    salesModel.Name = specificMenuItems[j].Name;
                    salesModel.Category = specificMenuItems[j].Category;
                    salesModel.Count += specificMenuItems[j].Count;
                    salesModel.TotalPrice += specificMenuItems[j].TotalPrice;
                    salesModel.DiscountTotalPrice += specificMenuItems[j].DiscountTotalPrice;
                }

                if (salesModel.Count > 0)
                {
                    SalesByTableMenuAndCategoryItems.Add(salesModel);
                }
            }
        }

        private void SetSalesByDailyItems()
        {
            var saleDateItems = new List<string>();

            for (int i = 0; i < SalesItems.Count; i++)
            {
                string saleDateItem = SalesItems[i].PayTime.ToString("yyyy-MM-dd");
                if (saleDateItems.Where(x => x == saleDateItem).ToList().Count == 0)
                {
                    saleDateItems.Add(saleDateItem);
                }
            }

            for (int i = 0; i < saleDateItems.Count; i++)
            {
                var specificMenuItems = SalesItems.Where(x => x.PayTime.ToString("yyyy-MM-dd") == saleDateItems[i]).ToList();
                var salesModel = new SalesModel();

                for (int j = 0; j < specificMenuItems.Count; j++)
                {
                    salesModel.Name = specificMenuItems[j].Name;
                    salesModel.PayTime = specificMenuItems[j].PayTime;
                    salesModel.Count += specificMenuItems[j].Count;
                    salesModel.TotalPrice += specificMenuItems[j].TotalPrice;
                    salesModel.DiscountTotalPrice += specificMenuItems[j].DiscountTotalPrice;
                }

                if (salesModel.Count > 0)
                {
                    SalesByDailyItems.Add(salesModel);
                }
            }
        }

        private void SetSalesByTimeItems()
        {
            for (int i = 0; i < TimeZoneItems.Count; i++)
            {
                // var specificMenuItems = SalesItems.Where(x => TimeSpan.Compare(x.PayTime, TimeZoneItems[i]) x.PayTime >= TimeZoneItems[i] && x <= TimeZoneItems[i]);
                
            }

            // 24:00 ~ 01:00
            // 01:00 ~ 02:00
            // 02:00 ~ 03:00
            // 03:00 ~ 04:00
            // 04:00 ~ 05:00
            // 05:00 ~ 06:00
            // 06:00 ~ 07:00
            // 07:00 ~ 08:00
            // 08:00 ~ 09:00
            // 09:00 ~ 10:00
            // 10:00 ~ 11:00
            // 11:00 ~ 12:00
            // 13:00 ~ 14:00
            // 14:00 ~ 15:00
            // 16:00 ~ 17:00
            // 18:00 ~ 19:00
            // 20:00 ~ 21:00
            // 21:00 ~ 22:00
            // 22:00 ~ 23:00
            // 23:00 ~ 24:00
        }

        private void SetSalesByMemberItems()
        {
            var saleMemberItems = new List<string>();

            for (int i = 0; i < SalesItems.Count; i++)
            {
                string saleMemberItem = SalesItems[i].MemberId;
                if (saleMemberItems.Where(x => x == saleMemberItem).Count() == 0)
                {
                    saleMemberItems.Add(saleMemberItem);
                }
            }
            
            List<string> menuNames = CategoryModel.EnumItems;

            for (int i = 0; i < saleMemberItems.Count; i++)
            {
                for (int j = 0; j < menuNames.Count; j++)
                {
                    var specificMenuItems = SalesItems.Where(x => x.MemberId == saleMemberItems[i] && x.Category == menuNames[j]).ToList();
                    var salesModel = new SalesModel();

                    for (int k = 0; k < specificMenuItems.Count; k++)
                    {
                        salesModel.MemberId = specificMenuItems[k].MemberId;
                        salesModel.Name = specificMenuItems[k].Name;
                        salesModel.Count += specificMenuItems[k].Count;
                        salesModel.TotalPrice += specificMenuItems[k].TotalPrice;
                        salesModel.DiscountTotalPrice += specificMenuItems[k].DiscountTotalPrice;
                    }

                    if (salesModel.Count > 0)
                    {
                        SalesByMemberItems.Add(salesModel);
                    }
                }
            }
        }

        public string GetMsgArgs()
        {
            if (TransMsg != null)
            {
                JObject jObject = new JObject();
                jObject.Add("MSGType", (int)EMessageType.NORMAL_MESSAGE);
                jObject.Add("Id", MemberId);
                jObject.Add("Content", TransMsg);
                jObject.Add("ShopName", "");
                jObject.Add("OrderNumber", "");
                jObject.Add("Group", IsGroupMsg ? true : false);
                jObject.Add("Menus", "");
                return JsonConvert.SerializeObject(jObject);
            }
            else
            {
                return null;
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
