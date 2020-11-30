using Prism.Mvvm;
using System;

namespace TheLiter.Core.Network.Model
{
    public class SalesModel : BindableBase
    {
        private int idx;
        public int Idx
        {
            get => idx;
            set => SetProperty(ref idx, value);
        }

        private string menuCategory;
        public string Category
        {
            get => menuCategory;
            set => SetProperty(ref menuCategory, value);
        }

        private string menuName;
        public string Name
        {
            get => menuName;
            set => SetProperty(ref menuName, value);
        }

        private int count;
        public int Count
        {
            get => count;
            set => SetProperty(ref count, value);
        }

        private int discountTotalPrice;
        public int DiscountTotalPrice
        {
            get => discountTotalPrice;
            set => SetProperty(ref discountTotalPrice, value);
        }

        private DateTime payTime;
        public DateTime PayTime
        {
            get => payTime;
            set => SetProperty(ref payTime, value);
        }

        private string payType;
        public string PayType
        {
            get => payType;
            set => SetProperty(ref payType, value);
        }

        private int _tableIdx;
        public int TableIdx
        {
            get => _tableIdx;
            set => SetProperty(ref _tableIdx, value);
        }

        private string memberId;
        public string MemberId
        {
            get => memberId;
            set => SetProperty(ref memberId, value);
        }

        private int receiptIdx;
        public int ReceiptIdx
        {
            get => receiptIdx;
            set => SetProperty(ref receiptIdx, value);
        }

        private int discountAmount;
        public int DiscountAmount
        {
            get => discountAmount;
            set => SetProperty(ref discountAmount, value);
        }

        private int totalPrice;
        public int TotalPrice
        {
            get => totalPrice;
            set => SetProperty(ref totalPrice, value);
        }
    }
}
