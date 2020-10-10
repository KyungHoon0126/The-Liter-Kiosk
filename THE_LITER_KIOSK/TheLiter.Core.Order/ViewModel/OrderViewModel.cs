using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using THE_LITER_KIOSK.Common;
using TheLiter.Core.Order.Model;

namespace TheLiter.Core.Order.ViewModel
{
    public class OrderViewModel : BindableBase
    {
        #region Properties
        private ObservableCollection<CategoryModel> _categoryItems;
        public ObservableCollection<CategoryModel> CategoryItems
        {
            get => _categoryItems;
            set
            {
                SetProperty(ref _categoryItems, value);
            }
        }

        private ObservableCollection<MenuModel> _menuItems;
        public ObservableCollection<MenuModel> MenuItems
        {
            get => _menuItems;
            set
            {
                SetProperty(ref _menuItems, value);
            }
        }

        private ObservableCollection<MenuModel> _orderedMenuItems;
        public ObservableCollection<MenuModel> OrderedMenuItems
        {
            get => _orderedMenuItems;
            set => SetProperty(ref _orderedMenuItems, value);
        }

        private MenuModel _selectedMenu;
        public MenuModel SelectedMenu
        {
            get => _selectedMenu;
            set => SetProperty(ref _selectedMenu, value);
        }
        #endregion

        public OrderViewModel()
        {
            InitVariables();
        }

        private void InitVariables()
        {
            CategoryItems = new ObservableCollection<CategoryModel>();
            MenuItems = new ObservableCollection<MenuModel>();
            OrderedMenuItems = new ObservableCollection<MenuModel>();
            SelectedMenu = new MenuModel();
        }

        public void LoadData()
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

        public void IncreaseMenuCount(MenuModel selectedMenu)
        {

        }

        public void RemoveSelectedMenu(MenuModel selectedMenu)
        {
            OrderedMenuItems.Remove(selectedMenu);
        }
    }
}
