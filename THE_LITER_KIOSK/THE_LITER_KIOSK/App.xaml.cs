using System.Windows;
using THE_LITER_KIOSK.Common;
using THE_LITER_KIOSK.Network;
using THE_LITER_KIOSK.UIManager;
using TheLiter.Core.Admin;
using TheLiter.Core.Member;
using TheLiter.Core.Order;
using TheLitter.Core.Place;

namespace THE_LITER_KIOSK
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static UIStateManager uIStateManager = new UIStateManager();
        public static string ctrlName = "";

        public static OrderData orderData = new OrderData();
        public static PlaceData placeData = new PlaceData();
        public static AdminData adminData = new AdminData();
        public static MemberData memberData = new MemberData();

        public static int qrIndex = 0;
        public static TcpClient tcpClient = new TcpClient();

        public App()
        {
            DispatcherUnhandledException += (s, e) =>
            {
                MessageBox.Show("An unhandled exception just occured : " + e.Exception, "예외 발생", MessageBoxButton.OK, MessageBoxImage.Error);
                e.Handled = true; 
            };

            Setting.Load();
        }
    }
}
