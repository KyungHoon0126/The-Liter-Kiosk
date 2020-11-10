using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using THE_LITER_KIOSK.UIManager;
using TheLiter.Core.Order.Model;

namespace THE_LITER_KIOSK.Controls.OrderControl
{
    /// <summary>
    /// Interaction logic for OrderControl.xaml
    /// </summary>
    public partial class OrderControl : CustomControlModel, INotifyPropertyChanged
    {
        #region Properties
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private int itemPerPage = 9;
        private List<MenuModel> menuList;
        private List<MenuModel> _currentMenuList;
        private List<MenuModel> CurrentMenuList
        {
            get => _currentMenuList;
            set
            {
                _currentMenuList = value;
                _currentPageIdx = 1;

                paging();

                btnPreviousMenu.IsEnabled = false;
                if (CurrentMenuList.Count > itemPerPage)
                {
                    btnNextMenu.IsEnabled = true;
                }
                else
                {
                    btnNextMenu.IsEnabled = false;
                }
            }
        }


        private ObservableCollection<MenuModel> _pagingMenuList;
        private ObservableCollection<MenuModel> pagingMenuList
        {
            get { return _pagingMenuList; }
            set { _pagingMenuList = value; lvMenuList.ItemsSource = _pagingMenuList; }
        }

        private int _currentPageIdx = 1;
        private int currentPageIdx
        {
            get => _currentPageIdx;
            set
            {
                _currentPageIdx = value;
                btnPreviousMenu.IsEnabled = false;
                btnNextMenu.IsEnabled = false;

                paging();

                if (currentPageIdx > 1)
                {
                    btnPreviousMenu.IsEnabled = true;
                }
                if (CurrentMenuList.Count - (currentPageIdx * itemPerPage) > 0)
                {
                    btnNextMenu.IsEnabled = true;
                }
            }
        }

        private void paging()
        {
            if (CurrentMenuList.Count - (currentPageIdx * itemPerPage - itemPerPage) < itemPerPage && CurrentMenuList.Count - (currentPageIdx * itemPerPage - itemPerPage) > 0)
            {
                pagingMenuList = new ObservableCollection<MenuModel>(CurrentMenuList.GetRange(
                    currentPageIdx * itemPerPage - itemPerPage,
                    CurrentMenuList.Count - (currentPageIdx * itemPerPage - itemPerPage)).ToList());
            }
            else if (CurrentMenuList.Count - (currentPageIdx * itemPerPage - itemPerPage) >= itemPerPage)
            {
                pagingMenuList = new ObservableCollection<MenuModel>(CurrentMenuList.GetRange(
                    currentPageIdx * itemPerPage - itemPerPage, itemPerPage).ToList());
            }
        }
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
                // Menus = App.orderData.orderViewModel.MenuItems;
            }));

            lvCategory.SelectedIndex = 0;
        }

        private void lvCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilteringMenuItems(((CategoryModel)lvCategory.SelectedItem).ECategory);
        }

        private void FilteringMenuItems(ECategory category)
        {
            // TODO : ALL, THELITERSPECIAL 선택 시 페이징 기능 추가.
            if (category == ECategory.ALL)
            {
                lvMenuList.ItemsSource = App.orderData.orderViewModel.MenuItems;
            }
            else
            {
                lvMenuList.ItemsSource = App.orderData.orderViewModel.MenuItems.Where(x => x.MenuCategory == category);
            }
        }

        private void btnPreviousMenu_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("첫 페이지 입니다.");
        }

        private void btnNextMenu_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("마지막 페이지 입니다.");
        }

        private void lvMenuList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvMenuList.SelectedItem == null)
            {
                return;
            }

            MenuModel selectedMenu = (MenuModel)lvMenuList.SelectedItem;

            if (selectedMenu != null && IsDuplicateMenu(selectedMenu))
            {
                IncreaseMenuCount(selectedMenu);
            }
            else
            {
                IncreaseMenuCount(selectedMenu);
                AddOrderedMenuItems(selectedMenu);
            }

            lvMenuList.SelectedIndex = -1;
        }

        private bool IsDuplicateMenu(MenuModel selectedMenu)
        {
            for (int i = 0; i < lvOrderList.Items.Count; i++)
            {
                if (selectedMenu.Name == (lvOrderList.Items[i] as MenuModel).Name)
                {
                    return true;
                }
            }
            return false;
        }

        private void btnAddMenu_Click(object sender, RoutedEventArgs e)     
        {
            IncreaseMenuCount(((ListViewItem)lvOrderList.ContainerFromElement(sender as Button)).Content as MenuModel);
        }

        private void btnSubMenu_Click(object sender, RoutedEventArgs e)
        {
            var selectedMenu = ((ListViewItem)lvOrderList.ContainerFromElement(sender as Button)).Content as MenuModel;
            if (IsQuantityValid(selectedMenu))
            {
                DecreaseMenuCount(selectedMenu);
                RemoveSelectedMenu(selectedMenu);
                return;
            }
            DecreaseMenuCount(selectedMenu);
        }

        private bool IsQuantityValid(MenuModel selectedMenu)
        {
            if (selectedMenu.Count == 1)
            {
                return true;
            }
            return false;
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            MenuModel selectedMenu = ((ListViewItem)lvOrderList.ContainerFromElement(sender as Button)).Content as MenuModel;
            ClearSelectedMenuItems(selectedMenu);
        }

        private void ClearSelectedMenuItems(MenuModel selectedMenu)
        {
            App.orderData.orderViewModel.ClearSelectedMenuItems(selectedMenu);
        }

        private void AddOrderedMenuItems(MenuModel menuModel)
        {
            App.orderData.orderViewModel.AddOrderedMenuItems(menuModel);
        }

        private void IncreaseMenuCount(MenuModel selectedMenu)
        {
            App.orderData.orderViewModel.IncreaseMenuCount(selectedMenu);
        }

        private void DecreaseMenuCount(MenuModel selectedMenu)
        {
            App.orderData.orderViewModel.DecreaseMenuCount(selectedMenu);
        }

        private void RemoveSelectedMenu(MenuModel selectedMenu)
        {
            App.orderData.orderViewModel.RemoveSelectedMenu(selectedMenu);
        }

        private void btnClearOrderList_Click(object sender, RoutedEventArgs e)
        {
            ShowCancelPopup("정말 모두 삭제하시겠습니까?", "모두 삭제되었습니다.");
            lvMenuList.SelectedItem = null;
        }

        private void btnOrder_Click(object sender, RoutedEventArgs e)
        {
            if (IsOrderedMenuListValid())
            {
                App.uIStateManager.SwitchCustomControl(CustomControlType.PLACE);
            }
            else
            {
                MessageBox.Show("주문할 음식을 선택해 주세요.");
            }
        }

        private bool IsOrderedMenuListValid()
        {
            return App.orderData.orderViewModel.IsOrderedMenuListValid();
        }

        private void btnMoveToHome_Click(object sender, RoutedEventArgs e)
        {
            ShowCancelPopup("정말 주문을 취소하시겠습니까?", "주문이 취소되었습니다.");
            App.uIStateManager.SwitchCustomControl(CustomControlType.HOME);
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
            App.orderData.InitData();
        }
    }
}
