using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using THE_LITER_KIOSK.Common;
using TheLiter.Core.DBManager;
using TheLiter.Core.Network;
using TheLiter.Core.Network.Model;
using TheLiter.Core.Order.DataBase.Model;
using TheLiter.Core.Order.Interface;
using TheLiter.Core.Order.Model;

namespace TheLiter.Core.Order.ViewModel
{
    public class OrderDBViewModel : MySqlDBConnectionManager, INotifyPropertyChanged, IDataSave
    {
        private DBManager<THE_LITER_KIOSK.DataBase.Models.SalesModel> salesDBManager = new DBManager<THE_LITER_KIOSK.DataBase.Models.SalesModel>();
        private DBManager<ReceiptModel> receiptDBManager = new DBManager<ReceiptModel>();
        private DBManager<Model.SalesModel> orderDBManager = new DBManager<Model.SalesModel>();

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

        private ObservableCollection<Model.SalesModel> _menuItems;
        public ObservableCollection<Model.SalesModel> MenuItems
        {
            get => _menuItems;
            set
            {
                _menuItems = value;
                NotifyPropertyChanged(nameof(MenuItems));
            }
        }

        private ObservableCollection<Model.SalesModel> _orderedMenuItems;
        public ObservableCollection<Model.SalesModel> OrderedMenuItems
        {
            get => _orderedMenuItems;
            set 
            {
                _orderedMenuItems = value; 
                NotifyPropertyChanged(nameof(OrderedMenuItems)); 
            }
        }

