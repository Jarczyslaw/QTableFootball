using JToolbox.Core;

namespace QTableFootball.App.ViewModels
{
    public class PlayerViewModel : NotifyPropertyChanged
    {
        private bool isSelected;
        private string name;

        public bool IsSelected
        {
            get => isSelected;
            set => Set(ref isSelected, value);
        }

        public string Name
        {
            get => name;
            set => Set(ref name, value);
        }
    }
}