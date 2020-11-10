using Prism.Mvvm;
using System.Collections.Generic;
using System.Linq;

namespace TheLiter.Core.Order.Model
{
    public enum ECategory
    {
        ALL = 0,
        ADE = 1,
        COFFEE = 2,
        DESERT = 3,
        LATTE = 4,
        LITERCCINO = 5,
        TEA = 6,
        THELITERSPECIAL = 7,
        YOGURS = 8
    }

    public class CategoryModel : BindableBase
    {
        public ECategory ECategory;

        #region Properties
        private string _categoryName;
        public string CategoryName
        {
            get => _categoryName;
            set => SetProperty(ref _categoryName, value);
        }

        public static List<string> EnumItems
        {
            get => ECategory.GetNames(typeof(ECategory)).ToList();
        }
        #endregion
    }
}
