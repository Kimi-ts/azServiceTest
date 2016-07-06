using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Audio
    {
        public string Name { get; set; }
        public string Src { get; set; }
        public int Plays { get; set; }
        public int Skips { get; set; }
    }
}