        private int _discountTotalPrice = 0;
        public int DiscountTotalPrice
        {
            get => _discountTotalPrice;
            set
            {
                _discountTotalPrice = value;
                NotifyPropertyChanged(nameof(DiscountTotalPrice));
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

        private int _receiptIdx;
        public int ReceiptIdx
        {
            get => _receiptIdx;
            set
            {
                _receiptIdx = value;
                NotifyPropertyChanged(nameof(ReceiptIdx));
            }
        }
        
        private bool _previousBtnIsEnabled = false;
        public bool PreviousBtnIsEnabled 
        {
            get => _previousBtnIsEnabled;
            set
            {
                _previousBtnIsEnabled = value;
                NotifyPropertyChanged(nameof(PreviousBtnIsEnabled)); 
            }
        }

        private bool _nextBtnIsEnabled = false;
        public bool NextBtnIsEnabled 
        {
            get => _nextBtnIsEnabled;
            set 
            {
                _nextBtnIsEnabled = value; 
                NotifyPropertyChanged(nameof(NextBtnIsEnabled)); 
            }
        }

        public int itemPerPage = 9;
        
        private int _currentPageIdx = 1;
        public int CurrentPageIdx
        {
            get => _currentPageIdx;
            set
            {
                _currentPageIdx = value;
                PreviousBtnIsEnabled = false;
                NextBtnIsEnabled = false;

                PagingMenuItems();
                SetMenuDiscountRateAndIsSoldOut();

                if (CurrentPageIdx > 1)
                    PreviousBtnIsEnabled = true;
                if (CurrentMenuList.Count - (CurrentPageIdx * itemPerPage) > 0)
                    NextBtnIsEnabled = true;
            }
        }

        private List<Model.SalesModel> _menuList;
        public List<Model.SalesModel> MenuList
        {
            get => _menuList;
            set { _menuList = value; }
        }

        private List<Model.SalesModel> _currentMenuList;
        public List<Model.SalesModel> CurrentMenuList
        {
            get => _currentMenuList;
            set
            {
                _currentPageIdx = 1;
                _currentMenuList = value;
                PreviousBtnIsEnabled = false;

                PagingMenuItems();
                SetMenuDiscountRateAndIsSoldOut();

                if (CurrentMenuList.Count > itemPerPage)
                    NextBtnIsEnabled = true;
                else
                    NextBtnIsEnabled = false;
            }
        }

        private ObservableCollection<Model.SalesModel> _pagingMenuList;
        public ObservableCollection<Model.SalesModel> PagingMenuList
        {
            get => _pagingMenuList;
            set 
            {
                _pagingMenuList = value;
                NotifyPropertyChanged(nameof(PagingMenuList)); 
            }
        }

        private bool _orderBtnIsEnabled = false;
        public bool OrderBtnIsEnabled
        {
            get => _orderBtnIsEnabled;
            set
            {
                _orderBtnIsEnabled = value;
                NotifyPropertyChanged(nameof(OrderBtnIsEnabled));
            }
        }

        private bool _clearAllMenuItemBtnIsEnabled = false;
        public bool ClearAllMenuItemBtnIsEnabled
        {
            get => _clearAllMenuItemBtnIsEnabled;
            set
            {
                _clearAllMenuItemBtnIsEnabled = value;
                NotifyPropertyChanged(nameof(ClearAllMenuItemBtnIsEnabled));
            }
        }

        private Model.SalesModel _selectedMenu;
        public Model.SalesModel SelectedMenu
        {
            get => _selectedMenu;
            set
            {
                _selectedMenu = value;
                NotifyPropertyChanged(nameof(SelectedMenu));
            }
        }

        private int _discountRate;
        public int DisCountRate
        {
            get => _discountRate;
            set
            {
                _discountRate = value;
                NotifyPropertyChanged(nameof(DisCountRate));
            }
        }

        private bool _isSoldOutChecked;
        public bool IsSoldOutChecked
        {
            get => _isSoldOutChecked;
            set
            {
                _isSoldOutChecked = value;
                NotifyPropertyChanged(nameof(IsSoldOutChecked));
            }
        }
        #endregion

        #region Commands
        public ICommand PreviousMenuCommand { get; set; }
        public ICommand NextMenuCommand { get; set; }
        #endregion

        #region Constructor
        public OrderDBViewModel()
        {
            InitVariables();
            InitCommands();
        }
        #endregion

        #region Init
        private void InitVariables()
        {
            CategoryItems = new ObservableCollection<CategoryModel>();
            MenuItems = new ObservableCollection<Model.SalesModel>();
            OrderedMenuItems = new ObservableCollection<Model.SalesModel>();
        }

        private void InitCommands()
        {
            PreviousMenuCommand = new DelegateCommand(IncreasePageIdx);
            NextMenuCommand = new DelegateCommand(DecreasePageIdx);
        }

        internal void SetPagingMenuItems()
        {
            MenuList = MenuItems.ToList();
            CurrentMenuList = MenuList;
        }
        #endregion

        internal void LoadOrderData()
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
                MenuItems.Add(new Model.SalesModel()
                {
                    Idx = 1,
                    Name = "CitronAde",
                    ImageUrl = ComDef.Path + "/Ade/CitronAde.jpg",
                    MenuCategory = ECategory.ADE,
                    Price = 1000,
                });
                MenuItems.Add(new Model.SalesModel()
                {
                    Idx = 2,
                    Name = "GrapefruitAde",
                    ImageUrl = ComDef.Path + "/Ade/GrapefruitAde.jpg",
                    MenuCategory = ECategory.ADE,
                    Price = 1000
                });
                MenuItems.Add(new Model.SalesModel()
                {
                    Idx = 3,
                    Name = "GreengrapeAde",
                    ImageUrl = ComDef.Path + "/Ade/GreengrapeAde.jpg",
                    MenuCategory = ECategory.ADE,
                    Price = 1000
                });
                MenuItems.Add(new Model.SalesModel()
                {
                    Idx = 4,
                    Name = "LemonAde",
                    ImageUrl = ComDef.Path + "/Ade/LemonAde.jpg",
                    MenuCategory = ECategory.ADE,
                    Price = 1000
                });
                MenuItems.Add(new Model.SalesModel()
                {
                    Idx = 5,
                    Name = "MojitoAde",
                    ImageUrl = ComDef.Path + "/Ade/MojitoAde.jpg",
                    MenuCategory = ECategory.ADE,
                    Price = 1000
                });
                #endregion

                #region Coffee
                MenuItems.Add(new Model.SalesModel()
                {
                    Idx = 6,
                    Name = "Americano",
                    ImageUrl = ComDef.Path + "/Coffee/Americano.jpg",
                    MenuCategory = ECategory.ADE,
                    Price = 1000
                });
                MenuItems.Add(new Model.SalesModel()
                {
                    Idx = 7,
                    Name = "Cafelatte",
                    ImageUrl = ComDef.Path + "/Coffee/Cafelatte.jpg",
                    MenuCategory = ECategory.COFFEE,
                    Price = 1000
                });
                MenuItems.Add(new Model.SalesModel()
                {
                    Idx = 8,
                    Name = "CafeMocha",
                    ImageUrl = ComDef.Path + "/Coffee/CafeMocha.jpg",
                    MenuCategory = ECategory.COFFEE,
                    Price = 1000
                });
                MenuItems.Add(new Model.SalesModel()
                {
                    Idx = 9,
                    Name = "Cappuccino",
                    ImageUrl = ComDef.Path + "/Coffee/Cappuccino.jpg",
                    MenuCategory = ECategory.COFFEE,
                    Price = 1000
                });
                MenuItems.Add(new Model.SalesModel()
                {
                    Idx = 10,
                    Name = "CaramelMacchiato",
                    ImageUrl = ComDef.Path + "/Coffee/CaramelMacchiato.jpg",
                    MenuCategory = ECategory.COFFEE,
                    Price = 1000
                });
                MenuItems.Add(new Model.SalesModel()
                {
                    Idx = 11,
                    Name = "ColdBrew",
                    ImageUrl = ComDef.Path + "/Coffee/ColdBrew.jpg",
                    MenuCategory = ECategory.COFFEE,
                    Price = 1000
                });
                MenuItems.Add(new Model.SalesModel()
                {
                    Idx = 12,
                    Name = "ColdBrewLatte",
                    ImageUrl = ComDef.Path + "/Coffee/ColdBrewLatte.jpg",
                    MenuCategory = ECategory.COFFEE,
                    Price = 1000
                });
                MenuItems.Add(new Model.SalesModel()
                {
                    Idx = 13,
                    Name = "CondensedMilkLatte",
                    ImageUrl = ComDef.Path + "/Coffee/CondensedMilkLatte.jpg",
                    MenuCategory = ECategory.COFFEE,
                    Price = 1000
                });
                MenuItems.Add(new Model.SalesModel()
                {
                    Idx = 14,
                    Name = "HazelnutLatte",
                    ImageUrl = ComDef.Path + "/Coffee/HazelnutLatte.jpg",
                    MenuCategory = ECategory.COFFEE,
                    Price = 1000
                });
                MenuItems.Add(new Model.SalesModel()
                {
                    Idx = 15,
                    Name = "TiramisuLatte",
                    ImageUrl = ComDef.Path + "/Coffee/TiramisuLatte.jpg",
                    MenuCategory = ECategory.COFFEE,
                    Price = 1000
                });
                MenuItems.Add(new Model.SalesModel()
                {
                    Idx = 16,
                    Name = "vanillaLatte",
                    ImageUrl = ComDef.Path + "/Coffee/vanillaLatte.jpg",
                    MenuCategory = ECategory.COFFEE,
                    Price = 1000
                });
                #endregion

                #region Desert
                MenuItems.Add(new Model.SalesModel()
                {
                    Idx = 17,
                    Name = "Cookie",
                    ImageUrl = ComDef.Path + "/Desert/Cookie.jpg",
                    MenuCategory = ECategory.DESERT,
                    Price = 1000
                });
                MenuItems.Add(new Model.SalesModel()
                {
                    Idx = 18,
                    Name = "CurstardStick",
                    ImageUrl = ComDef.Path + "/Desert/CurstardStick.jpg",
                    MenuCategory = ECategory.DESERT,
                    Price = 1000
                });
                MenuItems.Add(new Model.SalesModel()
                {
                    Idx = 19,
                    Name = "HeartPie",
                    ImageUrl = ComDef.Path + "/Desert/HeartPie.jpg",
                    MenuCategory = ECategory.DESERT,
                    Price = 1000
                });
                MenuItems.Add(new Model.SalesModel()
                {
                    Idx = 20,
                    Name = "MuffinSandwich",
                    ImageUrl = ComDef.Path + "/Desert/MuffinSandwich.jpg",
                    MenuCategory = ECategory.DESERT,
                    Price = 1000
                });
                MenuItems.Add(new Model.SalesModel()
                {
                    Idx = 21,
                    Name = "Waffle",
                    ImageUrl = ComDef.Path + "/Desert/Waffle.jpg",
                    MenuCategory = ECategory.DESERT,
                    Price = 1000
                });
                #endregion

                #region Latte
                MenuItems.Add(new Model.SalesModel()
                {
                    Idx = 22,
                    Name = "DeepChocoLatte",
                    ImageUrl = ComDef.Path + "/Latte/DeepChocoLatte.jpg",
                    MenuCategory = ECategory.LATTE,
                    Price = 1000
                });
                MenuItems.Add(new Model.SalesModel()
                {
                    Idx = 23,
                    Name = "GreenteaLatte",
                    ImageUrl = ComDef.Path + "/Latte/GreenteaLatte.jpg",
                    MenuCategory = ECategory.LATTE,
                    Price = 1000
                });
                MenuItems.Add(new Model.SalesModel()
                {
                    Idx = 24,
                    Name = "MilkTea",
                    ImageUrl = ComDef.Path + "/Latte/MilkTea.jpg",
                    MenuCategory = ECategory.LATTE,
                    Price = 1000
                });
                MenuItems.Add(new Model.SalesModel()
                {
                    Idx = 25,
                    Name = "MintChocoLatte",
                    ImageUrl = ComDef.Path + "/Latte/MintChocoLatte.jpg",
                    MenuCategory = ECategory.LATTE,
                    Price = 1000
                });
                MenuItems.Add(new Model.SalesModel()
                {
                    Idx = 26,
                    Name = "PurplesweetpotatoLatte",
                    ImageUrl = ComDef.Path + "/Latte/PurplesweetpotatoLatte.jpg",
                    MenuCategory = ECategory.LATTE,
                    Price = 1000
                });
                #endregion

                #region Literccino
                MenuItems.Add(new Model.SalesModel()
                {
                    Idx = 27,
                    Name = "CookieCreamLiterccino",
                    ImageUrl = ComDef.Path + "/Literccino/CookieCreamLiterccino.jpg",
                    MenuCategory = ECategory.LITERCCINO,
                    Price = 1000
                });
                MenuItems.Add(new Model.SalesModel()
                {
                    Idx = 28,
                    Name = "DoublechocoLiterccino",
                    ImageUrl = ComDef.Path + "/Literccino/DoublechocoLiterccino.jpg",
                    MenuCategory = ECategory.LITERCCINO,
                    Price = 1000
                });
                MenuItems.Add(new Model.SalesModel()
                {
                    Idx = 29,
                    Name = "GreenteaLiterccino",
                    ImageUrl = ComDef.Path + "/Literccino/GreenteaLiterccino.jpg",
                    MenuCategory = ECategory.LITERCCINO,
                    Price = 1000
                });
                MenuItems.Add(new Model.SalesModel()
                {
                    Idx = 30,
                    Name = "JavachipLiterccino",
                    ImageUrl = ComDef.Path + "/Literccino/JavachipLiterccino.jpg",
                    MenuCategory = ECategory.LITERCCINO,
                    Price = 1000
                });
                MenuItems.Add(new Model.SalesModel()
                {
                    Idx = 31,
                    Name = "MintChocochipLiterccino",
                    ImageUrl = ComDef.Path + "/Literccino/MintChocochipLiterccino.jpg",
                    MenuCategory = ECategory.LITERCCINO,
                    Price = 1000
                });
                #endregion

                #region Tea
                MenuItems.Add(new Model.SalesModel()
                {
                    Idx = 32,
                    Name = "FruitTea",
                    ImageUrl = ComDef.Path + "/Tea/FruitTea.jpg",
                    MenuCategory = ECategory.TEA,
                    Price = 1000
                });
                MenuItems.Add(new Model.SalesModel()
                {
                    Idx = 33,
                    Name = "HerbTea",
                    ImageUrl = ComDef.Path + "/Tea/HerbTea.jpg",
                    MenuCategory = ECategory.TEA,
                    Price = 1000
                });
                #endregion

                #region TheLiterSpecial
                MenuItems.Add(new Model.SalesModel()
                {
                    Idx = 34,
                    Name = "Bananalatte",
                    ImageUrl = ComDef.Path + "/TheLiterSpecial/Bananalatte.jpg",
                    MenuCategory = ECategory.THELITERSPECIAL,
                    Price = 1000
                });
                MenuItems.Add(new Model.SalesModel()
                {
                    Idx = 35,
                    Name = "C1Soda",
                    ImageUrl = ComDef.Path + "/TheLiterSpecial/C1Soda.jpg",
                    MenuCategory = ECategory.THELITERSPECIAL,
                    Price = 1000
                });
                MenuItems.Add(new Model.SalesModel()
                {
                    Idx = 36,
                    Name = "ChamMelon",
                    ImageUrl = ComDef.Path + "/TheLiterSpecial/ChamMelon.jpg",
                    MenuCategory = ECategory.THELITERSPECIAL,
                    Price = 1000
                });
                MenuItems.Add(new Model.SalesModel()
                {
                    Idx = 37,
                    Name = "Grainlatte",
                    ImageUrl = ComDef.Path + "/TheLiterSpecial/Grainlatte.jpg",
                    MenuCategory = ECategory.THELITERSPECIAL,
                    Price = 1000
                });
                MenuItems.Add(new Model.SalesModel()
                {
                    Idx = 38,
                    Name = "PeachSoongsoong",
                    ImageUrl = ComDef.Path + "/TheLiterSpecial/PeachSoongsoong.jpg",
                    MenuCategory = ECategory.THELITERSPECIAL,
                    Price = 1000
                });
                MenuItems.Add(new Model.SalesModel()
                {
                    Idx = 39,
                    Name = "PineappleSoda",
                    ImageUrl = ComDef.Path + "/TheLiterSpecial/PineappleSoda.jpg",
                    MenuCategory = ECategory.THELITERSPECIAL,
                    Price = 1000
                });
                MenuItems.Add(new Model.SalesModel()
                {
                    Idx = 40,
                    Name = "ShiningSuger",
                    ImageUrl = ComDef.Path + "/TheLiterSpecial/ShiningSuger.jpg",
                    MenuCategory = ECategory.THELITERSPECIAL,
                    Price = 1000
                });
                MenuItems.Add(new Model.SalesModel()
                {
                    Idx = 41,
                    Name = "StrawberrySoksok",
                    ImageUrl = ComDef.Path + "/TheLiterSpecial/StrawberrySoksok.jpg",
                    MenuCategory = ECategory.THELITERSPECIAL,
                    Price = 1000
                });
                MenuItems.Add(new Model.SalesModel()
                {
                    Idx = 42,
                    Name = "sugarlatte",
                    ImageUrl = ComDef.Path + "/TheLiterSpecial/sugarlatte.jpg",
                    MenuCategory = ECategory.THELITERSPECIAL,
                    Price = 1000
                });
                MenuItems.Add(new Model.SalesModel()
                {
                    Idx = 43,
                    Name = "sugarmilktea",
                    ImageUrl = ComDef.Path + "/TheLiterSpecial/sugarmilktea.jpg",
                    MenuCategory = ECategory.THELITERSPECIAL,
                    Price = 1000
                });
                MenuItems.Add(new Model.SalesModel()
                {
                    Idx = 44,
                    Name = "AvocadoJuice",
                    ImageUrl = ComDef.Path + "/TheLiterSpecial/AvocadoJuice.jpg",
                    MenuCategory = ECategory.THELITERSPECIAL,
                    Price = 1000
                });
                MenuItems.Add(new Model.SalesModel()
                {
                    Idx = 45,
                    Name = "BlueberryJuice",
                    ImageUrl = ComDef.Path + "/TheLiterSpecial/BlueberryJuice.jpg",
                    MenuCategory = ECategory.THELITERSPECIAL,
                    Price = 1000
                });
                MenuItems.Add(new Model.SalesModel()
                {
                    Idx = 46,
                    Name = "PersimmonJuice",
                    ImageUrl = ComDef.Path + "/TheLiterSpecial/PersimmonJuice.jpg",
                    MenuCategory = ECategory.THELITERSPECIAL,
                    Price = 1000
                });
                #endregion

                #region Yogurs
                MenuItems.Add(new Model.SalesModel()
                {
                    Idx = 47,
                    Name = "BlueberryYogurs",
                    ImageUrl = ComDef.Path + "/Yogurs/BlueberryYogurs.jpg",
                    MenuCategory = ECategory.YOGURS,
                    Price = 1000
                });
                MenuItems.Add(new Model.SalesModel()
                {
                    Idx = 48,
                    Name = "CitronYogurs",
                    ImageUrl = ComDef.Path + "/Yogurs/CitronYogurs.jpg",
                    MenuCategory = ECategory.YOGURS,
                    Price = 1000
                });
                MenuItems.Add(new Model.SalesModel()
                {
                    Idx = 49,
                    Name = "MangoYogurs",
                    ImageUrl = ComDef.Path + "/Yogurs/MangoYogurs.jpg",
                    MenuCategory = ECategory.YOGURS,
                    Price = 1000
                });
                MenuItems.Add(new Model.SalesModel()
                {
                    Idx = 50,
                    Name = "PeachYogurs",
                    ImageUrl = ComDef.Path + "/Yogurs/PeachYogurs.jpg",
                    MenuCategory = ECategory.YOGURS,
                    Price = 1000
                });
                MenuItems.Add(new Model.SalesModel()
                {
                    Idx = 51,
                    Name = "PlainYogurs",
                    ImageUrl = ComDef.Path + "/Yogurs/PlainYogurs.jpg",
                    MenuCategory = ECategory.YOGURS,
                    Price = 1000
                });
                MenuItems.Add(new Model.SalesModel()
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

        public void IsEnabledOrderAndClearAllMenuBtn()
        {
            if (IsValidOrderedMenuItems())
            {
                OrderBtnIsEnabled = true;
                ClearAllMenuItemBtnIsEnabled = true;
            }
            else
            {
                OrderBtnIsEnabled = false;
                ClearAllMenuItemBtnIsEnabled = false;
            }
        }

        private void PagingMenuItems()
        {
            int remainCurMenusCnt = GetRemainCurMenusCnt();

            if (remainCurMenusCnt > 0 && remainCurMenusCnt < itemPerPage)
            {
                PagingMenuList = ExtractMenuItems(GetRemainCurMenusCnt());
            }
            else if (remainCurMenusCnt >= itemPerPage)
            {
                PagingMenuList = ExtractMenuItems(itemPerPage);
            }
        }

        private int GetRemainCurMenusCnt()
        {
            return CurrentMenuList.Count - GetCurrentMenusCnt();
        }

        private int GetCurrentMenusCnt()
        {
            return (CurrentPageIdx * itemPerPage) - itemPerPage;
        }

        private ObservableCollection<Model.SalesModel> ExtractMenuItems(int itemPerPage)
        {
            return new ObservableCollection<Model.SalesModel>(CurrentMenuList.GetRange(GetCurrentMenusCnt(), itemPerPage).ToList());
        }

        private void IncreasePageIdx()
        {
            CurrentPageIdx--;
        }

        private void DecreasePageIdx()
        {
            CurrentPageIdx++;
        }

        internal void ClearMenuItems()
        {
            for (int i = 0; i < OrderedMenuItems.Count; i++)
            {
                OrderedMenuItems[i].Count = 0;
                OrderedMenuItems[i].DiscountPrice = 0;
            }

            OrderedMenuItems.Clear();
            DiscountTotalPrice = 0;
        }

        internal bool IsValidOrderedMenuItems()
        {
            return (OrderedMenuItems != null && OrderedMenuItems.Count > 0) ? true : false;
        }

        public bool IsQuantityValid(Model.SalesModel selectedMenu)
        {
            return (selectedMenu.Count == 1) ? true : false;
        }

        public void AddOrderedMenuItems(Model.SalesModel selectedMenu)
        {
            OrderedMenuItems.Add(selectedMenu);
        }

        public void IncreaseMenuCount(Model.SalesModel selectedMenu)
        {
            int discountAmount = ((selectedMenu.Price * selectedMenu.DiscountRate) / 100);
            int discountPrice = selectedMenu.Price - discountAmount; 
            selectedMenu.Count++;
            selectedMenu.DiscountPrice += discountPrice;  
            DiscountTotalPrice += discountPrice; 
            selectedMenu.DiscountAmount = discountAmount; 
            selectedMenu.TotalPrice += selectedMenu.Price;
            OrderTotalPrice += selectedMenu.Price; 
        }

        public void DecreaseMenuCount(Model.SalesModel selectedMenu)
        {
            int discountPrice = selectedMenu.Price - ((selectedMenu.Price * selectedMenu.DiscountRate) / 100); 
            selectedMenu.Count--;
            selectedMenu.DiscountPrice -= discountPrice; 
            DiscountTotalPrice -= discountPrice; 
            selectedMenu.TotalPrice -= selectedMenu.Price; 
            OrderTotalPrice -= selectedMenu.Price; 
        }

        public void ClearSelectedMenuItems(Model.SalesModel selectedMenu)
        {
            var temp = selectedMenu.Count;
            var removeTarget = OrderedMenuItems.Where(x => x.Name == selectedMenu.Name).FirstOrDefault();
            for (int i = 0; i < temp; i++)
            {
                selectedMenu.Count--;
                DiscountTotalPrice -= removeTarget.DiscountPrice;
            }
            selectedMenu.DiscountPrice = 0;
            RemoveSelectedMenu(selectedMenu);
            IsEnabledOrderAndClearAllMenuBtn();
        }

        public void RemoveSelectedMenu(Model.SalesModel selectedMenu)
        { 
            OrderedMenuItems.Remove(selectedMenu);
            IsEnabledOrderAndClearAllMenuBtn();
        }

        private async void SetMenuDiscountRateAndIsSoldOut()
        {
            List<Model.SalesModel> items = await GetAllMenuDisCountRateAndIsSoldOut();

            for (int i = 0; i < PagingMenuList.Count; i++)
            {
                for (int j = 0; j < items.Count; j++)
                {
                    if (PagingMenuList[i].Idx == items[j].Idx)
                    {
                        PagingMenuList[i].DiscountRate = items[j].DiscountRate;
                        PagingMenuList[i].IsSoldOut = items[j].IsSoldOut;
                        break;
                    }
                }
            }
        }

        public TcpModel SendOrderInfoToNormal(string id)
        {
            var tcpModel = new TcpModel();
            var menuItems = new List<MenuModel>();
            var orderedMenuItems = OrderedMenuItems.ToList();

            tcpModel.MessageType = (int)EMessageType.ORDER_INFO;
            tcpModel.Id = id;
            tcpModel.ShopName = "더리터 사이코점";
            tcpModel.Content = "";

            Debug.WriteLine(ReceiptIdx % 100);
            tcpModel.OrderNumber = string.Format("{0:D3}", (ReceiptIdx % 100));
            Debug.WriteLine(tcpModel.OrderNumber);

            for (int i = 0; i < orderedMenuItems.Count; i++)
            {
                menuItems.Add(new MenuModel()
                {
                    Name = orderedMenuItems[i].Name,
                    Price = orderedMenuItems[i].Count,
                    Count = orderedMenuItems[i].Count
                });
            }

            tcpModel.MenuItems = menuItems;
            return tcpModel;
        }

        public MessageModel SendOrderInfoToGroup(string id, ObservableCollection<Model.SalesModel> OrderedMenuItems)
        {
            var messageModel = new MessageModel();
            var menuItems = new List<MenuModel>();
            var orderedMenuItems = OrderedMenuItems.ToList();

            messageModel.MessageType = (int)EMessageType.ORDER_INFO;
            messageModel.Id = id;
            messageModel.ShopName = "더리터 사이코점";
            messageModel.Content = "";
            messageModel.OrderNumber = string.Format("{0:D3}", ReceiptIdx % 100);
            messageModel.Group = true;

            for (int i = 0; i < orderedMenuItems.Count; i++)
            {
                menuItems.Add(new MenuModel()
                {
                    Name = orderedMenuItems[i].Name,
                    Price = orderedMenuItems[i].Count,
                    Count = orderedMenuItems[i].Count
                });
            }

            messageModel.MenuItems = menuItems;
            return messageModel;
        }

        #region DataBase
        public async void GetReceiptIdx()
        {
            try
            {
                using (var db = GetConnection())
                {
                    db.Open();

                    string selectSql = @"
SELECT
    *
FROM
    receipt_tb
ORDER BY 
    receipt_idx DESC LIMIT 1
;";

                    var receiptIdx = await receiptDBManager.GetSingleDataAsync(db, selectSql, "");
                    ReceiptIdx = receiptIdx.ReceiptIdx + 1;
                }
            }
            catch (Exception e)
            {
                Debug.Write("GET RECEIPT IDX ERROR : " + e.Message);
            }
        }

        public async void SaveReceiptIdx()
        {
            try
            {
                using (var db = GetConnection())
                {
                    db.Open();

                    string insertSql = @"
INSERT INTO receipt_tb(
    receipt_idx
)
VALUES(
    @Receipt_idx
);";
                    if (await receiptDBManager.InsertAsync(db, insertSql, ReceiptIdx) == 1)
                        Debug.WriteLine("SUCCESS SAVE RECEIPT IDX");
                    else
                        Debug.WriteLine("FAILURE SAVE RECEIPT IDX");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("SAVE RECEIPT IDX ERROR : " + e.Message);
            }
        }

        public async void SaveSalesInformation(DateTime payTime, string payType, int? tableIdx, string memberId)
        {
            if (IsValidOrderedMenuItems())
            {
                try
                {
                    GetReceiptIdx();
                    SaveReceiptIdx();

                    using (var db = GetConnection())
                    {
                        db.Open();
                        
                        for (int i = 0; i < OrderedMenuItems.Count; i++)
                        {
                            var salesModel = new THE_LITER_KIOSK.DataBase.Models.SalesModel();
                            salesModel.Category = OrderedMenuItems[i].MenuCategory.ToString(); // 메뉴 카테고리
                            salesModel.Name = OrderedMenuItems[i].Name; // 메뉴 이름
                            salesModel.Count = OrderedMenuItems[i].Count; // 메뉴 수량
                            salesModel.PayTime = payTime; // 결제 시간
                            salesModel.PayType = payType; // 결제 방법
                            salesModel.MemberId = memberId; // 회원 아이디

                            if (tableIdx == null) salesModel.TableIdx = -1; // 테이블 번호, 없다면 -1
                            else salesModel.TableIdx = (int)tableIdx; 

                            if (ReceiptIdx > 0) salesModel.ReceiptIdx = ReceiptIdx; // 주문 번호

                            salesModel.DiscountAmount = OrderedMenuItems[i].DiscountAmount * OrderedMenuItems[i].Count; // 할인 금액
                            salesModel.DiscountTotalPrice = OrderedMenuItems[i].DiscountPrice; // 할인이 있다면, 할인이 적용된 가격의 총액
                            salesModel.TotalPrice = OrderedMenuItems[i].TotalPrice; // 메뉴 원래 가격의 총액

                            string insertSql = @"
INSERT INTO sales_tb(
    menuCategory,
    menuName,
    count,
    discountTotalPrice,
    payTime,
    payType,
    tableIdx,
    memberId,
    receiptIdx,
    discountAmount,
    totalPrice
)
VALUES(
    @Category,
    @Name,
    @Count,
    @DiscountTotalPrice,
    @PayTime,
    @PayType,
    @TableIdx,
    @MemberId,
    @ReceiptIdx,
    @DiscountAmount,
    @TotalPrice
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

        public async Task<List<Model.SalesModel>> GetAllMenuDisCountRateAndIsSoldOut()
        {
            try
            {
                
                using (var db = GetConnection())
                {
                    db.Open();

                    string selectSql = @"
SELECT
    *
FROM
    menu_tb
;";
                    var items = await orderDBManager.GetListAsync(db, selectSql, "");
                    if (items != null)
                    {
                        Debug.WriteLine("SUCCESS GET ALL MENU DISCOUNT RATE AND IS SOLD OUT ERROR : ");
                        return items;
                    }
                    else
                    {
                        Debug.WriteLine("GET ALL MENU DISCOUNT RATE AND IS SOLD OUT ERROR : ");
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("GET ALL MENU DISCOUNT RATE AND IS SOLD OUT ERROR : " + e.Message);
            }

            return null;
        }

        public async void SaveMenuDiscountRateAndIsSoldOut()
        {
            try
            {
                using (var db = GetConnection())
                {
                    db.Open();

                    var soldOut = IsSoldOutChecked.ToString();
                    string updateSql = $@"
UPDATE
    menu_tb
SET
    isSoldOut = '{soldOut}'
,
    discountRate = '{DisCountRate}'
WHERE
    idx = '{SelectedMenu.Idx}'
;";
                    if (await orderDBManager.UpdateAsync(db, updateSql, "") == 1)
                    {
                        DisCountRate = 0;
                        IsSoldOutChecked = false;
                        Debug.WriteLine("SUCCESS SAVE MENU DISCOUNT RATE AND IS SOLD OUT");
                    }
                    else
                    {
                        Debug.WriteLine("SAVE MENU DISCOUNT RATE AND IS SOLD OUT ERROR");
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("SAVE MENU DISCOUNT RATE AND IS SOLD OUT ERROR : " + e.Message);
            }
        }
        #endregion

        public void ClearQrCode()
        {
            QrCode = string.Empty;
        }

        public void ClearBarCode()
        {
            BarCode = string.Empty;
        }

        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
