using Prism.Mvvm;

namespace TheLiter.Core.Network.Model
{
    public class MenuModel : BindableBase
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
    }
}
