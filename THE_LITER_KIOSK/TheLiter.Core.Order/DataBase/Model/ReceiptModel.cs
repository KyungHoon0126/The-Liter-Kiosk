using Prism.Mvvm;

namespace TheLiter.Core.Order.DataBase.Model
{
    public class ReceiptModel : BindableBase
    {
        private int receipt_idx;
        public int ReceiptIdx
        {
            get => receipt_idx;
            set => SetProperty(ref receipt_idx, value);
        }
    }
}
