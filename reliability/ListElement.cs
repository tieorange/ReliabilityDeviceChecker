using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reliability
{
    public class ListElement
    {
        public ListElement()
        {
        }

        public string Name { get; set; }
        public List<ListType1> Type1s;

        public ListElement(string name) : this(null, name)
        {
        }

        public ListElement(List<ListType1> type1S, string name)
        {
            Type1s = type1S;
            Name = name;
        }
    }
}
