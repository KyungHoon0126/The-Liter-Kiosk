using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using THE_LITER_KIOSK.DataBase.Models;
using THE_LITER_KIOSK.Network;
using THE_LITER_KIOSK.UIManager;

namespace THE_LITER_KIOSK.Controls.PayControl
{
    /// <summary>
    /// CashCalcControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CashCalcControl : CustomControlModel
    {
        public delegate void OnCompletePayByCashHandler();
        public event OnCompletePayByCashHandler OnCompletePayByCash;

        delegate void SetPayByCashDelegate(int? tableIdx);

        public CashCalcControl()
        {
            InitializeComponent();
            Loaded += CashCalcControl_Loaded;
        }

        private void CashCalcControl_Loaded(object sender, RoutedEventArgs e)
        {
            tbCardNumber.Focus();
            this.DataContext = App.orderData.orderViewModel;
        }

        private void tbCardNumber_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                App.orderData.orderViewModel.BarCode = tbCardNumber.Text;

                if (App.memberData.memberViewModel.BarCode == App.orderData.orderViewModel.BarCode)
                {
                    var selectedTable = App.placeData.tableViewModel.SelectedTable;
                    var payTime = DateTime.Now;
                    var memberId = App.memberData.memberViewModel.Id;

                    SetPayByCashDelegate setDel = (tableIdx) =>
                    {
                        ClearBarCode();
                        SaveSalesInformation(payTime, PaymentType.CASH.ToString(), tableIdx, memberId);

                        if (TcpHelper.isConnected)
                        {
                            App.networkManager.Send(TcpHelper.SocketClient, App.networkManager.SetOrderMsgArgs(App.orderData.orderViewModel.SendOrderInfoToNormal(memberId)));
                            App.networkManager.Send(TcpHelper.SocketClient, App.networkManager.SetGroupMsgArgs(App.orderData.orderViewModel.SendOrderInfoToGroup(memberId, App.orderData.orderViewModel.OrderedMenuItems)));
                        }

                        App.uIStateManager.SwitchCustomControl(CustomControlType.PAYCOMPLETE);
                        OnCompletePayByCash?.Invoke();
                    };

                    if (selectedTable != null)
                    {
                        selectedTable.PayTime = payTime.ToString();
                        setDel(selectedTable.TableIdx);
                    }
                    else
                    {
                        setDel(null);
                    }
                }
                else
                {
                    MessageBox.Show("결제 정보가 일치하지 않습니다");
                    ClearBarCode();
                }
            }
        }

        private void SaveSalesInformation(DateTime payTime, string paymentType, int? tableIdx, string memberId)
        {
            Task.Run(() => { App.orderData.orderViewModel.SaveSalesInformation(payTime, paymentType, tableIdx, memberId); });
        }

        private void ClearBarCode()
        {
            App.orderData.orderViewModel.ClearBarCode();
            tbCardNumber.Text = string.Empty;
        }

        #region UserControl Transition
        private void btnTablePrev_Click(object sender, RoutedEventArgs e)
        {
            App.uIStateManager.SwitchCustomControl(CustomControlType.PAY);
        }
        #endregion
    }
}
