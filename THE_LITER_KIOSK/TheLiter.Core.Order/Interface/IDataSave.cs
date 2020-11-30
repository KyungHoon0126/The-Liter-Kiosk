using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheLiter.Core.Order.Model;

namespace TheLiter.Core.Order.Interface
{
    public interface IDataSave
    {
        void GetReceiptIdx();
        void SaveReceiptIdx();
        void SaveSalesInformation(DateTime payTime, string payType, int? tableIdx, string memberId);
        Task<List<SalesModel>> GetAllMenuDisCountRateAndIsSoldOut();
        void SaveMenuDiscountRateAndIsSoldOut();
    }
}
