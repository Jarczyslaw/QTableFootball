namespace QTableFootball.App.ViewModels
{
    public class SquadViewModel : BaseItemViewModel
    {
        public int Number { get; set; }
        public PlayerViewModel Player1 { get; set; }
        public PlayerViewModel Player2 { get; set; }
    }
}