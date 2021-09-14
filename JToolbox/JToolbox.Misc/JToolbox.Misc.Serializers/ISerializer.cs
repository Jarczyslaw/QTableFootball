namespace JToolbox.Misc.Serializers
{
    public interface ISerializer
    {
        T FromFile<T>(string filePath);

        T FromString<T>(string input);

        void ToFile<T>(T obj, string filePath);

        string ToString<T>(T val);
    }
}