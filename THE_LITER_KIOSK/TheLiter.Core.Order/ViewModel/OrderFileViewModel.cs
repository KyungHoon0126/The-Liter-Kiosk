using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using TheLiter.Core.DBManager;
using TheLiter.Core.Order.Interface;
using TheLiter.Core.Order.Model;

namespace TheLiter.Core.Order.ViewModel
{
    public class OrderFileViewModel : MySqlDBConnectionManager, INotifyPropertyChanged, IDataSave
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void GetReceiptIdx()
        {
            throw new NotImplementedException();
        }
        public void SaveReceiptIdx()
        {
            throw new NotImplementedException();
        }

        public Task<List<SalesModel>> GetAllMenuDisCountRateAndIsSoldOut()
        {
            throw new NotImplementedException();
        }

        public void SaveMenuDiscountRateAndIsSoldOut()
        {
            throw new NotImplementedException();
        }

        public void SaveSalesInformation(DateTime payTime, string payType, int? tableIdx, string memberId)
        {
            throw new NotImplementedException();
        }

        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
