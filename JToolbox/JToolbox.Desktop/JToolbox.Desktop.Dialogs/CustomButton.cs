namespace JToolbox.Desktop.Dialogs
{
    public class CustomButton<T>
    {
        public bool Default { get; set; }
        public string Text { get; set; }
        public T Value { get; set; }
    }
}