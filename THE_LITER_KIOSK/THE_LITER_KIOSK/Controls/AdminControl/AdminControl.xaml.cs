using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;
using THE_LITER_KIOSK.Controls.AdminControl.Management;
using THE_LITER_KIOSK.UIManager;
using TheLiter.Core.Admin.Model;

namespace THE_LITER_KIOSK.Controls.AdminControl
{
    /// <summary>
    /// Interaction logic for AdminControl.xaml
    /// </summary>
    public partial class AdminControl : CustomControlModel
    {
        public AdminControl()
        {
            InitializeComponent();
            Loaded += AdminControl_Loaded;
        }

        private void AdminControl_Loaded(object sender, RoutedEventArgs e)
        {
            App.memberData.memberViewModel.CompleteAction += MemberViewModel_CompleteAction;
            ctrlMenuManagement.LoadMenuSettingWindow += CtrlMenuManagement_LoadMenuSettingWindow;
            
            App.adminData.LoadData();
            this.DataContext = App.adminData.adminViewModel;

            DispatcherTimer programOperationTimer = new DispatcherTimer();
            programOperationTimer.Interval = TimeSpan.FromSeconds(1);
            programOperationTimer.Tick += ProgramOperationTimer_Tick;
            programOperationTimer.Start();
        }

        private void MemberViewModel_CompleteAction(object sender, bool success)
        {
            if (success)
            {
                progressRing.IsActive = !success;
            }
            else
            {
                progressRing.IsActive = !success;
            }
        }

        private void CtrlMenuManagement_LoadMenuSettingWindow(object sender, RoutedEventArgs e)
        {
            MenuSettingWindow menuSettingWindow = new MenuSettingWindow();
            menuSettingWindow.ShowDialog();
        }

        private void ProgramOperationTimer_Tick(object sender, EventArgs e)
        {
            var admin = App.adminData.adminViewModel;
            admin.OperationTimeDesc = (admin.OperationTime - new DateTime(0001, 01, 01, 00, 00, 00)).ToString();
        }
    }
}
