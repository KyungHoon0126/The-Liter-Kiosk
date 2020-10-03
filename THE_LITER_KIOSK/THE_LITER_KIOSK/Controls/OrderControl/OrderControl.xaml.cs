using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using System.Windows.Threading;
using TheLiter.Core.Order.Model;

namespace THE_LITER_KIOSK.Controls.OrderControl
{
    /// <summary>
    /// Interaction logic for OrderControl.xaml
    /// </summary>
    public partial class OrderControl : UserControl, INotifyPropertyChanged
    {
        int result = 0;

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

        private ObservableCollection<MenuModel> _menus = new ObservableCollection<MenuModel>();
        public ObservableCollection<MenuModel> Menus
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

            // DispatcherPriority.Noraml :  보통 우선 순위로 작업이 처리됩니다. 일반적인 애플리케이션 우선 순위이다.
            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
            {
                // #1
                //for (int i = 0; i < App.orderData.orderViewModel.MenuItems.Count; i++)
                //{
                //    menus.Add(App.orderData.orderViewModel.MenuItems[i].Clone() as MenuModel);
                //}

                // #2
                Menus = App.orderData.orderViewModel.MenuItems;
            }));


            lvCategory.SelectedIndex = 0;
            // SetMenuPage();
        }

        private void SetMenuPage()
        {
            if (lvMenuList.Items.Count > 0)
            {
                App.orderData.orderViewModel.OrderedMenuItems.Clear();
                lvMenuList.ItemsSource = null;
                lvMenuList.ClearValue(ItemsControl.ItemsSourceProperty);
                currentPageIdx = 0;
                itemPerPage = 12;
                totalPage = 0;
            }

            int itemCnt = Menus.Count;
            totalPage = (itemCnt / itemPerPage);
            if (itemCnt % itemPerPage != 0) 
            {
                totalPage += 1;
            }

            CollectionViewSource.Source = Menus;
            CollectionViewSource.Filter += CollectionViewSource_Filter;
            this.lvMenuList.DataContext = CollectionViewSource;
            CollectionViewSource.View.Refresh();
        }

        private void CollectionViewSource_Filter(object sender, FilterEventArgs e)
        {
            int idx = Menus.IndexOf((MenuModel)e.Item);

            if (idx >= itemPerPage * currentPageIdx && idx < itemPerPage * (currentPageIdx + 1))
            {
                e.Accepted = true;
                Debug.WriteLine(((MenuModel)e.Item).Name + e.Accepted);
            }
            else
            {
                e.Accepted = false;
                Debug.WriteLine(((MenuModel)e.Item).Name + e.Accepted);
            }    
        }

        private void lvCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilteringMenuItems(((CategoryModel)lvCategory.SelectedItem).ECategory);
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
                return;
            }
            MessageBox.Show("이미 첫 페이지 입니다.");
        }

        private void btnNextMenu_Click(object sender, RoutedEventArgs e)
        {
            if (currentPageIdx < totalPage - 1)
            {
                currentPageIdx++;
                CollectionViewSource.View.Refresh();
                return;
            }
            MessageBox.Show("마지막 페이지 입니다.");
        }



        private void lvMenuList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MenuModel selectedMenu = (MenuModel)lvMenuList.SelectedItem;
            if (selectedMenu == null)
            {
                return;
            }

            if (IsDuplicateMenu(selectedMenu))
            {
                IncreseMenuCount(selectedMenu);
                AddOrderedFoodItems(selectedMenu);
            }
        }

        // +
        private void btnAddMenu_Click(object sender, RoutedEventArgs e)     
        {
            MenuModel selectedFood = ((ListViewItem)lvMenuList.ContainerFromElement(sender as Button)).Content as MenuModel;
            IncreseMenuCount(selectedFood);
        }

        // -
        private void btnSubMenu_Click(object sender, RoutedEventArgs e)
        {
            MenuModel selectedFood = ((ListViewItem)lvMenuList.ContainerFromElement(sender as Button)).Content as MenuModel;

            if (IsQuantityValid(selectedFood))
            {

                return;
            }
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {

        }

        private bool IsQuantityValid(MenuModel selectedMenu)
        {
            if (selectedMenu.Count == 1)
            {
                return true;
            }
            return false;
        }

        private void IncreseMenuCount(MenuModel selectedMenu)
        {
            selectedMenu.Count++;
            SetTextBlockTotal(selectedMenu, '+');
        }

        private void SetTextBlockTotal(MenuModel selectedFood, char sign)
        {
            switch (sign)
            {
                case '+':
                    tbTotal.Text = Convert.ToString(result += selectedFood.Price);
                    break;
                case '-':
                    tbTotal.Text = Convert.ToString(result -= selectedFood.Price);
                    break;
            }
        }

        private bool IsDuplicateMenu(MenuModel selectedFood)
        {
            for (int i = 0; i < lvOrderList.Items.Count; i++)
            {
                if (selectedFood.Name == (lvOrderList.Items[i] as MenuModel).Name)
                {
                    MessageBox.Show("아래의 수량을 조정해주세요.");
                    return false;
                }
            }
            return true;
        }

        private void AddOrderedFoodItems(MenuModel selectedFood)
        {
            App.orderData.orderViewModel.OrderedMenuItems.Add(selectedFood);
        }


        private void btnClearOrderList_Click(object sender, RoutedEventArgs e)
        {
            ShowCancelPopup("정말 모두 삭제하시겠습니까?", "모두 삭제되었습니다.");
        }

        private void btnOrder_Click(object sender, RoutedEventArgs e)
        {
            if (IsOrderedMenuListValid())
            {
                MessageBox.Show("주문이 완료되었습니다.");
                InitData();
            }
            else
            {
                MessageBox.Show("주문할 음식을 선택해 주세요.");
            }
        }

        private void btnCancelOrder_Click(object sender, RoutedEventArgs e)
        {
            ShowCancelPopup("정말 주문을 취소하시겠습니까?", "주문이 취소되었습니다.");
        }

        private bool IsOrderedMenuListValid()
        {
            return (App.orderData.orderViewModel.OrderedMenuItems.Count > 0) ? true : false;
        }

        private void ShowCancelPopup(string popupMsg, string resultMsg)
        {
            if (IsOrderedMenuListValid())
            {
                MessageBoxResult result = MessageBox.Show(popupMsg, "주문 목록", MessageBoxButton.YesNo);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        MessageBox.Show(resultMsg);
                        InitData();
                        break;
                    case MessageBoxResult.No:
                        break;
                }
            }
        }

        private void InitData()
        {
            foreach (MenuModel menuItem in App.orderData.orderViewModel.OrderedMenuItems)
            {
                menu.Count = 0;
            }
            App.orderData.orderViewModel.OrderedMenuItems.Clear();
            tbTotal.Text = string.Empty;
        }
    }
}
