using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using THE_LITER_KIOSK.Common;
using THE_LITER_KIOSK.DataBase.Models;
using TheLiter.Core.DBManager;
using TheLiter.Core.Order.Model;

namespace TheLiter.Core.Order.ViewModel
{
    public class OrderViewModel : MySqlDBConnectionManager, INotifyPropertyChanged
    {
        private DBManager<SalesModel> salesDBManager = new DBManager<SalesModel>();

        public event PropertyChangedEventHandler PropertyChanged;

        #region Properties
        private ObservableCollection<CategoryModel> _categoryItems;
        public ObservableCollection<CategoryModel> CategoryItems
        {
            get => _categoryItems;
            set
            {
                _categoryItems = value;
                NotifyPropertyChanged(nameof(CategoryItems));
            }
        }

        private ObservableCollection<MenuModel> _menuItems;
        public ObservableCollection<MenuModel> MenuItems
        {
            get => _menuItems;
            set
            {
                _menuItems = value;
                NotifyPropertyChanged(nameof(MenuItems));
            }
        }

        private ObservableCollection<MenuModel> _orderedMenuItems;
        public ObservableCollection<MenuModel> OrderedMenuItems
        {
            get => _orderedMenuItems;
            set 
            {
                _orderedMenuItems = value; 
                NotifyPropertyChanged(nameof(OrderedMenuItems)); 
            }
        }

        private int _orderTotalPrice = 0;
        public int OrderTotalPrice
        {
            get => _orderTotalPrice;
            set
            {
                _orderTotalPrice = value;
                NotifyPropertyChanged(nameof(OrderTotalPrice));
            }
        }

        private string _qrCode;
        public string QrCode
        {
            get => _qrCode;
            set
            {
                _qrCode = value;
                NotifyPropertyChanged(nameof(QrCode));
            }
        }

        private string _barCode;
        public string BarCode
        {
            get => _barCode;
            set
            {
                _barCode = value;
                NotifyPropertyChanged(nameof(BarCode));
            }
        }
        #endregion

        #region Constructor
        public OrderViewModel()
        {
            InitVariables();
        }
        #endregion

        #region Init
        private void InitVariables()
        {
            CategoryItems = new ObservableCollection<CategoryModel>();
            MenuItems = new ObservableCollection<MenuModel>();
            OrderedMenuItems = new ObservableCollection<MenuModel>();
        }
        #endregion

        public void LoadOrderData()
        {
            Parallel.Invoke(
                            async () =>
                            {
                                await LoadCategoryDataAsync();
                            },
                            async () =>
                            {
                                await LoadMenuDataAsync();
                            });
        }

        private async Task LoadCategoryDataAsync()
        {
            await Task.Run(() =>
            {
                #region Categories
                CategoryItems.Add(new CategoryModel()
                {
                    CategoryName = "ALL",
                    ECategory = ECategory.ALL
                });
                CategoryItems.Add(new CategoryModel()
                {
                    CategoryName = "ADE",
                    ECategory = ECategory.ADE
                });
                CategoryItems.Add(new CategoryModel()
                {
                    CategoryName = "COFFEE",
                    ECategory = ECategory.COFFEE
                });
                CategoryItems.Add(new CategoryModel()
                {
                    CategoryName = "DESERT",
                    ECategory = ECategory.DESERT
                });
                CategoryItems.Add(new CategoryModel()
                {
                    CategoryName = "LATTE",
                    ECategory = ECategory.LATTE
                });
                CategoryItems.Add(new CategoryModel()
                {
                    CategoryName = "LITERCCINO",
                    ECategory = ECategory.LITERCCINO
                });
                CategoryItems.Add(new CategoryModel()
                {
                    CategoryName = "TEA",
                    ECategory = ECategory.TEA
                });
                CategoryItems.Add(new CategoryModel()
                {
                    CategoryName = "THELITERSPECIAL",
                    ECategory = ECategory.THELITERSPECIAL
                });
                CategoryItems.Add(new CategoryModel()
                {
                    CategoryName = "YOGURS",
                    ECategory = ECategory.YOGURS
                });
                #endregion
            });
        }

