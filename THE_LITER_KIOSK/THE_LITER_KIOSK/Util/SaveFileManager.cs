using Microsoft.WindowsAPICodePack.Dialogs;
using THE_LITER_KIOSK.Interface;

namespace THE_LITER_KIOSK.Common
{
    public class SaveFileManager : IStatistics
    {
        public string GetFilePath()
        {
            var directoryPicker = new CommonSaveFileDialog();
            directoryPicker.DefaultExtension = "csv"; 
            AddFileExtensionFilter(ref directoryPicker, new string[] { "csv" }, "엑셀파일");

            if (directoryPicker.ShowDialog() == CommonFileDialogResult.Ok)
            {
                string path = directoryPicker.FileName;
                string[] fileExtension = path.Split(new char[] { '.' });
                
                // 파일 경로의 확장자가 string[2]로 안끝날 때 사용
                if (fileExtension.Length >= 3)
                {
                    string newPath = fileExtension[0] + "." + fileExtension[fileExtension.Length - 1];
                    return newPath;
                }
                else
                {
                    return path;
                }
            }
            return null;
        }

        /// <summary>
        /// ref CommonSaveFileDialog dialog : 참조를 통해 directoryPicker의 인수를 전달.
        /// </summary>
        /// <param name="dialog"></param>
        /// <param name="extension", 확장자></param>
        /// <param name="displayName", 확장자와 함께 표시될 이름></param>
        public void AddFileExtensionFilter(ref CommonSaveFileDialog dialog, string[] extension, string displayName)
        {
            var filter = new CommonFileDialogFilter();

            for (int i = 0; i < extension.Length; i++)
            {
                filter.Extensions.Add(extension[i]);
            }

            filter.DisplayName = displayName;
            dialog.Filters.Add(filter);
        }
    }
}
