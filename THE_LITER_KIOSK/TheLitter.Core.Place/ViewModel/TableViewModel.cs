using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TheLiter.Core.Order.Model;
using TheLiter.Core.Order.ViewModel;
using TheLitter.Core.Place.Model;

namespace TheLitter.Core.Place.ViewModel
{
    public class TableViewModel: BindableBase
    {
        OrderViewModel orderViewModel = new OrderViewModel();

        #region Properties
        private bool _isActive;
        public bool IsActive
        {
            get => _isActive;
            set => SetProperty(ref _isActive, value);
        }

        private ObservableCollection<TableModel> _tableItems = new ObservableCollection<TableModel>();
        public ObservableCollection<TableModel> TableItems
        {
            get => _tableItems;
            set => SetProperty(ref _tableItems, value);
        }

        private TableModel _selectedTable;
        public TableModel SelectedTable
        {
            get => _selectedTable;
            set => SetProperty(ref _selectedTable, value);
        }
        #endregion

        public async void LoadTableData()
        {
            await LoadTableDataAysnc();
        }

        public async Task LoadTableDataAysnc()
        {
            await Task.Run(() =>
            {
                #region TableItems
                var menus = new List<MenuModel>(orderViewModel.MenuItems);
                TableItems.Add(new TableModel()
                {
                    TableIdx = 1,
                    TotalPrice = 0,
                    IsUsed=false,
                });
                TableItems.Add(new TableModel()
                {
                    TableIdx = 2,
                    TotalPrice = 0,             
                    IsUsed=false,
                });
                TableItems.Add(new TableModel()
                {
                    TableIdx = 3,
                    TotalPrice = 0,
                    IsUsed=false,
                });
                TableItems.Add(new TableModel()
                {
                    TableIdx = 4,
                    TotalPrice = 0, 
                    IsUsed=false,
                });
                TableItems.Add(new TableModel()
                {
                    TableIdx = 5,
                    TotalPrice = 0,
                    IsUsed=false,
                });
                TableItems.Add(new TableModel()
                {
                    TableIdx = 6,
                    TotalPrice = 0,
                   IsUsed=false,
                });
                TableItems.Add(new TableModel()
                {
                    TableIdx = 7,
                    TotalPrice = 0,
                    IsUsed=false,
                });
                TableItems.Add(new TableModel()
                {
                    TableIdx = 8,
                    TotalPrice = 0,
                    IsUsed=false,
                });
                TableItems.Add(new TableModel()
                {
                    TableIdx = 9,
                    TotalPrice = 0,
                    IsUsed=false,
                });
                #endregion
            });
        }
    }
}
