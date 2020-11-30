using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using THE_LITER_KIOSK.Network;

namespace THE_LITER_KIOSK.Controls.AdminControl
{
    /// <summary>
    /// Interaction logic for ChatControl.xaml
    /// </summary>
    public partial class ChatControl : UserControl
    {
        public ChatControl()
        {
            InitializeComponent();
            Loaded += ChatControl_Loaded;
        }

        private void ChatControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = App.adminData.adminViewModel;
            App.adminData.adminViewModel.MemberId = App.memberData.memberViewModel.Id;
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            SendMsg();
        }

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendMsg();
            }
        }

        private void SendMsg()
        {
            if (tbTransMsg.Text.Trim().Length > 0 && tbTransMsg.Text != "" && tbTransMsg.Text != null && TcpHelper.SocketClient.Connected)
            {
                App.networkManager.Send(TcpHelper.SocketClient, App.adminData.adminViewModel.GetMsgArgs());

                spUserChat.Children.Add(new TextBlock()
                {
                    Margin = new Thickness(5),
                    Text =  $"SEND : {tbTransMsg.Text}", 
                    FontSize = 30, 
                    TextWrapping = TextWrapping.Wrap
                });
                
                tbTransMsg.Text = string.Empty;

                spServerChat.Children.Add(new TextBlock()
                {
                    Margin = new Thickness(5),
                    FontSize = 30,
                    Text = $"RECEIVE : {TcpHelper.ReceiveMessage}",
                    TextWrapping = TextWrapping.Wrap
                });
            }
        }
    }
}
