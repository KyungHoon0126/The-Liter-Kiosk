using Prism.Mvvm;
using System;

namespace TheLiter.Core.Admin.Model
{
    public class SalesModel : BindableBase
    {
        private int _idx;
        public int Idx
        {
            get => _idx;
            set => SetProperty(ref _idx, value);
        }

        private int receipt_idx;
        public int ReceiptIdx
        {
            get => receipt_idx;
            set => SetProperty(ref receipt_idx, value);
        }

        private string menu_category;
        public string Category
        {
            get => menu_category;
            set => SetProperty(ref menu_category, value);
        }

        private string menu_name;
        public string Name
        {
            get => menu_name;
            set => SetProperty(ref menu_name, value);
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

        private string member_id;
        public string MemberId
        {
            get => member_id;
            set => SetProperty(ref member_id, value);
        }
    }
}
