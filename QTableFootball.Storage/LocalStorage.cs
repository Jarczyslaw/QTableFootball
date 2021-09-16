using JToolbox.Misc.Serializers;
using System;
using System.Collections.Generic;
using System.IO;

namespace QTableFootball.Storage
{
    public class LocalStorage
    {
        private readonly ISerializer serializer = new SerializerJson();

        public string FilePath { get; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "players.json");

        public List<string> Load()
        {
            if (!File.Exists(FilePath))
            {
                return new List<string>();
            }

            return serializer.FromFile<List<string>>(FilePath);
        }

        public void Save(List<string> Players)
        {
            serializer.ToFile(Players, FilePath);
        }
    }
}