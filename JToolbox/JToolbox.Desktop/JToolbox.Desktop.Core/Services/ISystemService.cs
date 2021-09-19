using System.Windows.Media.Imaging;

namespace JToolbox.Desktop.Core.Services
{
    public interface ISystemService
    {
        void CopyToClipboard(BitmapSource bitmapSource);

        void CopyToClipboard(string data);

        string GetTextFromClipboard();

        void OpenAppLocation();

        void OpenFileLocation(string filePath);

        void OpenFolderLocation(string folderPath);

        void Restart();

        void Shutdown();

        void StartProcess(string process, string arguments = null);

        void StartProcessSilent(string process, string arguments = null);
    }
}