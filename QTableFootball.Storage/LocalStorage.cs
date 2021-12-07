using JToolbox.Misc.Serializers;
using System;
using System.IO;

namespace QTableFootball.Storage
{
    public class LocalStorage
    {
        private readonly ISerializer serializer = new SerializerJson();

        public string FilePath { get; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.json");

        public StorageContent Load()
        {
            if (!File.Exists(FilePath))
            {
                return new StorageContent();
            }

            return serializer.FromFile<StorageContent>(FilePath);
        }

        public void Save(StorageContent storage)
        {
            serializer.ToFile(storage, FilePath);
        }
    }
}