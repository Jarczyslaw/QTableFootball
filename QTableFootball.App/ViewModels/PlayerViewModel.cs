namespace QTableFootball.App.ViewModels
{
    public class PlayerViewModel : BaseItemViewModel
    {
        private string name;

        public string Name
        {
            get => name;
            set => Set(ref name, value);
        }
    }
}