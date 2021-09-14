namespace JToolbox.Desktop.Dialogs
{
    public class FilterPair
    {
        public string Title { get; set; }
        public string Extensions { get; set; }

        public override string ToString()
        {
            return $"{Title} (*.{Extensions})|*.{Extensions}";
        }
    }
}