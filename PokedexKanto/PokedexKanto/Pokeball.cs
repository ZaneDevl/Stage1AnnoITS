using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokedexKanto
{
    public class Pokeball
    {
        public string Name { get; set; }
        public double CaptureRateModifier { get; set; }

        public Pokeball(string name, double captureRateModifier) 
        {
            Name = name;
            CaptureRateModifier = captureRateModifier;
        }
    }
}
