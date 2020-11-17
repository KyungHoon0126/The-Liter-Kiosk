using System.Windows;
using System.Windows.Controls;

namespace THE_LITER_KIOSK.Controls.AdminControl
{
    /// <summary>
    /// Interaction logic for MemberManagementControl.xaml
    /// </summary>
    public partial class MemberManagementControl : UserControl
    {
        public MemberManagementControl()
        {
            InitializeComponent();
            Loaded += MemberManagementControl_Loaded;
        }

        private void MemberManagementControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = App.memberData.memberViewModel;
        }
    }
}
