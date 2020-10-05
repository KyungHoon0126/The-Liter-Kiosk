using System.Windows;
using TheLiter.Core.Order;
using TheLitter.Core.Place;

namespace THE_LITER_KIOSK
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static OrderData orderData = new OrderData();
        public static PlaceData placeData = new PlaceData();
    }
}
