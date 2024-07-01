using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokedexKanto
{
    public class Medal
    {
        public string Name { get; set; }
        public int MegaBallReward { get; set; }
        public int UltraBallReward { get; set;}
        public int PokeDollarReward { get; set; }
        public Medal (string name, int megaBallReward, int ultraBallReward, int pokeDollarReward)
        {
            Name = name;
            MegaBallReward = megaBallReward;
            UltraBallReward = ultraBallReward;
            PokeDollarReward = pokeDollarReward;
        }
    }
}
