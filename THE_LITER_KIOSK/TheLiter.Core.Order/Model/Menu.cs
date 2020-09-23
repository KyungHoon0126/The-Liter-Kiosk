using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheLiter.Core.Order.Model
{
    public class Menu : BindableBase
    {
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                SetProperty(ref _name, value);
            }
        }

        private int _count;
        public int Count
        {
            get => _count;
            set
            {
                SetProperty(ref _count, value);
            }
        }

        private int _price;
        public int Price
        {
            get => _price;
            set
            {
                SetProperty(ref _price, value);
            }
        }

        private string _imageUrl;
        public string ImageUrl
        {
            get => _imageUrl;
            set
            {
                SetProperty(ref _imageUrl, value);
            }
        }

        public ECategory MenuCategory { get; set; }
    }
}
