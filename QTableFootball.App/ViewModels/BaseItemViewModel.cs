using JToolbox.Core;

namespace QTableFootball.App.ViewModels
{
    public abstract class BaseItemViewModel : NotifyPropertyChanged
    {
        protected bool isSelected;

        public bool IsSelected
        {
            get => isSelected;
            set => Set(ref isSelected, value);
        }
    }
}