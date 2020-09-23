using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using THE_LITER_KIOSK.Common;
using TheLiter.Core.Order.Model;

namespace TheLiter.Core.Order.ViewModel
{
    public class OrderViewModel : BindableBase
    {
        #region Properties
        private ObservableCollection<Category> _categoryItems;
        public ObservableCollection<Category> CategoryItems
        {
            get => _categoryItems;
            set
            {
                SetProperty(ref _categoryItems, value);
            }
        }

        private ObservableCollection<Menu> _menuItems;
        public ObservableCollection<Menu> MenuItems
        {
            get => _menuItems;
            set
            {
                SetProperty(ref _menuItems, value);
            }
        }
        #endregion

        public OrderViewModel()
        {
            InitVariables();
        }

        private void InitVariables()
        {
            CategoryItems = new ObservableCollection<Category>();
            MenuItems = new ObservableCollection<Menu>();
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
                            }
                        );
        }

        private async Task LoadCategoryDataAsync()
        {
            await Task.Run(() =>
            {
                #region Categories
                CategoryItems.Add(new Category()
                {
                    CategoryName = "ALL",
                    ECategory = ECategory.ALL
                });
                CategoryItems.Add(new Category()
                {
                    CategoryName = "ADE",
                    ECategory = ECategory.ADE
                });
                CategoryItems.Add(new Category()
                {
                    CategoryName = "COFFEE",
                    ECategory = ECategory.COFFEE
                });
                CategoryItems.Add(new Category()
                {
                    CategoryName = "DESERT",
                    ECategory = ECategory.DESERT
                });
                CategoryItems.Add(new Category()
                {
                    CategoryName = "LATTE",
                    ECategory = ECategory.LATTE
                });
                CategoryItems.Add(new Category()
                {
                    CategoryName = "LITERCCINO",
                    ECategory = ECategory.LITERCCINO
                });
                CategoryItems.Add(new Category()
                {
                    CategoryName = "TEA",
                    ECategory = ECategory.TEA
                });
                CategoryItems.Add(new Category()
                {
                    CategoryName = "THELITERSPECIAL",
                    ECategory = ECategory.THELITERSPECIAL
                });
                CategoryItems.Add(new Category()
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
                MenuItems.Add(new Menu()
                {
                    Name = "CitronAde",
                    ImageUrl = ComDef.Path + "/Ade/CitronAde.jpg",
                    MenuCategory = ECategory.ADE,
                    Price = 1000
                });
                MenuItems.Add(new Menu()
                {
                    Name = "GrapefruitAde",
                    ImageUrl = ComDef.Path + "/Ade/GrapefruitAde.jpg",
                    MenuCategory = ECategory.ADE,
                    Price = 1000
                });
                MenuItems.Add(new Menu()
                {
                    Name = "GreengrapeAde",
                    ImageUrl = ComDef.Path + "/Ade/GreengrapeAde.jpg",
                    MenuCategory = ECategory.ADE,
                    Price = 1000
                });
                MenuItems.Add(new Menu()
                {
                    Name = "LemonAde",
                    ImageUrl = ComDef.Path + "/Ade/LemonAde.jpg",
                    MenuCategory = ECategory.ADE,
                    Price = 1000
                });
                MenuItems.Add(new Menu()
                {
                    Name = "MojitoAde",
                    ImageUrl = ComDef.Path + "/Ade/MojitoAde.jpg",
                    MenuCategory = ECategory.ADE,
                    Price = 1000
                });
                #endregion

                #region Coffee
                MenuItems.Add(new Menu()
                {
                    Name = "Americano",
                    ImageUrl = ComDef.Path + "/Coffee/Americano.jpg",
                    MenuCategory = ECategory.ADE,
                    Price = 1000
                });
                MenuItems.Add(new Menu()
                {
                    Name = "Cafelatte",
                    ImageUrl = ComDef.Path + "/Coffee/Cafelatte.jpg",
                    MenuCategory = ECategory.COFFEE,
                    Price = 1000
                });
                MenuItems.Add(new Menu()
                {
                    Name = "CafeMocha",
                    ImageUrl = ComDef.Path + "/Coffee/CafeMocha.jpg",
                    MenuCategory = ECategory.COFFEE,
                    Price = 1000
                });
                MenuItems.Add(new Menu()
                {
                    Name = "Cappuccino",
                    ImageUrl = ComDef.Path + "/Coffee/Cappuccino.jpg",
                    MenuCategory = ECategory.COFFEE,
                    Price = 1000
                });
                MenuItems.Add(new Menu()
                {
                    Name = "CaramelMacchiato",
                    ImageUrl = ComDef.Path + "/Coffee/CaramelMacchiato.jpg",
                    MenuCategory = ECategory.COFFEE,
                    Price = 1000
                });
                MenuItems.Add(new Menu()
                {
                    Name = "ColdBrew",
                    ImageUrl = ComDef.Path + "/Coffee/ColdBrew.jpg",
                    MenuCategory = ECategory.COFFEE,
                    Price = 1000
                });
                MenuItems.Add(new Menu()
                {
                    Name = "ColdBrewLatte",
                    ImageUrl = ComDef.Path + "/Coffee/ColdBrewLatte.jpg",
                    MenuCategory = ECategory.COFFEE,
                    Price = 1000
                });
                MenuItems.Add(new Menu()
                {
                    Name = "CondensedMilkLatte",
                    ImageUrl = ComDef.Path + "/Coffee/CondensedMilkLatte.jpg",
                    MenuCategory = ECategory.COFFEE,
                    Price = 1000
                });
                MenuItems.Add(new Menu()
                {
                    Name = "HazelnutLatte",
                    ImageUrl = ComDef.Path + "/Coffee/HazelnutLatte.jpg",
                    MenuCategory = ECategory.COFFEE,
                    Price = 1000
                });
                MenuItems.Add(new Menu()
                {
                    Name = "TiramisuLatte",
                    ImageUrl = ComDef.Path + "/Coffee/TiramisuLatte.jpg",
                    MenuCategory = ECategory.COFFEE,
                    Price = 1000
                });
                MenuItems.Add(new Menu()
                {
                    Name = "vanillaLatte",
                    ImageUrl = ComDef.Path + "/Coffee/vanillaLatte.jpg",
                    MenuCategory = ECategory.COFFEE,
                    Price = 1000
                });
                #endregion

                #region Desert
                MenuItems.Add(new Menu()
                {
                    Name = "Cookie",
                    ImageUrl = ComDef.Path + "/Desert/Cookie.jpg",
                    MenuCategory = ECategory.DESERT,
                    Price = 1000
                });
                MenuItems.Add(new Menu()
                {
                    Name = "CurstardStick",
                    ImageUrl = ComDef.Path + "/Desert/CurstardStick.jpg",
                    MenuCategory = ECategory.DESERT,
                    Price = 1000
                });
                MenuItems.Add(new Menu()
                {
                    Name = "HeartPie",
                    ImageUrl = ComDef.Path + "/Desert/HeartPie.jpg",
                    MenuCategory = ECategory.DESERT,
                    Price = 1000
                });
                MenuItems.Add(new Menu()
                {
                    Name = "MuffinSandwich",
                    ImageUrl = ComDef.Path + "/Desert/MuffinSandwich.jpg",
                    MenuCategory = ECategory.DESERT,
                    Price = 1000
                });
                MenuItems.Add(new Menu()
                {
                    Name = "Waffle",
                    ImageUrl = ComDef.Path + "/Desert/Waffle.jpg",
                    MenuCategory = ECategory.DESERT,
                    Price = 1000
                });
                #endregion

                #region Latte
                MenuItems.Add(new Menu()
                {
                    Name = "DeepChocoLatte",
                    ImageUrl = ComDef.Path + "/Latte/DeepChocoLatte.jpg",
                    MenuCategory = ECategory.LATTE,
                    Price = 1000
                });
                MenuItems.Add(new Menu()
                {
                    Name = "GreenteaLatte",
                    ImageUrl = ComDef.Path + "/Latte/GreenteaLatte.jpg",
                    MenuCategory = ECategory.LATTE,
                    Price = 1000
                });
                MenuItems.Add(new Menu()
                {
                    Name = "MilkTea",
                    ImageUrl = ComDef.Path + "/Latte/MilkTea.jpg",
                    MenuCategory = ECategory.LATTE,
                    Price = 1000
                });
                MenuItems.Add(new Menu()
                {
                    Name = "MintChocoLatte",
                    ImageUrl = ComDef.Path + "/Latte/MintChocoLatte.jpg",
                    MenuCategory = ECategory.LATTE,
                    Price = 1000
                });
                MenuItems.Add(new Menu()
                {
                    Name = "PurplesweetpotatoLatte",
                    ImageUrl = ComDef.Path + "/Latte/PurplesweetpotatoLatte.jpg",
                    MenuCategory = ECategory.LATTE,
                    Price = 1000
                });
                #endregion

                #region Literccino
                MenuItems.Add(new Menu()
                {
                    Name = "CookieCreamLiterccino",
                    ImageUrl = ComDef.Path + "/Literccino/CookieCreamLiterccino.jpg",
                    MenuCategory = ECategory.LITERCCINO,
                    Price = 1000
                });
                MenuItems.Add(new Menu()
                {
                    Name = "DoublechocoLiterccino",
                    ImageUrl = ComDef.Path + "/Literccino/DoublechocoLiterccino.jpg",
                    MenuCategory = ECategory.LITERCCINO,
                    Price = 1000
                });
                MenuItems.Add(new Menu()
                {
                    Name = "GreenteaLiterccino",
                    ImageUrl = ComDef.Path + "/Literccino/GreenteaLiterccino.jpg",
                    MenuCategory = ECategory.LITERCCINO,
                    Price = 1000
                });
                MenuItems.Add(new Menu()
                {
                    Name = "JavachipLiterccino",
                    ImageUrl = ComDef.Path + "/Literccino/JavachipLiterccino.jpg",
                    MenuCategory = ECategory.LITERCCINO,
                    Price = 1000
                });
                MenuItems.Add(new Menu()
                {
                    Name = "MintChocochipLiterccino",
                    ImageUrl = ComDef.Path + "/Literccino/MintChocochipLiterccino.jpg",
                    MenuCategory = ECategory.LITERCCINO,
                    Price = 1000
                });
                #endregion

                #region Tea
                MenuItems.Add(new Menu()
                {
                    Name = "FruitTea",
                    ImageUrl = ComDef.Path + "/Tea/FruitTea.jpg",
                    MenuCategory = ECategory.TEA,
                    Price = 1000
                });
                MenuItems.Add(new Menu()
                {
                    Name = "HerbTea",
                    ImageUrl = ComDef.Path + "/Tea/HerbTea.jpg",
                    MenuCategory = ECategory.TEA,
                    Price = 1000
                });
                #endregion

                #region TheLiterSpecial
                MenuItems.Add(new Menu()
                {
                    Name = "Bananalatte",
                    ImageUrl = ComDef.Path + "/TheLiterSpecial/Bananalatte.jpg",
                    MenuCategory = ECategory.THELITERSPECIAL,
                    Price = 1000
                });
                MenuItems.Add(new Menu()
                {
                    Name = "C1Soda",
                    ImageUrl = ComDef.Path + "/TheLiterSpecial/C1Soda.jpg",
                    MenuCategory = ECategory.THELITERSPECIAL,
                    Price = 1000
                });
                MenuItems.Add(new Menu()
                {
                    Name = "ChamMelon",
                    ImageUrl = ComDef.Path + "/TheLiterSpecial/ChamMelon.jpg",
                    MenuCategory = ECategory.THELITERSPECIAL,
                    Price = 1000
                });
                MenuItems.Add(new Menu()
                {
                    Name = "Grainlatte",
                    ImageUrl = ComDef.Path + "/TheLiterSpecial/Grainlatte.jpg",
                    MenuCategory = ECategory.THELITERSPECIAL,
                    Price = 1000
                });
                MenuItems.Add(new Menu()
                {
                    Name = "PeachSoongsoong",
                    ImageUrl = ComDef.Path + "/TheLiterSpecial/PeachSoongsoong.jpg",
                    MenuCategory = ECategory.THELITERSPECIAL,
                    Price = 1000
                });
                MenuItems.Add(new Menu()
                {
                    Name = "PineappleSoda",
                    ImageUrl = ComDef.Path + "/TheLiterSpecial/PineappleSoda.jpg",
                    MenuCategory = ECategory.THELITERSPECIAL,
                    Price = 1000
                });
                MenuItems.Add(new Menu()
                {
                    Name = "ShiningSuger",
                    ImageUrl = ComDef.Path + "/TheLiterSpecial/ShiningSuger.jpg",
                    MenuCategory = ECategory.THELITERSPECIAL,
                    Price = 1000
                });
                MenuItems.Add(new Menu()
                {
                    Name = "StrawberrySoksok",
                    ImageUrl = ComDef.Path + "/TheLiterSpecial/StrawberrySoksok.jpg",
                    MenuCategory = ECategory.THELITERSPECIAL,
                    Price = 1000
                });
                MenuItems.Add(new Menu()
                {
                    Name = "sugarlatte",
                    ImageUrl = ComDef.Path + "/TheLiterSpecial/sugarlatte.jpg",
                    MenuCategory = ECategory.THELITERSPECIAL,
                    Price = 1000
                });
                MenuItems.Add(new Menu()
                {
                    Name = "sugarmilktea",
                    ImageUrl = ComDef.Path + "/TheLiterSpecial/sugarmilktea.jpg",
                    MenuCategory = ECategory.THELITERSPECIAL,
                    Price = 1000
                });
                #endregion

                #region Yogurs
                MenuItems.Add(new Menu()
                {
                    Name = "BlueberryYogurs",
                    ImageUrl = ComDef.Path + "/Yogurs/BlueberryYogurs.jpg",
                    MenuCategory = ECategory.YOGURS,
                    Price = 1000
                });
                MenuItems.Add(new Menu()
                {
                    Name = "CitronYogurs",
                    ImageUrl = ComDef.Path + "/Yogurs/CitronYogurs.jpg",
                    MenuCategory = ECategory.YOGURS,
                    Price = 1000
                });
                MenuItems.Add(new Menu()
                {
                    Name = "MangoYogurs",
                    ImageUrl = ComDef.Path + "/Yogurs/MangoYogurs.jpg",
                    MenuCategory = ECategory.YOGURS,
                    Price = 1000
                });
                MenuItems.Add(new Menu()
                {
                    Name = "PeachYogurs",
                    ImageUrl = ComDef.Path + "/Yogurs/PeachYogurs.jpg",
                    MenuCategory = ECategory.YOGURS,
                    Price = 1000
                });
                MenuItems.Add(new Menu()
                {
                    Name = "PlainYogurs",
                    ImageUrl = ComDef.Path + "/Yogurs/PlainYogurs.jpg",
                    MenuCategory = ECategory.YOGURS,
                    Price = 1000
                });
                MenuItems.Add(new Menu()
                {
                    Name = "StrawberryYogurs",
                    ImageUrl = ComDef.Path + "/Yogurs/StrawberryYogurs.jpg",
                    MenuCategory = ECategory.YOGURS,
                    Price = 1000
                });
                #endregion
                #endregion
            });
        }
    }
}
