using JToolbox.Desktop.Dialogs.Resources;
using Ookii.Dialogs.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JToolbox.Desktop.Dialogs
{
    public class DialogsService : IDialogsService
    {
        public string OpenFile(string title, bool checkFileExists = false, string initialDirectory = null, List<FilterPair> filters = null, IntPtr? owner = null)
        {
            var dialog = GetOpenFileDialog();
            SetOpenFileDialogOptions(title, checkFileExists, initialDirectory, filters, dialog);

            if (dialog.ShowDialog(GetOwnerHandle(owner)) == true)
            {
                return dialog.FileName;
            }
            return null;
        }

        public List<string> OpenFiles(string title, bool checkFileExists = false, string initialDirectory = null, List<FilterPair> filters = null, IntPtr? owner = null)
        {
            var dialog = GetOpenFileDialog();
            SetOpenFileDialogOptions(title, checkFileExists, initialDirectory, filters, dialog);

            if (dialog.ShowDialog(GetOwnerHandle(owner)) == true)
            {
                return dialog.FileNames
                    .ToList();
            }
            return null;
        }

        public string OpenFolder(string title, string initialDirectory = null, IntPtr? owner = null)
        {
            var dialog = new VistaFolderBrowserDialog
            {
                ShowNewFolderButton = true,
                SelectedPath = initialDirectory,
                Description = title,
                UseDescriptionForTitle = true,
            };

            if (dialog.ShowDialog(GetOwnerHandle(owner)) == true)
            {
                return dialog.SelectedPath;
            }
            return null;
        }

        public string SaveFile(string title, string initialDirectory = null, string defaultFileName = null, FilterPair filter = null, IntPtr? owner = null)
        {
            var dialog = new VistaSaveFileDialog
            {
                FileName = defaultFileName,
                Filter = CreateFilter(new List<FilterPair> { filter }),
                DefaultExt = filter.Extensions,
                AddExtension = true,
                InitialDirectory = initialDirectory,
                OverwritePrompt = true,
                RestoreDirectory = true,
                Title = title
            };

            if (dialog.ShowDialog(GetOwnerHandle(owner)) == true)
            {
                return dialog.FileName;
            }
            return null;
        }

        public void ShowCriticalException(Exception exception, string message = null, IntPtr? owner = null)
        {
            var targetMessage = GetMessageFromException(exception, message);
            ShowMessageTaskDialog(TaskDialogIcon.Error, Languages.CriticalException, targetMessage, exception.StackTrace, owner);
        }

        public T ShowCustomButtonsQuestion<T>(string question, IEnumerable<CustomButton<T>> customButtons, IntPtr? owner = null)
        {
            using (var dialog = GetTaskDialog())
            {
                dialog.WindowTitle = Languages.Question;
                dialog.MainIcon = TaskDialogIcon.Information;
                dialog.Content = question;
                foreach (var button in customButtons)
                {
                    var btn = new TaskDialogButton(button.Text)
                    {
                        Default = button.Default
                    };
                    dialog.Buttons.Add(btn);
                }
                var clickedButton = dialog.ShowDialog(GetOwnerHandle(owner));
                if (customButtons.Any(s => s.Text == clickedButton.Text))
                {
                    return customButtons.First(s => s.Text == clickedButton.Text).Value;
                }
                return customButtons.First(s => s.Default).Value;
            }
        }

        public void ShowError(string error, string details = null, IntPtr? owner = null)
        {
            ShowMessageTaskDialog(TaskDialogIcon.Error, Languages.Error, error, details, owner);
        }

        public void ShowException(Exception exception, string message = null, IntPtr? owner = null)
        {
            var targetMessage = GetMessageFromException(exception, message);
            ShowMessageTaskDialog(TaskDialogIcon.Error, Languages.Exception, targetMessage, exception.StackTrace, owner);
        }

        public void ShowInfo(string message, string details = null, IntPtr? owner = null)
        {
            ShowMessageTaskDialog(TaskDialogIcon.Information, Languages.Information, message, details, owner);
        }

        public void ShowWarning(string message, string details = null, IntPtr? owner = null)
        {
            ShowMessageTaskDialog(TaskDialogIcon.Warning, Languages.Warning, message, details, owner);
        }

        public bool ShowYesNoQuestion(string question, IntPtr? owner = null)
        {
            using (var dialog = GetTaskDialog())
            {
                dialog.WindowTitle = Languages.Question;
                dialog.MainIcon = TaskDialogIcon.Information;
                dialog.Content = question;
                var yesButton = new TaskDialogButton(ButtonType.Yes);
                dialog.Buttons.Add(yesButton);
                dialog.Buttons.Add(new TaskDialogButton(ButtonType.No));
                return dialog.ShowDialog(GetOwnerHandle(owner)) == yesButton;
            }
        }

        private string GetMessageFromException(Exception exception, string message)
        {
            var targetMessage = exception.Message;
            if (!string.IsNullOrEmpty(message))
            {
                targetMessage = message + ": " + targetMessage;
            }
            return targetMessage;
        }

        private TaskDialog GetTaskDialog()
        {
            return new TaskDialog
            {
                EnableHyperlinks = true,
                CenterParent = true,
                ExpandFooterArea = true,
            };
        }

        private IntPtr GetOwnerHandle(IntPtr? owner)
        {
            if (owner == null)
            {
                return NativeMethods.GetActiveWindow();
            }
            else if (owner.Value == IntPtr.Zero)
            {
                return IntPtr.Zero;
            }
            else
            {
                return owner.Value;
            }
        }

        private void ShowMessageTaskDialog(TaskDialogIcon icon, string title, string message, string details, IntPtr? owner)
        {
            using (var dialog = GetTaskDialog())
            {
                dialog.WindowTitle = title;
                dialog.MainIcon = icon;
                dialog.Content = message;
                dialog.ExpandedInformation = details;
                dialog.ExpandFooterArea = true;
                dialog.Buttons.Add(new TaskDialogButton(ButtonType.Ok));
                dialog.ShowDialog(GetOwnerHandle(owner));
            }
        }

        private VistaOpenFileDialog GetOpenFileDialog()
        {
            return new VistaOpenFileDialog
            {
                AddExtension = true,
                RestoreDirectory = true,
            };
        }

        private string CreateFilter(List<FilterPair> filters)
        {
            return string.Join("|", filters.Select(s => s.ToString()));
        }

        private void SetOpenFileDialogOptions(string title, bool checkFileExists, string initialDirectory, List<FilterPair> filters, VistaOpenFileDialog dialog)
        {
            dialog.Title = title;
            dialog.InitialDirectory = initialDirectory;
            dialog.Multiselect = false;
            dialog.AddExtension = true;
            dialog.CheckFileExists = checkFileExists;
            dialog.CheckPathExists = checkFileExists;
            dialog.Filter = CreateFilter(filters);
            dialog.DefaultExt = filters.First().Extensions;
        }
    }
}