        private async Task LoadMenuDataAsync()
        {
            await Task.Run(() =>
            {
                #region Menus
                #region Ade
                MenuItems.Add(new MenuModel()
                {
                    Idx = 1,
                    Name = "CitronAde",
                    ImageUrl = ComDef.Path + "/Ade/CitronAde.jpg",
                    MenuCategory = ECategory.ADE,
                    Price = 1000
                });
                MenuItems.Add(new MenuModel()
                {
                    Idx = 2,
                    Name = "GrapefruitAde",
                    ImageUrl = ComDef.Path + "/Ade/GrapefruitAde.jpg",
                    MenuCategory = ECategory.ADE,
                    Price = 1000
                });
                MenuItems.Add(new MenuModel()
                {
                    Idx = 3,
                    Name = "GreengrapeAde",
                    ImageUrl = ComDef.Path + "/Ade/GreengrapeAde.jpg",
                    MenuCategory = ECategory.ADE,
                    Price = 1000
                });
                MenuItems.Add(new MenuModel()
                {
                    Idx = 4,
                    Name = "LemonAde",
                    ImageUrl = ComDef.Path + "/Ade/LemonAde.jpg",
                    MenuCategory = ECategory.ADE,
                    Price = 1000
                });
                MenuItems.Add(new MenuModel()
                {
                    Idx = 5,
                    Name = "MojitoAde",
                    ImageUrl = ComDef.Path + "/Ade/MojitoAde.jpg",
                    MenuCategory = ECategory.ADE,
                    Price = 1000
                });
                #endregion

                #region Coffee
                MenuItems.Add(new MenuModel()
                {
                    Idx = 6,
                    Name = "Americano",
                    ImageUrl = ComDef.Path + "/Coffee/Americano.jpg",
                    MenuCategory = ECategory.ADE,
                    Price = 1000
                });
                MenuItems.Add(new MenuModel()
                {
                    Idx = 7,
                    Name = "Cafelatte",
                    ImageUrl = ComDef.Path + "/Coffee/Cafelatte.jpg",
                    MenuCategory = ECategory.COFFEE,
                    Price = 1000
                });
                MenuItems.Add(new MenuModel()
                {
                    Idx = 8,
                    Name = "CafeMocha",
                    ImageUrl = ComDef.Path + "/Coffee/CafeMocha.jpg",
                    MenuCategory = ECategory.COFFEE,
                    Price = 1000
                });
                MenuItems.Add(new MenuModel()
                {
                    Idx = 9,
                    Name = "Cappuccino",
                    ImageUrl = ComDef.Path + "/Coffee/Cappuccino.jpg",
                    MenuCategory = ECategory.COFFEE,
                    Price = 1000
                });
                MenuItems.Add(new MenuModel()
                {
                    Idx = 10,
                    Name = "CaramelMacchiato",
                    ImageUrl = ComDef.Path + "/Coffee/CaramelMacchiato.jpg",
                    MenuCategory = ECategory.COFFEE,
                    Price = 1000
                });
                MenuItems.Add(new MenuModel()
                {
                    Idx = 11,
                    Name = "ColdBrew",
                    ImageUrl = ComDef.Path + "/Coffee/ColdBrew.jpg",
                    MenuCategory = ECategory.COFFEE,
                    Price = 1000
                });
                MenuItems.Add(new MenuModel()
                {
                    Idx = 12,
                    Name = "ColdBrewLatte",
                    ImageUrl = ComDef.Path + "/Coffee/ColdBrewLatte.jpg",
                    MenuCategory = ECategory.COFFEE,
                    Price = 1000
                });
                MenuItems.Add(new MenuModel()
                {
                    Idx = 13,
                    Name = "CondensedMilkLatte",
                    ImageUrl = ComDef.Path + "/Coffee/CondensedMilkLatte.jpg",
                    MenuCategory = ECategory.COFFEE,
                    Price = 1000
                });
                MenuItems.Add(new MenuModel()
                {
                    Idx = 14,
                    Name = "HazelnutLatte",
                    ImageUrl = ComDef.Path + "/Coffee/HazelnutLatte.jpg",
                    MenuCategory = ECategory.COFFEE,
                    Price = 1000
                });
                MenuItems.Add(new MenuModel()
                {
                    Idx = 15,
                    Name = "TiramisuLatte",
                    ImageUrl = ComDef.Path + "/Coffee/TiramisuLatte.jpg",
                    MenuCategory = ECategory.COFFEE,
                    Price = 1000
                });
                MenuItems.Add(new MenuModel()
                {
                    Idx = 16,
                    Name = "vanillaLatte",
                    ImageUrl = ComDef.Path + "/Coffee/vanillaLatte.jpg",
                    MenuCategory = ECategory.COFFEE,
                    Price = 1000
                });
                #endregion

                #region Desert
                MenuItems.Add(new MenuModel()
                {
                    Idx = 17,
                    Name = "Cookie",
                    ImageUrl = ComDef.Path + "/Desert/Cookie.jpg",
                    MenuCategory = ECategory.DESERT,
                    Price = 1000
                });
                MenuItems.Add(new MenuModel()
                {
                    Idx = 18,
                    Name = "CurstardStick",
                    ImageUrl = ComDef.Path + "/Desert/CurstardStick.jpg",
                    MenuCategory = ECategory.DESERT,
                    Price = 1000
                });
                MenuItems.Add(new MenuModel()
                {
                    Idx = 19,
                    Name = "HeartPie",
                    ImageUrl = ComDef.Path + "/Desert/HeartPie.jpg",
                    MenuCategory = ECategory.DESERT,
                    Price = 1000
                });
                MenuItems.Add(new MenuModel()
                {
                    Idx = 20,
                    Name = "MuffinSandwich",
                    ImageUrl = ComDef.Path + "/Desert/MuffinSandwich.jpg",
                    MenuCategory = ECategory.DESERT,
                    Price = 1000
                });
                MenuItems.Add(new MenuModel()
                {
                    Idx = 21,
                    Name = "Waffle",
                    ImageUrl = ComDef.Path + "/Desert/Waffle.jpg",
                    MenuCategory = ECategory.DESERT,
                    Price = 1000
                });
                #endregion

                #region Latte
                MenuItems.Add(new MenuModel()
                {
                    Idx = 22,
                    Name = "DeepChocoLatte",
                    ImageUrl = ComDef.Path + "/Latte/DeepChocoLatte.jpg",
                    MenuCategory = ECategory.LATTE,
                    Price = 1000
                });
                MenuItems.Add(new MenuModel()
                {
                    Idx = 23,
                    Name = "GreenteaLatte",
                    ImageUrl = ComDef.Path + "/Latte/GreenteaLatte.jpg",
                    MenuCategory = ECategory.LATTE,
                    Price = 1000
                });
                MenuItems.Add(new MenuModel()
                {
                    Idx = 24,
                    Name = "MilkTea",
                    ImageUrl = ComDef.Path + "/Latte/MilkTea.jpg",
                    MenuCategory = ECategory.LATTE,
                    Price = 1000
                });
                MenuItems.Add(new MenuModel()
                {
                    Idx = 25,
                    Name = "MintChocoLatte",
                    ImageUrl = ComDef.Path + "/Latte/MintChocoLatte.jpg",
                    MenuCategory = ECategory.LATTE,
                    Price = 1000
                });
                MenuItems.Add(new MenuModel()
                {
                    Idx = 26,
                    Name = "PurplesweetpotatoLatte",
                    ImageUrl = ComDef.Path + "/Latte/PurplesweetpotatoLatte.jpg",
                    MenuCategory = ECategory.LATTE,
                    Price = 1000
                });
                #endregion

                #region Literccino
                MenuItems.Add(new MenuModel()
                {
                    Idx = 27,
                    Name = "CookieCreamLiterccino",
                    ImageUrl = ComDef.Path + "/Literccino/CookieCreamLiterccino.jpg",
                    MenuCategory = ECategory.LITERCCINO,
                    Price = 1000
                });
                MenuItems.Add(new MenuModel()
                {
                    Idx = 28,
                    Name = "DoublechocoLiterccino",
                    ImageUrl = ComDef.Path + "/Literccino/DoublechocoLiterccino.jpg",
                    MenuCategory = ECategory.LITERCCINO,
                    Price = 1000
                });
                MenuItems.Add(new MenuModel()
                {
                    Idx = 29,
                    Name = "GreenteaLiterccino",
                    ImageUrl = ComDef.Path + "/Literccino/GreenteaLiterccino.jpg",
                    MenuCategory = ECategory.LITERCCINO,
                    Price = 1000
                });
                MenuItems.Add(new MenuModel()
                {
                    Idx = 30,
                    Name = "JavachipLiterccino",
                    ImageUrl = ComDef.Path + "/Literccino/JavachipLiterccino.jpg",
                    MenuCategory = ECategory.LITERCCINO,
                    Price = 1000
                });
                MenuItems.Add(new MenuModel()
                {
                    Idx = 31,
                    Name = "MintChocochipLiterccino",
                    ImageUrl = ComDef.Path + "/Literccino/MintChocochipLiterccino.jpg",
                    MenuCategory = ECategory.LITERCCINO,
                    Price = 1000
                });
                #endregion

                #region Tea
                MenuItems.Add(new MenuModel()
                {
                    Idx = 32,
                    Name = "FruitTea",
                    ImageUrl = ComDef.Path + "/Tea/FruitTea.jpg",
                    MenuCategory = ECategory.TEA,
                    Price = 1000
                });
                MenuItems.Add(new MenuModel()
                {
                    Idx = 33,
                    Name = "HerbTea",
                    ImageUrl = ComDef.Path + "/Tea/HerbTea.jpg",
                    MenuCategory = ECategory.TEA,
                    Price = 1000
                });
                #endregion

                #region TheLiterSpecial
                MenuItems.Add(new MenuModel()
                {
                    Idx = 34,
                    Name = "Bananalatte",
                    ImageUrl = ComDef.Path + "/TheLiterSpecial/Bananalatte.jpg",
                    MenuCategory = ECategory.THELITERSPECIAL,
                    Price = 1000
                });
                MenuItems.Add(new MenuModel()
                {
                    Idx = 35,
                    Name = "C1Soda",
                    ImageUrl = ComDef.Path + "/TheLiterSpecial/C1Soda.jpg",
                    MenuCategory = ECategory.THELITERSPECIAL,
                    Price = 1000
                });
                MenuItems.Add(new MenuModel()
                {
                    Idx = 36,
                    Name = "ChamMelon",
                    ImageUrl = ComDef.Path + "/TheLiterSpecial/ChamMelon.jpg",
                    MenuCategory = ECategory.THELITERSPECIAL,
                    Price = 1000
                });
                MenuItems.Add(new MenuModel()
                {
                    Idx = 37,
                    Name = "Grainlatte",
                    ImageUrl = ComDef.Path + "/TheLiterSpecial/Grainlatte.jpg",
                    MenuCategory = ECategory.THELITERSPECIAL,
                    Price = 1000
                });
                MenuItems.Add(new MenuModel()
                {
                    Idx = 38,
                    Name = "PeachSoongsoong",
                    ImageUrl = ComDef.Path + "/TheLiterSpecial/PeachSoongsoong.jpg",
                    MenuCategory = ECategory.THELITERSPECIAL,
                    Price = 1000
                });
                MenuItems.Add(new MenuModel()
                {
                    Idx = 39,
                    Name = "PineappleSoda",
                    ImageUrl = ComDef.Path + "/TheLiterSpecial/PineappleSoda.jpg",
                    MenuCategory = ECategory.THELITERSPECIAL,
                    Price = 1000
                });
                MenuItems.Add(new MenuModel()
                {
                    Idx = 40,
                    Name = "ShiningSuger",
                    ImageUrl = ComDef.Path + "/TheLiterSpecial/ShiningSuger.jpg",
                    MenuCategory = ECategory.THELITERSPECIAL,
                    Price = 1000
                });
                MenuItems.Add(new MenuModel()
                {
                    Idx = 41,
                    Name = "StrawberrySoksok",
                    ImageUrl = ComDef.Path + "/TheLiterSpecial/StrawberrySoksok.jpg",
                    MenuCategory = ECategory.THELITERSPECIAL,
                    Price = 1000
                });
                MenuItems.Add(new MenuModel()
                {
                    Idx = 42,
                    Name = "sugarlatte",
                    ImageUrl = ComDef.Path + "/TheLiterSpecial/sugarlatte.jpg",
                    MenuCategory = ECategory.THELITERSPECIAL,
                    Price = 1000
                });
                MenuItems.Add(new MenuModel()
                {
                    Idx = 43,
                    Name = "sugarmilktea",
                    ImageUrl = ComDef.Path + "/TheLiterSpecial/sugarmilktea.jpg",
                    MenuCategory = ECategory.THELITERSPECIAL,
                    Price = 1000
                });
                MenuItems.Add(new MenuModel()
                {
                    Idx = 44,
                    Name = "AvocadoJuice",
                    ImageUrl = ComDef.Path + "/TheLiterSpecial/AvocadoJuice.jpg",
                    MenuCategory = ECategory.THELITERSPECIAL,
                    Price = 1000
                });
                MenuItems.Add(new MenuModel()
                {
                    Idx = 45,
                    Name = "BlueberryJuice",
                    ImageUrl = ComDef.Path + "/TheLiterSpecial/BlueberryJuice.jpg",
                    MenuCategory = ECategory.THELITERSPECIAL,
                    Price = 1000
                });
                MenuItems.Add(new MenuModel()
                {
                    Idx = 46,
                    Name = "PersimmonJuice",
                    ImageUrl = ComDef.Path + "/TheLiterSpecial/PersimmonJuice.jpg",
                    MenuCategory = ECategory.THELITERSPECIAL,
                    Price = 1000
                });
                #endregion

                #region Yogurs
                MenuItems.Add(new MenuModel()
                {
                    Idx = 47,
                    Name = "BlueberryYogurs",
                    ImageUrl = ComDef.Path + "/Yogurs/BlueberryYogurs.jpg",
                    MenuCategory = ECategory.YOGURS,
                    Price = 1000
                });
                MenuItems.Add(new MenuModel()
                {
                    Idx = 48,
                    Name = "CitronYogurs",
                    ImageUrl = ComDef.Path + "/Yogurs/CitronYogurs.jpg",
                    MenuCategory = ECategory.YOGURS,
                    Price = 1000
                });
                MenuItems.Add(new MenuModel()
                {
                    Idx = 49,
                    Name = "MangoYogurs",
                    ImageUrl = ComDef.Path + "/Yogurs/MangoYogurs.jpg",
                    MenuCategory = ECategory.YOGURS,
                    Price = 1000
                });
                MenuItems.Add(new MenuModel()
                {
                    Idx = 50,
                    Name = "PeachYogurs",
                    ImageUrl = ComDef.Path + "/Yogurs/PeachYogurs.jpg",
                    MenuCategory = ECategory.YOGURS,
                    Price = 1000
                });
                MenuItems.Add(new MenuModel()
                {
                    Idx = 51,
                    Name = "PlainYogurs",
                    ImageUrl = ComDef.Path + "/Yogurs/PlainYogurs.jpg",
                    MenuCategory = ECategory.YOGURS,
                    Price = 1000
                });
                MenuItems.Add(new MenuModel()
                {
                    Idx = 52,
                    Name = "StrawberryYogurs",
                    ImageUrl = ComDef.Path + "/Yogurs/StrawberryYogurs.jpg",
                    MenuCategory = ECategory.YOGURS,
                    Price = 1000
                });
                #endregion
                #endregion
            });
        }

