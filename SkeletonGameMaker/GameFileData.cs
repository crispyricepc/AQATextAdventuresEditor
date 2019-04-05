using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkeletonGameMaker
{
    public class GameFileData
    {
        public string Path { get; }
        public string Name { get; }
        public DateTime LastEdited { get; }

        public GameFileData(string path, string name, DateTime lastEdited)
        {
            Path = path;
            Name = name;
            LastEdited = lastEdited;
        }
    }
}
