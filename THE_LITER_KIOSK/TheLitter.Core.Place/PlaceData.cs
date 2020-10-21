using TheLitter.Core.Place.ViewModel;

namespace TheLitter.Core.Place
{
    public class PlaceData
    {
       public TableViewModel tableViewModel = new TableViewModel();
       
       public void LoadTableData()
       {
           tableViewModel.LoadTableData();
       }
    }
}
