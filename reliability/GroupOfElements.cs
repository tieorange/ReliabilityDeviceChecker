using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reliability
{
    public class GroupOfElements
    {
        public Element Element { get; set; }
        public int Count { get; set; }
        public GroupOfElements(Element element, int count)
        {
            Element = element;
            Count = count;
        }
    }
}
