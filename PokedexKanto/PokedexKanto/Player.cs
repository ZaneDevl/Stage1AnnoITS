using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokedexKanto
{
    public class Player
    {
        public int PokeDollars { get; set; } 
        public int MegaBallCount { get; set; } 
        public int UltraBallCount { get; set; } 
        public MedalsTracker MedalsTracker {  get; private set; }

        public Player() 
        {
            PokeDollars = 0;
            MegaBallCount = 0;
            UltraBallCount = 0;
            MedalsTracker = new MedalsTracker();
        }
    }
}
