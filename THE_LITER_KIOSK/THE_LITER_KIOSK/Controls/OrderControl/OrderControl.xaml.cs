using System.Linq;
using System.Windows;
using System.Windows.Controls;
using THE_LITER_KIOSK.UIManager;
using TheLiter.Core.Order.Model;

namespace THE_LITER_KIOSK.Controls.OrderControl
{
    /// <summary>
    /// Interaction logic for OrderControl.xaml
    /// </summary>
    public partial class OrderControl : CustomControlModel
    {
        public OrderControl()
        {
            InitializeComponent();
            Loaded += OrderControl_Loaded;
        }

        private void OrderControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = App.orderData.orderViewModel;
            // lvCategory.SelectedIndex = 0;
        }

        private void lvCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var path = App.orderData.orderViewModel;

            path.CurrentPageIdx = 1;

            if (lvCategory.SelectedIndex == 0)
                path.CurrentMenuList = path.MenuList;
            else
                path.CurrentMenuList = path.MenuList.Where(x => x.MenuCategory == ((CategoryModel)lvCategory.SelectedItem).ECategory).ToList();
        }

        private void lvMenuList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvMenuList.SelectedItem == null) return;

            SalesModel selectedMenu = (SalesModel)lvMenuList.SelectedItem;

            if (selectedMenu != null && IsDuplicateMenu(selectedMenu))
            {
                IncreaseMenuCount(selectedMenu);
            }
            else
            {
                IncreaseMenuCount(selectedMenu);
                App.orderData.orderViewModel.AddOrderedMenuItems(selectedMenu);
            }

            App.orderData.orderViewModel.IsEnabledOrderAndClearAllMenuBtn();
            lvMenuList.SelectedIndex = -1;
        }

        private bool IsDuplicateMenu(SalesModel selectedMenu)
        {
            return lvOrderList.Items.Cast<SalesModel>().ToList().Where(x => x.Name == selectedMenu.Name).FirstOrDefault() == null ? false : true;
        }

        private void btnAddMenu_Click(object sender, RoutedEventArgs e)
        {
            IncreaseMenuCount(ExtractSelectedMenu(sender));
        }

        private void btnSubMenu_Click(object sender, RoutedEventArgs e)
        {
            var selectedMenu = ExtractSelectedMenu(sender);
            if (App.orderData.orderViewModel.IsQuantityValid(selectedMenu))
            {
                DecreaseMenuCount(selectedMenu);
                App.orderData.orderViewModel.RemoveSelectedMenu(selectedMenu);
                return;
            }
            DecreaseMenuCount(selectedMenu);
        }

        // TODO : Socket 서버 연결안됬을 때 주문정보 안보내도록 고쳐야함
        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            SalesModel selectedMenu = ExtractSelectedMenu(sender);
            App.orderData.orderViewModel.ClearSelectedMenuItems(selectedMenu);
        }

        private void btnClearOrderList_Click(object sender, RoutedEventArgs e)
        {
            ShowCancelPopup("정말 모두 삭제하시겠습니까?", "모두 삭제되었습니다.");
            lvMenuList.SelectedItem = null;
        }

        private void btnOrder_Click(object sender, RoutedEventArgs e)
        {
            if (IsOrderedMenuListValid())
                App.uIStateManager.SwitchCustomControl(CustomControlType.PLACE);
            else
                MessageBox.Show("주문할 음식을 선택해 주세요.");
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
                        App.orderData.InitData();
                        break;
                    case MessageBoxResult.No:
                        break;
                }
            }
        }

        private SalesModel ExtractSelectedMenu(object sender)
        {
            return ((ListViewItem)lvOrderList.ContainerFromElement(sender as Button)).Content as SalesModel;
        }

        private void IncreaseMenuCount(SalesModel selectedMenu)
        {
            App.orderData.orderViewModel.IncreaseMenuCount(selectedMenu);
        }

        private void DecreaseMenuCount(SalesModel selectedMenu)
        {
            App.orderData.orderViewModel.DecreaseMenuCount(selectedMenu);
        }

        private bool IsOrderedMenuListValid()
        {
            return App.orderData.orderViewModel.IsOrderedMenuItemsValid();
        }
    }
}
