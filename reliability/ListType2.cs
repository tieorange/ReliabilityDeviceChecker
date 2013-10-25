using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reliability
{
    public class ListType2
    {
        public ListType2()
        {
        }

        public string Name { get; set; }
        public double Intensity1 { get; set; }
        public double Intensity2 { get; set; }

        public ListType2(string name) : this(name, 0, 0)
        {
        }

        public ListType2(string name, double intensity1, double intensity2)
        {
            Name = name;
            Intensity1 = intensity1;
            Intensity2 = intensity2;
        }
    }
}