        public void ClearMenuData()
        {
            for (int i = 0; i < OrderedMenuItems.Count; i++)
            {
                OrderedMenuItems[i].Count = 0;
                OrderedMenuItems[i].TotalPrice = 0;
            }

            OrderedMenuItems.Clear();
            OrderTotalPrice = 0;
        }

        public bool IsOrderedMenuListValid()
        {
            return (OrderedMenuItems != null && OrderedMenuItems.Count > 0) ? true : false;
        }

        public void AddOrderedMenuItems(MenuModel selectedMenu)
        {
            OrderedMenuItems.Add(selectedMenu);
        }

        public void IncreaseMenuCount(MenuModel selectedMenu)
        {
            selectedMenu.Count++;
            selectedMenu.TotalPrice += selectedMenu.Price;
            OrderTotalPrice += selectedMenu.Price;
        }

        public void DecreaseMenuCount(MenuModel selectedMenu)
        {
            selectedMenu.Count--;
            selectedMenu.TotalPrice -= selectedMenu.Price;
            OrderTotalPrice -= selectedMenu.Price;
        }

        public void ClearSelectedMenuItems(MenuModel selectedMenu)
        {
            var temp = selectedMenu.Count;
            var removeTarget = OrderedMenuItems.Where(x => x.Name == selectedMenu.Name).FirstOrDefault();
            for (int i = 0; i < temp; i++)
            {
                selectedMenu.Count--;
                OrderTotalPrice -= removeTarget.Price;
            }
            selectedMenu.TotalPrice = 0;
            RemoveSelectedMenu(selectedMenu);
        }

