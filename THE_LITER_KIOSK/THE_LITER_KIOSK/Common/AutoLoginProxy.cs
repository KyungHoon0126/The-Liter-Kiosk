using Prism.Mvvm;

namespace THE_LITER_KIOSK.Common
{
    public class AutoLoginProxy : BindableBase
    {
        private bool _isAutoLogin = Setting.IsAutoLogin;
        public bool IsAutoLogin
        {
            get => _isAutoLogin;
            set
            {
                SetProperty(ref _isAutoLogin, value);
                Setting.IsAutoLogin = _isAutoLogin;
                Setting.Save();
            }
        }
    }
}
