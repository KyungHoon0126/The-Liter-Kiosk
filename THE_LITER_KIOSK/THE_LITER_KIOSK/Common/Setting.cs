using THE_LITER_KIOSK.Properties;

namespace THE_LITER_KIOSK.Common
{
    public class Setting
    {
        public static string ServerURL { get; set; }
        public static bool IsAutoLogin { get; set; }

        public static void Load()
        {
            IsAutoLogin = Settings.Default.isAutoLogin;
            App.memberData.memberViewModel.ServerAddress = Settings.Default.serverUrl;
        }

        public static void Save()
        {
            Settings.Default.isAutoLogin = IsAutoLogin;
            Settings.Default.serverUrl = ServerURL;
            Settings.Default.Save();
        }

        public static void SaveUserData(string username)
        {
            Settings.Default.userId = username;
            Settings.Default.userPw = string.Empty;
            Save();
        }

        public static void SaveUserdata(string username, string password)
        {
            Settings.Default.userId = username;
            Settings.Default.userPw = password;
            Save();
        }

        public static string GetUserId()
        {
            return Settings.Default.userId;
        }

        public static string GetUserPw()
        {
            return Settings.Default.userPw;
        }
    }
}
