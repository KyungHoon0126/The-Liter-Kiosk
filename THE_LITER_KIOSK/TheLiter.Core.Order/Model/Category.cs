using Prism.Mvvm;

namespace TheLiter.Core.Order.Model
{
    public enum ECategory
    {
        ALL,
        ADE,
        COFFEE,
        DESERT,
        LATTE,
        LITERCCINO,
        TEA,
        THELITERSPECIAL,
        YOGURS
    }

    public class Category : BindableBase
    {
        public ECategory ECategory;

        #region Properties
        private string _categoryName;
        public string CategoryName
        {
            get => _categoryName;
            set
            {
                SetProperty(ref _categoryName, value);
            }
        }
        #endregion
    }
}
