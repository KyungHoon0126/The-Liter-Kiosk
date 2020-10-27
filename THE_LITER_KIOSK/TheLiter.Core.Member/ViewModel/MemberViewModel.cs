using Prism.Commands;
using Prism.Mvvm;
using System.Windows.Input;

namespace TheLiter.Core.Member.ViewModel
{
    public class MemberViewModel : BindableBase
    {
        #region Properties
        private string _id;
        public string Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private string _pw;
        public string Pw
        {
            get => _pw;
            set => SetProperty(ref _pw, value);
        }

        private string _qrCode;
        public string QrCode
        {
            get => _qrCode;
            set => SetProperty(ref _qrCode, value);
        }

        private string _barCode;
        public string BarCode
        {
            get => _barCode;
            set => SetProperty(ref _barCode, value);
        }
        #endregion

        #region Command
        public ICommand SignUpCommand;
        #endregion

        public MemberViewModel()
        {
            SignUpCommand = new DelegateCommand(OnSignUp, CanSignUp);
        }

        private void OnSignUp()
        {

        }

        private bool CanSignUp()
        {
            return true;
        }
    }
}
