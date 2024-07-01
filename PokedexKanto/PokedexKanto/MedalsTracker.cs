using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokedexKanto
{
    public class MedalsTracker
    {
        private HashSet<string> medals;
        public MedalsTracker() 
        {
            medals = new HashSet<string>();
        }

        public void AddMedal(Medal medal, Player player)
        {
            if (medals.Add(medal.Name))
            {
                player.MegaBallCount += medal.MegaBallReward;
                player.UltraBallCount += medal.UltraBallReward;
                player.PokeDollars += medal.PokeDollarReward;
                Console.WriteLine($"Complimenti hai ottenuto la {medal.Name}! ");
                Console.WriteLine($"Hai ottenuto : {medal.PokeDollarReward} Pokedollari, {medal.MegaBallReward} Megaball, {medal.UltraBallReward} Ultraball!");
            }
        }

        public bool HasMedal (string medalName)
        {
            return medals.Contains(medalName);
        }

        public void DisplayMedals()
        {
            Console.WriteLine("Medaglie ottenute: ");
            foreach (var medal in medals)
            {
                Console.WriteLine(medal);
            }
        }
    }
}
