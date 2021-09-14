using System;
using System.Collections.Generic;

namespace JToolbox.Desktop.Dialogs
{
    public interface IDialogsService
    {
        void ShowInfo(string message, string details = null, IntPtr? owner = null);

        void ShowWarning(string message, string details = null, IntPtr? owner = null);

        void ShowError(string error, string details = null, IntPtr? owner = null);

        void ShowException(Exception exception, string message = null, IntPtr? owner = null);

        void ShowCriticalException(Exception exception, string message = null, IntPtr? owner = null);

        bool ShowYesNoQuestion(string question, IntPtr? owner = null);

        T ShowCustomButtonsQuestion<T>(string question, IEnumerable<CustomButton<T>> customButtons, IntPtr? owner = null);

        string OpenFile(string title, bool checkFileExists = false, string initialDirectory = null, List<FilterPair> filters = null, IntPtr? owner = null);

        List<string> OpenFiles(string title, bool checkFileExists = false, string initialDirectory = null, List<FilterPair> filters = null, IntPtr? owner = null);

        string OpenFolder(string title, string initialDirectory = null, IntPtr? owner = null);

        string SaveFile(string title, string initialDirectory = null, string defaultFileName = null, FilterPair filter = null, IntPtr? owner = null);
    }
}