using System.Collections.Generic;

namespace QTableFootball.Storage
{
    public class StorageContent
    {
        public List<string> Players { get; set; } = new List<string>();
        public int SquadSize { get; set; } = 2;
    }
}