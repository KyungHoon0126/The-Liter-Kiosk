using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Prism.Mvvm;
using System.Collections.Generic;

namespace THE_LITER_KIOSK.Network
{
    public class TcpModel:BindableBase
    {
        private int _messageType;
        [JsonProperty("MSGType")]
        public int MessageType
        {
            get => _messageType;
            set => SetProperty(ref _messageType, value);
        }

        private string _id;
        public string Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private string _content = "";
        public string Content
        {
            get => _content;
            set => SetProperty(ref _content, value);
        }

        private string _shopName = "더리터 사이코점";
        public string ShopName
        {
            get => _shopName;
            set => SetProperty(ref _shopName, value);
        }

        private string _orderNumber = "";
        public string OrderNumber
        {
            get => _orderNumber;
            set => SetProperty(ref _orderNumber, value);
        }

        private List<Network.MenuModel> _menuItems;
        [JsonProperty("Menus")]
        public List<Network.MenuModel> MenuItems
        {
            get => _menuItems;
            set => SetProperty(ref _menuItems, value);
        }
    }

    public class MenuModel: BindableBase
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
