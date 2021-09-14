namespace JToolbox.Desktop.Dialogs
{
    public class CustomButton<T>
    {
        public string Text { get; set; }
        public T Value { get; set; }
        public bool Default { get; set; }
    }
}