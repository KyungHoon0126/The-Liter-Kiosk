using Microsoft.WindowsAPICodePack.Dialogs;

namespace THE_LITER_KIOSK.Interface
{
    public interface IStatistics
    {
        public string GetFilePath();
        public void AddFileExtensionFilter(ref CommonSaveFileDialog dialog, string[] extension, string displayName);
    }
}
