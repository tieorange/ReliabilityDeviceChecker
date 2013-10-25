using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reliability
{
    public class Element
    {
        public int Number { get; set; }
        public string Name { get; set; }
        public string Type1 { get; set; }
        public string Type2 { get; set; }
        //public Tuple<double, double> Intensity { get; set; }
        public double Intensity1 { get; set; }
        public double Intensity2 { get; set; }
        public string IntensityText { get; set; }
        public int Coefficient { get; set; }
        
        public Element(int number, string name, string type1, string type2, double intensity1, double intensity2, int coefficient)
        {
            Number = number;
            Name = name;
            Type1 = type1;
            Type2 = type2;
            Intensity1 = intensity1;
            Intensity2 = intensity2;
            Coefficient = coefficient;
            if (Equals(Intensity1.ToString(CultureInfo.InvariantCulture),
                Intensity2.ToString(CultureInfo.InvariantCulture))) //"2"
                IntensityText = Intensity1.ToString(CultureInfo.InvariantCulture);
            else //"2...1.3"
                IntensityText = String.Format(Intensity1 + "..." + Intensity2);
        }
        public Element(string name, string type1, string type2, double intensity1, double intensity2, int coefficient) : this(0, name, type1, type2, intensity1, intensity2, coefficient) { }

        public Element()
        {
        }
        public void IncrementNumber()
        {
            Number++;
        }
    }
}
