using JToolbox.Core;
using System;

namespace QTableFootball.App.ViewModels
{
    public abstract class BaseItemViewModel : NotifyPropertyChanged
    {
        protected bool isSelected;

        public event Action OnSelectedChanged;

        public bool IsSelected
        {
            get => isSelected;
            set
            {
                if (Set(ref isSelected, value))
                {
                    OnSelectedChanged?.Invoke();
                }
            }
        }
    }
}