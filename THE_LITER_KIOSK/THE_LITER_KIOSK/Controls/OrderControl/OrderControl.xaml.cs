using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;
using TheLiter.Core.Order.Model;

namespace THE_LITER_KIOSK.Controls.OrderControl
{
    /// <summary>
    /// Interaction logic for OrderControl.xaml
    /// </summary>
    public partial class OrderControl : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Properties
        private CollectionViewSource _collectionViewSource = new CollectionViewSource();
        public CollectionViewSource CollectionViewSource
        {
            get => _collectionViewSource;
            set
            {
                _collectionViewSource = value;
                NotifyPropertyChanged(nameof(CollectionViewSource));
            }
        }

        private ObservableCollection<TheLiter.Core.Order.Model.Menu> _menus = new ObservableCollection<TheLiter.Core.Order.Model.Menu>();
        public ObservableCollection<TheLiter.Core.Order.Model.Menu> Menus
        {
            get => _menus;
            set
            {
                _menus = value;
                NotifyPropertyChanged(nameof(Menus));
            }
        }

        //CollectionViewSource CollectionViewSource = new CollectionViewSource();
        //ObservableCollection<TheLiter.Core.Order.Model.Menu> Menus = new ObservableCollection<TheLiter.Core.Order.Model.Menu>();
        int currentPageIdx = 0;
        int itemPerPage = 12;
        int totalPage = 0;
        #endregion

        public OrderControl()
        {
            InitializeComponent();
            Loaded += OrderControl_Loaded;
        }

        private void OrderControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = App.orderData.orderViewModel;

            // DispatcherPriority.Noraml :  보통 우선 순위로 작업이 처리됩니다. 일반적인 애플리케이션 우선 순위입니다.
            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
            {
                // #1
                //for (int i = 0; i < App.orderData.orderViewModel.MenuItems.Count; i++)
                //{
                //    menus.Add(App.orderData.orderViewModel.MenuItems[i].Clone() as TheLiter.Core.Order.Model.Menu);
                //}

                // #2
                Menus = App.orderData.orderViewModel.MenuItems;
            }));

            SetMenuPage();
        }

        private void SetMenuPage()
        {
            //if (lvMenuList.Items.Count > 0)
            //{
            //    lvMenuList.ItemsSource = null;
            //    lvMenuList.ClearValue(ItemsControl.ItemsSourceProperty);
            //}

            int itemCnt = Menus.Count;
            totalPage = (itemCnt / itemPerPage);
            if (itemCnt % itemPerPage != 0)
            {
                totalPage += 1;
            }

            CollectionViewSource.Source = Menus;
            CollectionViewSource.Filter += CollectionViewSource_Filter;
            this.lvMenuList.DataContext = CollectionViewSource;
        }

        private void CollectionViewSource_Filter(object sender, FilterEventArgs e)
        {
            int idx = Menus.IndexOf((TheLiter.Core.Order.Model.Menu)e.Item);

            if (idx >= itemPerPage * currentPageIdx && idx < itemPerPage * (currentPageIdx + 1))
            {
                e.Accepted = true;
                Debug.WriteLine((e.Item as TheLiter.Core.Order.Model.Menu).Name + " " + e.Accepted);
            }
            else
            {
                e.Accepted = false;
                Debug.WriteLine((e.Item as TheLiter.Core.Order.Model.Menu).Name + e.Accepted);
            }    
        }

        private void lvCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilteringMenuItems(((Category)lvCategory.SelectedItem).ECategory);
        }

        private void FilteringMenuItems(ECategory category)
        {
            if (category == ECategory.ALL)
            {
                SetMenuPage();
                //lvMenuList.ItemsSource = App.orderData.orderViewModel.MenuItems;
            }
            else
            {
                lvMenuList.ItemsSource = App.orderData.orderViewModel.MenuItems.Where(x => x.MenuCategory == category);
            }
        }

        private void btnPreviousMenu_Click(object sender, RoutedEventArgs e)
        {
            if (currentPageIdx > 0)
            {
                currentPageIdx--;
                CollectionViewSource.View.Refresh();
            }
        }

        private void btnNextMenu_Click(object sender, RoutedEventArgs e)
        {
            if (currentPageIdx < totalPage - 1)
            {
                currentPageIdx++;
                CollectionViewSource.View.Refresh();
            }
        }
    }
}
