using Prism.Mvvm;
using System;

namespace THE_LITER_KIOSK.DataBase.Models
{
    public enum PaymentType
    {
        CASH,
        CARD
    }

    public class SalesModel : BindableBase
    {
        private string _category;
        public string Category
        {
            get => _category;
            set => SetProperty(ref _category, value);
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

        private DateTime _payTime;
        public DateTime PayTime
        {
            get => _payTime;
            set => SetProperty(ref _payTime, value);
        }

        private string _payType;
        public string PayType
        {
            get => _payType;
            set => SetProperty(ref _payType, value);
        }

        private int _tableIdx;
        public int TableIdx
        {
            get => _tableIdx;
            set => SetProperty(ref _tableIdx, value);
        }

        private string _memberId;
        public string MemberId
        {
            get => _memberId;
            set => SetProperty(ref _memberId, value);
        }
    }
}
