using System;
using System.Windows;
using System.Windows.Controls;
using THE_LITER_KIOSK.UIManager;

namespace THE_LITER_KIOSK.Controls.HomeControl
{
    /// <summary>
    /// HomeControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class HomeControl : CustomControlModel
    {
        public HomeControl()
        {
            InitializeComponent();
        }

        private void btnOrder_Click(object sender, RoutedEventArgs e)
        {
            App.uIStateManager.SwitchCustomControl(CustomControlType.ORDER);
        }
    }
}
