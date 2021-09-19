namespace JToolbox.Desktop.Dialogs
{
    public class FilterPair
    {
        public string Extensions { get; set; }
        public string Title { get; set; }

        public override string ToString()
        {
            return $"{Title} (*.{Extensions})|*.{Extensions}";
        }
    }
}