        public void RemoveSelectedMenu(MenuModel selectedMenu)
        { 
            OrderedMenuItems.Remove(selectedMenu);
        }

        public void ClearQrCode()
        {
            QrCode = string.Empty;
        }

        #region DataBase
        public async void SaveSalesInformation(DateTime payTime, string payType, int? tableIdx, string memberId)
        {
            if (IsOrderedMenuListValid())
            {
                try
                {
                    using (var db = GetConnection())
                    {
                        db.Open();

                        for (int i = 0; i < OrderedMenuItems.Count; i++)
                        {
                            var salesModel = new SalesModel();
                            salesModel.Category = OrderedMenuItems[i].MenuCategory.ToString();
                            salesModel.Name = OrderedMenuItems[i].Name;
                            salesModel.Count = OrderedMenuItems[i].Count;
                            salesModel.Price = OrderedMenuItems[i].TotalPrice;
                            salesModel.PayTime = payTime;
                            salesModel.PayType = payType;
                            if (tableIdx == null) salesModel.TableIdx = -1;
                            else salesModel.TableIdx = (int)tableIdx;
                            salesModel.MemberId = memberId;

                            string insertSql = @"
INSERT INTO sales_tb(
    menu_category,
    menu_name,
    count,
    price,
    payTime,
    payType,
    tableIdx,
    member_id
)
VALUES(
    @Category,
    @Name,
    @Count,
    @Price,
    @PayTime,
    @PayType,
    @TableIdx,
    @MemberId
);";
                            if (await salesDBManager.InsertAsync(db, insertSql, salesModel) == 1)
                                Debug.WriteLine("SUCCESS SAVE SALES INFORMATION");
                            else
                                Debug.WriteLine("FAILURE SAVE SALES INFORMATION");
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.Write("SAVE SALE INFORMATION ERROR : " + e.Message);
                }
            }
        }
        #endregion

        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
