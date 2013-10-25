using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reliability
{
    public class ListType1
    {
        public ListType1()
        {
        }

        public string Name { get; set; }
        public double Intensity1 { get; set; }
        public double Intensity2 { get; set; }
        public List<ListType2> Type2s;

        //коли немає типу2
        public ListType1(string name, double intensity1, double intensity2) : this(null, name, intensity1, intensity2) { }

        public ListType1(string name) : this(null, name, 0, 0) { }
        public ListType1(List<ListType2> type2S, string name) : this(type2S, name, 0, 0) { }

        public ListType1(List<ListType2> type2S, string name, double intensity1, double intensity2)
        {
            Type2s = type2S;
            Name = name;
            Intensity1 = intensity1;
            Intensity2 = intensity2;
        }
    }
}
