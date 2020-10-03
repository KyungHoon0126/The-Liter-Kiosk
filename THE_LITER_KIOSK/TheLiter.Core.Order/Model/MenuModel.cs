using Prism.Mvvm;
using System;

namespace TheLiter.Core.Order.Model
{
    public class MenuModel : BindableBase, ICloneable
    {
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

        private int _idx;
        public int Idx
        {
            get => _idx;
            set => SetProperty(ref _idx, value);
        }

        public ECategory MenuCategory 
        {
            get;
            set;
        }

        public int TotalPrice
        {
            get
            {
                int total = 0;
                total = Price * Count;
                return total;
            }
        }

        public object Clone()
        {
            return new MenuModel()
            {
                Name = this.Name,
                ImageUrl = this.ImageUrl,
                MenuCategory = this.MenuCategory,
                Price = this.Price,
                Idx = this.Idx,
                Count = this.Count,
            };
        }
    }
}
