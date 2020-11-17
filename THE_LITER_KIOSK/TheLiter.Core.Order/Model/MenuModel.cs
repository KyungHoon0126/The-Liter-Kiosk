using Prism.Mvvm;
using System;

namespace TheLiter.Core.Order.Model
{
    public class MenuModel : BindableBase, ICloneable
    {
        private int _idx;
        public int Idx
        {
            get => _idx;
            set => SetProperty(ref _idx, value);
        }

        private ECategory _menuCategory;
        public ECategory MenuCategory
        {
            get => _menuCategory;
            set => SetProperty(ref _menuCategory, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private int _count;
        public int Count
        {
            get => _count;
            set => SetProperty(ref _count, value);
        }

        private int _price;
        public int Price
        {
            get => _price;
            set => SetProperty(ref _price, value);
        }

        private string _imageUrl;
        public string ImageUrl
        {
            get => _imageUrl;
            set => SetProperty(ref _imageUrl, value);
        }

        private int _totalPrice;
        public int TotalPrice
        {
            get => _totalPrice;
            set => SetProperty(ref _totalPrice, value);
        }

        private int _discountRate;
        public int DiscountRate
        {
            get => _discountRate;
            set => SetProperty(ref _discountRate, value);
        }

        private bool _isSoldOut;
        public bool IsSoldOut
        {
            get => _isSoldOut;
            set => SetProperty(ref _isSoldOut, value);
        }

        public MenuModel Clone(MenuModel item)
        {
            MenuModel menuModel = new MenuModel();
            menuModel.Idx = item.Idx;
            menuModel.MenuCategory = item.MenuCategory;
            menuModel.Name = item.Name;
            menuModel.Count = item.Count;
            menuModel.Price = item.Price;
            menuModel.ImageUrl = item.ImageUrl;
            menuModel.DiscountRate = item.DiscountRate;
            return menuModel;
        }

        public object Clone()
        {
            return new MenuModel()
            {
                Idx = this.Idx,
                MenuCategory = this.MenuCategory,
                Name = this.Name,
                Count = this.Count,
                Price = this.Price,
                ImageUrl = this.ImageUrl,
                DiscountRate = this.DiscountRate
            };
        }
    }
}
