using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Numerics;

namespace PokedexKanto
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Pokemon> pokedex = CreatePokedex(); 
            List<Pokemon> capturedPokemon = new List<Pokemon>();
            HashSet<int> uniquecapturedPokemonIds = new HashSet<int>();
            Player player = new Player();
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("Benvenuto nel Pokèdex di Kanto!");
                Console.WriteLine("1. Visualizza il Pokèdex completo.");
                Console.WriteLine("2. Cerca il Pokèmon in base al numero di Pokèdex.");
                Console.WriteLine("3. Cattura un Pokèmon");
                Console.WriteLine("4. Visualizza Pokèmon catturati");
                Console.WriteLine("5. Valutazione Pokèdex");
                Console.WriteLine("6. Visita PokèMarket");
                Console.WriteLine("7. Visualizza medaglie.");
                Console.WriteLine("8. Esci dal Pokèdex.");
                Console.WriteLine("Scegli un opzione.");

                string input = Console.ReadLine();
                
                switch (input)
                {
                    case "1":
                        Console.WriteLine("Caricamento del Pokèdex in corso...");
                        DisplayAllPokemon(pokedex, uniquecapturedPokemonIds); 
                        break;
                    case "2":
                        Console.WriteLine("Cerca un Pokèmon inserendo il suo numero di Pokèdex:");
                        SearchPokemonById(pokedex);
                        break;
                    case "3":
                        Console.WriteLine("Hai scelto di entrare nell'erba alta...");
                        CapturePokemon(pokedex, capturedPokemon, uniquecapturedPokemonIds, player);
                        break;
                    case "4":
                        Console.WriteLine("Stai aprendo il Pc di Bill.");
                        DisplayCapturedPokemon(capturedPokemon);
                        break;
                    case "5":
                        Console.WriteLine("Il Prof. Oak dice che...");
                        EvaluatePokedex(uniquecapturedPokemonIds);
                        break;
                    case "6":
                        Console.WriteLine("Hai scelto di visitare il PokèMarket...");
                        VisitPokeMarket(player);
                        break;
                    case "7":
                        Console.WriteLine("Stai aprendo la Scheda Allenatore...");
                        player.MedalsTracker.DisplayMedals();
                        break;
                    case "8":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Opzione non valida. Riprova.");
                    break;
                }
                Console.WriteLine();
            }
        }
        static List<Pokemon> CreatePokedex()
        {
            return new List<Pokemon>
            {
                new Pokemon { Id = 1, Name = "Bulbasaur", Type = "Grass/Poison", Ability = "Overgrow", Category = "Seed", Height = 0.7f, Weight = 6.9f, Description = "Bulbasaur può essere visto schiacciare un pisolino al sole. Nella sacca di seme sulla schiena, immagazzina energia."},
                new Pokemon { Id = 2, Name = "Ivysaur", Type = "Grass/Poison", Ability = "Overgrow", Category = "Seed", Height = 1.0f, Weight = 13.0f, Description = "Ivysaur si nasconde nei boschi densi dove ha la possibilità di raccogliere più luce solare." },
                new Pokemon { Id = 3, Name = "Venusaur", Type = "Grass/Poison", Ability = "Overgrow", Category = "Seed", Height = 2.0f, Weight = 100.0f, Description = "Venusaur può assorbire la luce solare e usarla per la fotosintesi. Può causare enormi danni se si eccita." },
                new Pokemon { Id = 4, Name = "Charmander", Type = "Fire", Ability = "Blaze", Category = "Lizard", Height = 0.6f, Weight = 8.5f, Description = "Charmander preferisce luoghi caldi. Quando piove, il vapore sgorga dalla punta della coda." },
                new Pokemon { Id = 5, Name = "Charmeleon", Type = "Fire", Ability = "Blaze", Category = "Flame", Height = 1.1f, Weight = 19.0f, Description = "Charmeleon ha una coda fiammeggiante. Se la fiamma viene spenta, il suo spirito si spegnerà insieme a essa, e morirà." },
                new Pokemon { Id = 6, Name = "Charizard", Type = "Fire/Flying", Ability = "Blaze", Category = "Flame", Height = 1.7f, Weight = 90.5f, Description = "Charizard ha ali che bruciano più caldo quando vola velocemente. Evita il contatto ravvicinato con questo Pokémon che vola veloce." },
                new Pokemon { Id = 7, Name = "Squirtle", Type = "Water", Ability = "Torrent", Category = "Tiny Turtle", Height = 0.5f, Weight = 9.0f, Description = "Squirtle quando ritira il lungo collo nel guscio, spruzza acqua con forza straordinaria." },
                new Pokemon { Id = 8, Name = "Wartortle", Type = "Water", Ability = "Torrent", Category = "Turtle", Height = 1.0f, Weight = 22.5f, Description = "Wartortle è dotato di una coda di lancia, ma usa anche le zampe per lottare." },
                new Pokemon { Id = 9, Name = "Blastoise", Type = "Water", Ability = "Torrent", Category = "Shellfish", Height = 1.6f, Weight = 85.5f, Description = "Blastoise ha due cannoni d'acqua sul guscio. Questi cannoni d'acqua sono molto potenti." },
                new Pokemon { Id = 10, Name = "Caterpie", Type = "Bug", Ability = "Shield Dust", Category = "Worm", Height = 0.3f, Weight = 2.9f, Description = "Caterpie ha un corpo molle. Dopo il riposo, lascia un liquido appiccicoso sui rami o sulle foglie." },
                new Pokemon { Id = 11, Name = "Metapod", Type = "Bug", Ability = "Shed Skin", Category = "Cocoon", Height = 0.7f, Weight = 9.9f, Description = "Metapod ha una corazza rigida per proteggersi. Si muove solo quando è necessario evitare il pericolo." },
                new Pokemon { Id = 12, Name = "Butterfree", Type = "Bug/Flying", Ability = "Compound Eyes", Category = "Butterfly", Height = 1.1f, Weight = 32.0f, Description = "Butterfree ha antenne che si sciolgono quando il Pokémon cattura il sapore dei fiori. Questo liquido viene poi usato per il suo polline." },
                new Pokemon { Id = 13, Name = "Weedle", Type = "Bug/Poison", Ability = "Shield Dust", Category = "Hairy Bug", Height = 0.3f, Weight = 3.2f, Description = "Weedle è coperto da uno strato di peli corti. Si muove liberamente a caccia di erba e foglie." },
                new Pokemon { Id = 14, Name = "Kakuna", Type = "Bug/Poison", Ability = "Shed Skin", Category = "Cocoon", Height = 0.6f, Weight = 10.0f, Description = "Kakuna rimane praticamente immobile, mantenendo il suo corpo protetto in guscio. Quando minacciato, si difende con veleno." },
                new Pokemon { Id = 15, Name = "Beedrill", Type = "Bug/Poison", Ability = "Swarm", Category = "Poison Bee", Height = 1.0f, Weight = 29.5f, Description = "Beedrill è estremamente territoriale. Non vieta la sua zona di caccia neppure a chiunque gli passi vicino." },
                new Pokemon { Id = 16, Name = "Pidgey", Type = "Normal/Flying", Ability = "Keen Eye", Category = "Tiny Bird", Height = 0.3f, Weight = 1.8f, Description = "Pidgey ha un ottimo senso di direzione. In grado di trovare il suo nido anche se si trova molto lontano da casa." },
                new Pokemon { Id = 17, Name = "Pidgeotto", Type = "Normal/Flying", Ability = "Keen Eye", Category = "Bird", Height = 1.1f, Weight = 30.0f, Description = "Pidgeotto è dotato di una vista acuta. In grado di individuare un Pokèmon in movimento da molte miglia di distanza." },
                new Pokemon { Id = 18, Name = "Pidgeot", Type = "Normal/Flying", Ability = "Keen Eye", Category = "Bird", Height = 1.5f, Weight = 39.5f, Description = "Pidgeot è in grado di volare alla velocità del suono. Questo gli permette di catturare prede senza fatica." },
                new Pokemon { Id = 19, Name = "Rattata", Type = "Normal", Ability = "Run Away", Category = "Mouse", Height = 0.3f, Weight = 3.5f, Description = "Rattata è un noto roditore notturno. È molto attivo durante la notte, in cerca di cibo." },
                new Pokemon { Id = 20, Name = "Raticate", Type = "Normal", Ability = "Run Away", Category = "Mouse", Height = 0.7f, Weight = 18.5f, Description = "Raticate ha un potente morso. È in grado di spezzare anche le più dure noccioline." },
                new Pokemon { Id = 21, Name = "Spearow", Type = "Normal/Flying", Ability = "Keen Eye", Category = "Tiny Bird", Height = 0.3f, Weight = 2.0f, Description = "Spearow ha occhi attentamente posizionati. È in grado di osservare in tutto il mondo, nonostante il suo piccolo cervello." },
                new Pokemon { Id = 22, Name = "Fearow", Type = "Normal/Flying", Ability = "Keen Eye", Category = "Beak", Height = 1.2f, Weight = 38.0f, Description = "Fearow è noto per la sua velocità. Può raggiungere una velocità di oltre 60 miglia all'ora." },
                new Pokemon { Id = 23, Name = "Ekans", Type = "Poison", Ability = "Intimidate", Category = "Snake", Height = 2.0f, Weight = 6.9f, Description = "Ekans muove il suo corpo sinuoso per muoversi silenziosamente. Ha una morsa velenosa." },
                new Pokemon { Id = 24, Name = "Arbok", Type = "Poison", Ability = "Intimidate", Category = "Cobra", Height = 3.5f, Weight = 65.0f, Description = "Arbok ha una paura istintiva di molti predatori e per questo si protegge con un veleno letale." },
                new Pokemon { Id = 25, Name = "Pikachu", Type = "Electric", Ability = "Static", Category = "Mouse", Height = 0.4f, Weight = 6.0f, Description = "Pikachu ha una grande quantità di energia elettrica. Si scarica quando è stressato o arrabbiato." },
                new Pokemon { Id = 26, Name = "Raichu", Type = "Electric", Ability = "Static", Category = "Mouse", Height = 0.8f, Weight = 30.0f, Description = "Raichu ha una coda che emette elettricità. Questo Pokèmon carica la sua coda per scaricare elettricità." },
                new Pokemon { Id = 27, Name = "Sandshrew", Type = "Ground", Ability = "Sand Veil", Category = "Mouse", Height = 0.6f, Weight = 12.0f, Description = "Sandshrew ha una pelle ruvida e solida che lo protegge dagli attacchi. Si nasconde sotto la sabbia quando è minacciato." },
                new Pokemon { Id = 28, Name = "Sandslash", Type = "Ground", Ability = "Sand Veil", Category = "Mouse", Height = 1.0f, Weight = 29.5f, Description = "Sandslash è noto per le sue affilate lame sulla schiena. Utilizza queste lame per difendersi dai predatori." },
                new Pokemon { Id = 29, Name = "NidoranF", Type = "Poison", Ability = "Poison Point", Category = "Poison Pin", Height = 0.4f, Weight = 7.0f, Description = "Nidoran♀ è noto per la sua natura timida. Si nasconde facilmente in luoghi tranquilli." },
                new Pokemon { Id = 30, Name = "Nidorina", Type = "Poison", Ability = "Poison Point", Category = "Poison Pin", Height = 0.8f, Weight = 20.0f, Description = "Nidorina è una difesa genitore. Protegge i suoi cuccioli con la sua velenosa coda." },
                new Pokemon { Id = 31, Name = "Nidoqueen", Type = "Poison/Ground", Ability = "Poison Point", Category = "Drill", Height = 1.3f, Weight = 60.0f, Description = "Nidoqueen ha una corazza spessa che la protegge dagli attacchi. Può sparare veleno dalle sue corna." },
                new Pokemon { Id = 32, Name = "NidoranM", Type = "Poison", Ability = "Poison Point", Category = "Poison Pin", Height = 0.5f, Weight = 9.0f, Description = "Nidoran♂ ha corna velenose. Sono ricoperte di veleno se qualcuno le tocca con mano." },
                new Pokemon { Id = 33, Name = "Nidorino", Type = "Poison", Ability = "Poison Point", Category = "Poison Pin", Height = 0.9f, Weight = 19.5f, Description = "Nidorino ha muscoli potenti che gli consentono di spostare oggetti molto pesanti. Le sue corna velenose rendono pericoloso il contatto." },
                new Pokemon { Id = 34, Name = "Nidoking", Type = "Poison/Ground", Ability = "Poison Point", Category = "Drill", Height = 1.4f, Weight = 62.0f, Description = "Nidoking ha un carattere aggressivo e ama combattere. Ha la forza di distruggere edifici con un solo colpo." },
                new Pokemon { Id = 35, Name = "Clefairy", Type = "Fairy", Ability = "Cute Charm", Category = "Fairy", Height = 0.6f, Weight = 7.5f, Description = "Clefairy è noto per la sua coda a forma di cuore. Si dice che porti fortuna a chi lo vede." },
                new Pokemon { Id = 36, Name = "Clefable", Type = "Fairy", Ability = "Cute Charm", Category = "Fairy", Height = 1.3f, Weight = 40.0f, Description = "Clefable è un Pokémon misterioso. Si dice che possieda poteri magici e che porti fortuna a chi lo possiede." },
                new Pokemon { Id = 37, Name = "Vulpix", Type = "Fire", Ability = "Flash Fire", Category = "Fox", Height = 0.6f, Weight = 9.9f, Description = "Vulpix ha una coda molto folta. Si dice che la temperatura della sua coda aumenti quando è felice o eccitato." },
                new Pokemon { Id = 38, Name = "Ninetales", Type = "Fire", Ability = "Flash Fire", Category = "Fox", Height = 1.1f, Weight = 19.9f, Description = "Ninetales possiede nove code. Ogni coda ha un colore diverso e una propria abilità magica." },
                new Pokemon { Id = 39, Name = "Jigglypuff", Type = "Normal/Fairy", Ability = "Cute Charm", Category = "Balloon", Height = 0.5f, Weight = 5.5f, Description = "Jigglypuff è noto per il suo canto ipnotico. Mette a dormire i suoi nemici cantando la sua ninnananna." },
                new Pokemon { Id = 40, Name = "Wigglytuff", Type = "Normal/Fairy", Ability = "Cute Charm", Category = "Balloon", Height = 1.0f, Weight = 12.0f, Description = "Wigglytuff ha una pelle elastica e soffice che lo protegge dagli attacchi. È molto affettuoso con i suoi allenatori." },
                new Pokemon { Id = 41, Name = "Zubat", Type = "Poison/Flying", Ability = "Inner Focus", Category = "Bat", Height = 0.8f, Weight = 7.5f, Description = "Zubat ama appollaiarsi nei luoghi bui. Si nutre di sangue succhiando il sangue dei suoi nemici." },
                new Pokemon { Id = 42, Name = "Golbat", Type = "Poison/Flying", Ability = "Inner Focus", Category = "Bat", Height = 1.6f, Weight = 55.0f, Description = "Golbat ha una lingua lunga e appuntita che usa per succhiare il sangue delle sue prede." },
                new Pokemon { Id = 43, Name = "Oddish", Type = "Grass/Poison", Ability = "Chlorophyll", Category = "Weed", Height = 0.5f, Weight = 5.4f, Description = "Oddish ha un odore caratteristico. Il suo corpo rilascia un profumo dolce quando si sente minacciato." },
                new Pokemon { Id = 44, Name = "Gloom", Type = "Grass/Poison", Ability = "Chlorophyll", Category = "Weed", Height = 0.8f, Weight = 8.6f, Description = "Gloom ha una secrezione vischiosa sul suo corpo. Questa sostanza può causare paralisi se toccata." },
                new Pokemon { Id = 45, Name = "Vileplume", Type = "Grass/Poison", Ability = "Chlorophyll", Category = "Flower", Height = 1.2f, Weight = 18.6f, Description = "Vileplume ha un aroma avvolgente. Si dice che chiunque lo annusi non possa resistere al sonno." },
                new Pokemon { Id = 46, Name = "Paras", Type = "Bug/Grass", Ability = "Effect Spore", Category = "Mushroom", Height = 0.3f, Weight = 5.4f, Description = "Paras ha un fungo sul dorso. Il fungo assorbe sostanze nutritive dal suo ospite e lo mantiene in vita." },
                new Pokemon { Id = 47, Name = "Parasect", Type = "Bug/Grass", Ability = "Effect Spore", Category = "Mushroom", Height = 1.0f, Weight = 29.5f, Description = "Parasect è noto per il suo aspetto spaventoso. Le sue zampe sono così forti da poter tagliare tronchi d'albero." },
                new Pokemon { Id = 48, Name = "Venonat", Type = "Bug/Poison", Ability = "Compound Eyes", Category = "Insect", Height = 1.0f, Weight = 30.0f, Description = "Venonat ha antenne sensibili che possono rilevare il calore del corpo. Si dice che questo Pokèmon sia attratto dalla luce." },
                new Pokemon { Id = 49, Name = "Venomoth", Type = "Bug/Poison", Ability = "Shield Dust", Category = "Poison Moth", Height = 1.5f, Weight = 12.5f, Description = "Venomoth ha un piumaggio polveroso. Questo Pokèmon rilascia polvere velenosa quando è minacciato." },
                new Pokemon { Id = 50, Name = "Diglett", Type = "Ground", Ability = "Sand Veil", Category = "Mole", Height = 0.2f, Weight = 0.8f, Description = "Diglett ha una lunga coda sotterranea che gli consente di muoversi rapidamente sotto terra." },
                new Pokemon { Id = 51, Name = "Dugtrio", Type = "Ground", Ability = "Sand Veil", Category = "Mole", Height = 0.7f, Weight = 33.3f, Description = "Dugtrio si muove rapidamente sotto terra. Può scavare un tunnel in pochi secondi." },
                new Pokemon { Id = 52, Name = "Meowth", Type = "Normal", Ability = "Pickup", Category = "Scratch Cat", Height = 0.4f, Weight = 4.2f, Description = "Meowth ama raccogliere oggetti scintillanti. Si dice che porti fortuna a chi trova monete nel suo cappello." },
                new Pokemon { Id = 53, Name = "Persian", Type = "Normal", Ability = "Limber", Category = "Classy Cat", Height = 1.0f, Weight = 32.0f, Description = "Persian ha una coda lunghissima e folta. È in grado di tenere in equilibrio il suo corpo anche sui terreni più accidentati." },
                new Pokemon { Id = 54, Name = "Psyduck", Type = "Water", Ability = "Damp", Category = "Duck", Height = 0.8f, Weight = 19.6f, Description = "Psyduck ha frequenti mal di testa. Quando la testa gli duole, emette onde cerebrali che possono causare mal di testa agli altri." },
                new Pokemon { Id = 55, Name = "Golduck", Type = "Water", Ability = "Damp", Category = "Duck", Height = 1.7f, Weight = 76.6f, Description = "Golduck ha una forza incredibile. È in grado di sollevare pesi molto più grandi del suo corpo." },
                new Pokemon { Id = 56, Name = "Mankey", Type = "Fighting", Ability = "Vital Spirit", Category = "Pig Monkey", Height = 0.5f, Weight = 28.0f, Description = "Mankey è noto per il suo temperamento. Si arrabbia facilmente e attacca chiunque gli si avvicini." },
                new Pokemon { Id = 57, Name = "Primeape", Type = "Fighting", Ability = "Vital Spirit", Category = "Pig Monkey", Height = 1.0f, Weight = 32.0f, Description = "Primeape è estremamente aggressivo. Attacca senza pietà chiunque gli sfidi il dominio." },
                new Pokemon { Id = 58, Name = "Growlithe", Type = "Fire", Ability = "Intimidate", Category = "Puppy", Height = 0.7f, Weight = 19.0f, Description = "Growlithe ha un forte senso dell'olfatto. È in grado di individuare odori sottili a chilometri di distanza." },
                new Pokemon { Id = 59, Name = "Arcanine", Type = "Fire", Ability = "Intimidate", Category = "Legendary", Height = 1.9f, Weight = 155.0f, Description = "Arcanine è conosciuto come il Pokémon leggendario. Si dice che le sue fiamme siano in grado di illuminare anche le notti più buie." },
                new Pokemon { Id = 60, Name = "Poliwag", Type = "Water", Ability = "Water Absorb", Category = "Tadpole", Height = 0.6f, Weight = 12.4f, Description = "Poliwag ha una pellicola trasparente sulla sua pelle che lo protegge dagli attacchi. Si muove in modo ondulatorio per muoversi in acqua." },
                new Pokemon { Id = 61, Name = "Poliwhirl", Type = "Water", Ability = "Water Absorb", Category = "Tadpole", Height = 1.0f, Weight = 20.0f, Description = "Poliwhirl si evolve quando è circondato da una forte energia. Si dice che possa controllare le maree." },
                new Pokemon { Id = 62, Name = "Poliwrath", Type = "Water/Fighting", Ability = "Water Absorb", Category = "Tadpole", Height = 1.3f, Weight = 54.0f, Description = "Poliwrath ha muscoli potenti. Può afferrare un avversario con le sue mani e schiacciarlo." },
                new Pokemon { Id = 63, Name = "Abra", Type = "Psychic", Ability = "Synchronize", Category = "Psi", Height = 0.9f, Weight = 19.5f, Description = "Abra ha un'abilità straordinaria. È in grado di percepire eventi futuri tramite le sue onde cerebrali." },
                new Pokemon { Id = 64, Name = "Kadabra", Type = "Psychic", Ability = "Synchronize", Category = "Psi", Height = 1.3f, Weight = 56.5f, Description = "Kadabra possiede un'intelligenza superiore. Si dice che possa possedere abilità telecinetiche." },
                new Pokemon { Id = 65, Name = "Alakazam", Type = "Psychic", Ability = "Synchronize", Category = "Psi", Height = 1.5f, Weight = 48.0f, Description = "Alakazam è uno dei Pokèmon più intelligenti. Il suo cervello è più sviluppato rispetto a quello umano." },
                new Pokemon { Id = 66, Name = "Machop", Type = "Fighting", Ability = "Guts", Category = "Superpower", Height = 0.8f, Weight = 19.5f, Description = "Machop ha una grande forza fisica. Si allena sollevando pesi per aumentare la sua forza." },
                new Pokemon { Id = 67, Name = "Machoke", Type = "Fighting", Ability = "Guts", Category = "Superpower", Height = 1.5f, Weight = 70.5f, Description = "Machoke è noto per i suoi muscoli sporgenti. Il suo corpo è così forte da poter sollevare montagne." },
                new Pokemon { Id = 68, Name = "Machamp", Type = "Fighting", Ability = "Guts", Category = "Superpower", Height = 1.6f, Weight = 130.0f, Description = "Machamp ha quattro braccia. Può muoverle tutte contemporaneamente per eseguire mosse devastanti." },
                new Pokemon { Id = 69, Name = "Bellsprout", Type = "Grass/Poison", Ability = "Chlorophyll", Category = "Flower", Height = 0.7f, Weight = 4.0f, Description = "Bellsprout ha un gambo lungo e sottile. Cattura le prede con la sua bocca." },
                new Pokemon { Id = 70, Name = "Weepinbell", Type = "Grass/Poison", Ability = "Chlorophyll", Category = "Flycatcher", Height = 1.0f, Weight = 6.4f, Description = "Weepinbell ha un aroma intenso. Le sue esalazioni possono far svenire chi le inala." },
                new Pokemon { Id = 71, Name = "Victreebel", Type = "Grass/Poison", Ability = "Chlorophyll", Category = "Flycatcher", Height = 1.7f, Weight = 15.5f, Description = "Victreebel ha delle vesciche su foglie. Rilascia un liquido acido che scioglie la carne delle sue prede." },
                new Pokemon { Id = 72, Name = "Tentacool", Type = "Water/Poison", Ability = "Clear Body", Category = "Jellyfish", Height = 0.9f, Weight = 45.5f, Description = "Tentacool è un Pokémon marino. Ha tentacoli velenosi che possono paralizzare le sue prede." },
                new Pokemon { Id = 73, Name = "Tentacruel", Type = "Water/Poison", Ability = "Clear Body", Category = "Jellyfish", Height = 1.6f, Weight = 55.0f, Description = "Tentacruel è dotato di poteri psichici. Può controllare il movimento delle onde del mare." },
                new Pokemon { Id = 74, Name = "Geodude", Type = "Rock/Ground", Ability = "Rock Head", Category = "Rock", Height = 0.4f, Weight = 20.0f, Description = "Geodude è un Pokémon roccioso. Il suo corpo è composto da rocce dure come il granito." },
                new Pokemon { Id = 75, Name = "Graveler", Type = "Rock/Ground", Ability = "Rock Head", Category = "Rock", Height = 1.0f, Weight = 105.0f, Description = "Graveler si evolve da Geodude. Le sue rocce si rigenerano continuamente." },
                new Pokemon { Id = 76, Name = "Golem", Type = "Rock/Ground", Ability = "Rock Head", Category = "Megaton", Height = 1.4f, Weight = 300.0f, Description = "Golem è un Pokémon robusto. Il suo corpo è così duro da poter resistere a qualsiasi attacco." },
                new Pokemon { Id = 77, Name = "Ponyta", Type = "Fire", Ability = "Run Away", Category = "Fire Horse", Height = 1.0f, Weight = 30.0f, Description = "Ponyta è noto per la sua velocità. Si muove così rapidamente da lasciare dietro di sé scie di fuoco." },
                new Pokemon { Id = 78, Name = "Rapidash", Type = "Fire", Ability = "Run Away", Category = "Fire Horse", Height = 1.7f, Weight = 95.0f, Description = "Rapidash ha una criniera di fuoco. Si dice che possa superare la velocità del suono." },
                new Pokemon { Id = 79, Name = "Slowpoke", Type = "Water/Psychic", Ability = "Oblivious", Category = "Dopey", Height = 1.2f, Weight = 36.0f, Description = "Slowpoke è noto per la sua lentezza. Si dice che il suo cervello funzioni a velocità estremamente ridotta." },
                new Pokemon { Id = 80, Name = "Slowbro", Type = "Water/Psychic", Ability = "Oblivious", Category = "Hermit Crab", Height = 1.6f, Weight = 78.5f, Description = "Slowbro ha una conchiglia sulla sua coda. Si dice che il suo cervello si sia evoluto per diventare più potente." },
                new Pokemon { Id = 81, Name = "Magnemite", Type = "Electric/Steel", Ability = "Magnet Pull", Category = "Magnet", Height = 0.3f, Weight = 6.0f, Description = "Magnemite è composto da magneti. Si attacca agli oggetti metallici e li attira verso di sé." },
                new Pokemon { Id = 82, Name = "Magneton", Type = "Electric/Steel", Ability = "Magnet Pull", Category = "Magnet", Height = 1.0f, Weight = 60.0f, Description = "Magneton è formato da tre Magnemite. Possono unirsi e formare un solo Magneton." },
                new Pokemon { Id = 83, Name = "Farfetch'd", Type = "Normal/Flying", Ability = "Keen Eye", Category = "Wild Duck", Height = 0.8f, Weight = 15.0f, Description = "Farfetch'd porta sempre con sé uno stelo di porro. Lo usa come arma nelle battaglie." },
                new Pokemon { Id = 84, Name = "Doduo", Type = "Normal/Flying", Ability = "Run Away", Category = "Twin Bird", Height = 1.4f, Weight = 39.2f, Description = "Doduo ha due teste indipendenti. Le teste comunicano tra loro tramite suoni gutturali." },
                new Pokemon { Id = 85, Name = "Dodrio", Type = "Normal/Flying", Ability = "Run Away", Category = "Triple Bird", Height = 1.8f, Weight = 85.2f, Description = "Dodrio è noto per la sua velocità. Può correre a oltre 60 km/h." },
                new Pokemon { Id = 86, Name = "Seel", Type = "Water", Ability = "Thick Fat", Category = "Sea Lion", Height = 1.1f, Weight = 90.0f, Description = "Seel si evolve in Dewgong. Ha una pelliccia fitta che lo protegge dal freddo." },
                new Pokemon { Id = 87, Name = "Dewgong", Type = "Water/Ice", Ability = "Thick Fat", Category = "Sea Lion", Height = 1.7f, Weight = 120.0f, Description = "Dewgong ha un corpo simile a una foca. Si muove agevolmente sia sulla terraferma che nell'acqua." },
                new Pokemon { Id = 88, Name = "Grimer", Type = "Poison", Ability = "Stench", Category = "Sludge", Height = 0.9f, Weight = 30.0f, Description = "Grimer è composto da sostanze tossiche. Rilascia gas velenosi quando è eccitato." },
                new Pokemon { Id = 89, Name = "Muk", Type = "Poison", Ability = "Stench", Category = "Sludge", Height = 1.2f, Weight = 30.0f, Description = "Muk è un Pokémon velenoso. Il suo corpo è costituito da sostanze tossiche e scorie." },
                new Pokemon { Id = 90, Name = "Shellder", Type = "Water", Ability = "Shell Armor", Category = "Bivalve", Height = 0.3f, Weight = 4.0f, Description = "Shellder è protetto da un guscio duro. Si aggrappa alle rocce con i suoi denti affilati." },
                new Pokemon { Id = 91, Name = "Cloyster", Type = "Water/Ice", Ability = "Shell Armor", Category = "Bivalve", Height = 1.5f, Weight = 132.5f, Description = "Cloyster ha un guscio duro come la roccia. Può chiudersi ermeticamente per proteggersi dagli attacchi." },
                new Pokemon { Id = 92, Name = "Gastly", Type = "Ghost/Poison", Ability = "Levitate", Category = "Gas", Height = 1.3f, Weight = 0.1f, Description = "Gastly è un Pokémon spettrale. Si nasconde nell'ombra e spaventa le sue vittime con il suo aspetto spaventoso." },
                new Pokemon { Id = 93, Name = "Haunter", Type = "Ghost/Poison", Ability = "Levitate", Category = "Gas", Height = 1.6f, Weight = 0.1f, Description = "Haunter ha una lingua lunga e appuntita. La usa per paralizzare le sue prede." },
                new Pokemon { Id = 94, Name = "Gengar", Type = "Ghost/Poison", Ability = "Levitate", Category = "Shadow", Height = 1.5f, Weight = 40.5f, Description = "Gengar può passare attraverso le pareti. Si dice che si nasconda nell'ombra per attaccare i viandanti." },
                new Pokemon { Id = 95, Name = "Onix", Type = "Rock/Ground", Ability = "Rock Head", Category = "Rock Snake", Height = 8.8f, Weight = 210.0f, Description = "Onix è un serpente di roccia. Il suo corpo è lungo più di 10 metri." },
                new Pokemon { Id = 96, Name = "Drowzee", Type = "Psychic", Ability = "Insomnia", Category = "Hypnosis", Height = 1.0f, Weight = 32.4f, Description = "Drowzee cattura le sue prede con le sue onde cerebrali. Si nutre dei sogni degli altri Pokémon." },
                new Pokemon { Id = 97, Name = "Hypno", Type = "Psychic", Ability = "Insomnia", Category = "Hypnosis", Height = 1.6f, Weight = 75.6f, Description = "Hypno ha poteri ipnotici. Si dice che possa controllare le menti degli altri Pokémon." },
                new Pokemon { Id = 98, Name = "Krabby", Type = "Water", Ability = "Hyper Cutter", Category = "River Crab", Height = 0.4f, Weight = 6.5f, Description = "Krabby ha delle chele robuste. Le usa per afferrare le sue prede e schiacciarle." },
                new Pokemon { Id = 99, Name = "Kingler", Type = "Water", Ability = "Hyper Cutter", Category = "Pincer", Height = 1.3f, Weight = 60.0f, Description = "Kingler è noto per le sue enormi chele. Può spezzare persino le conchiglie più dure." },
                new Pokemon { Id = 100, Name = "Voltorb", Type = "Electric", Ability = "Soundproof", Category = "Ball", Height = 0.5f, Weight = 10.4f, Description = "Voltorb assomiglia a una Poké Ball. Esplode quando viene disturbato." },
                new Pokemon { Id = 101, Name = "Electrode", Type = "Electric", Ability = "Soundproof", Category = "Ball", Height = 1.2f, Weight = 66.6f, Description = "Electrode ha un corpo sferico. È pieno di energia elettrica pronta a esplodere." },
                new Pokemon { Id = 102, Name = "Exeggcute", Type = "Grass/Psychic", Ability = "Chlorophyll", Category = "Egg", Height = 0.4f, Weight = 2.5f, Description = "Exeggcute è composto da uova. Le uova sono molto nutrienti e ricche di sostanze nutritive." },
                new Pokemon { Id = 103, Name = "Exeggutor", Type = "Grass/Psychic", Ability = "Chlorophyll", Category = "Coconut", Height = 2.0f, Weight = 120.0f, Description = "Exeggutor è alto più di 6 metri. Si dice che possa generare un'ombra così grande da oscurare interi campi." },
                new Pokemon { Id = 104, Name = "Cubone", Type = "Ground", Ability = "Rock Head", Category = "Lonely", Height = 0.4f, Weight = 6.5f, Description = "Cubone indossa sempre un teschio sulla testa. Si dice che il teschio appartenga alla madre deceduta." },
                new Pokemon { Id = 105, Name = "Marowak", Type = "Ground", Ability = "Rock Head", Category = "Bone Keeper", Height = 1.0f, Weight = 45.0f, Description = "Marowak è noto per il suo bastone osseo. Lo usa per difendersi dagli attacchi nemici." },
                new Pokemon { Id = 106, Name = "Hitmonlee", Type = "Fighting", Ability = "Limber", Category = "Kicking", Height = 1.5f, Weight = 49.8f, Description = "Hitmonlee ha gambe incredibilmente forti. Può colpire un avversario con una velocità incredibile." },
                new Pokemon { Id = 107, Name = "Hitmonchan", Type = "Fighting", Ability = "Keen Eye", Category = "Punching", Height = 1.4f, Weight = 50.2f, Description = "Hitmonchan è un esperto di arti marziali. Si dice che possa lanciare più di 1000 pugni al secondo." },
                new Pokemon { Id = 108, Name = "Lickitung", Type = "Normal", Ability = "Own Tempo", Category = "Licking", Height = 1.2f, Weight = 65.5f, Description = "Lickitung ha una lingua lunga più di 2 metri. Può avvolgere la sua lingua intorno a un albero e tirarlo giù." },
                new Pokemon { Id = 109, Name = "Koffing", Type = "Poison", Ability = "Levitate", Category = "Poison Gas", Height = 0.6f, Weight = 1.0f, Description = "Koffing emette gas velenosi. Può avvolgere i suoi nemici in una nuvola tossica." },
                new Pokemon { Id = 110, Name = "Weezing", Type = "Poison", Ability = "Levitate", Category = "Poison Gas", Height = 1.2f, Weight = 9.5f, Description = "Weezing è formato da due Koffing. Le due teste possono emettere gas velenosi contemporaneamente." },
                new Pokemon { Id = 111, Name = "Rhyhorn", Type = "Ground/Rock", Ability = "Lightning Rod", Category = "Spikes", Height = 1.0f, Weight = 115.0f, Description = "Rhyhorn ha una corazza dura come la roccia. Può caricare contro un avversario con grande potenza." },
                new Pokemon { Id = 112, Name = "Rhydon", Type = "Ground/Rock", Ability = "Lightning Rod", Category = "Drill", Height = 1.9f, Weight = 120.0f, Description = "Rhydon possiede una forza fisica incredibile. Può spostare anche le rocce più grandi." },
                new Pokemon { Id = 113, Name = "Chansey", Type = "Normal", Ability = "Natural Cure", Category = "Egg", Height = 1.1f, Weight = 34.6f, Description = "Chansey è noto per la sua gentilezza. Si dice che offra uova nutrienti agli altri Pokémon." },
                new Pokemon { Id = 114, Name = "Tangela", Type = "Grass", Ability = "Chlorophyll", Category = "Vine", Height = 1.0f, Weight = 35.0f, Description = "Tangela è coperto di viti. Può intrappolare i suoi nemici con le sue viti." },
                new Pokemon { Id = 115, Name = "Kangaskhan", Type = "Normal", Ability = "Early Bird", Category = "Parent", Height = 2.2f, Weight = 80.0f, Description = "Kangaskhan protegge sempre il suo piccolo. Si dice che possa lanciare un avversario a 100 metri di distanza." },
                new Pokemon { Id = 116, Name = "Horsea", Type = "Water", Ability = "Swift Swim", Category = "Dragon", Height = 0.4f, Weight = 8.0f, Description = "Horsea è un piccolo drago marino. Può sparare getti d'acqua dai suoi tubi nasali." },
                new Pokemon { Id = 117, Name = "Seadra", Type = "Water", Ability = "Poison Point", Category = "Dragon", Height = 1.2f, Weight = 25.0f, Description = "Seadra ha pungiglioni velenosi. Può paralizzare una preda con il suo veleno." },
                new Pokemon { Id = 118, Name = "Goldeen", Type = "Water", Ability = "Swift Swim", Category = "Goldfish", Height = 0.6f, Weight = 15.0f, Description = "Goldeen ha un corpo elegante. Si dice che sia molto popolare tra i pescatori." },
                new Pokemon { Id = 119, Name = "Seaking", Type = "Water", Ability = "Swift Swim", Category = "Goldfish", Height = 1.3f, Weight = 39.0f, Description = "Seaking è conosciuto come il Pokémon Re. Si dice che possa controllare i mari." },
                new Pokemon { Id = 120, Name = "Staryu", Type = "Water", Ability = "Illuminate", Category = "Starshape", Height = 0.8f, Weight = 34.5f, Description = "Staryu ha un nucleo luminoso al centro del suo corpo. Può ricrescere se diviso in pezzi." },
                new Pokemon { Id = 121, Name = "Starmie", Type = "Water/Psychic", Ability = "Illuminate", Category = "Mysterious", Height = 1.1f, Weight = 80.0f, Description = "Starmie ha un nucleo luminoso al centro del suo corpo. Si dice che contenga poteri misteriosi." },
                new Pokemon { Id = 122, Name = "Mr. Mime", Type = "Psychic/Fairy", Ability = "Soundproof", Category = "Barrier", Height = 1.3f, Weight = 54.5f, Description = "Mr. Mime è noto per le sue abilità di mimo. Può creare barriere invisibili per proteggersi." },
                new Pokemon { Id = 123, Name = "Scyther", Type = "Bug/Flying", Ability = "Swarm", Category = "Mantis", Height = 1.5f, Weight = 56.0f, Description = "Scyther ha lame affilate come rasoi. Può tagliare un albero in un solo colpo." },
                new Pokemon { Id = 124, Name = "Jynx", Type = "Ice/Psychic", Ability = "Oblivious", Category = "Human Shape", Height = 1.4f, Weight = 40.6f, Description = "Jynx ha una danza incantevole. Può mettere un avversario in trance con il suo movimento." },
                new Pokemon { Id = 125, Name = "Electabuzz", Type = "Electric", Ability = "Static", Category = "Electric", Height = 1.1f, Weight = 30.0f, Description = "Electabuzz ha pugni elettrici. Si dice che possa lanciare fulmini con i suoi pugni." },
                new Pokemon { Id = 126, Name = "Magmar", Type = "Fire", Ability = "Flame Body", Category = "Spitfire", Height = 1.3f, Weight = 44.5f, Description = "Magmar ha un corpo coperto di lava. Può sparare fiamme da bocca e mani." },
                new Pokemon { Id = 127, Name = "Pinsir", Type = "Bug", Ability = "Hyper Cutter", Category = "Stag Beetle", Height = 1.5f, Weight = 55.0f, Description = "Pinsir ha una forza incredibile nelle sue chele. Si dice che possa tagliare un albero con un solo colpo." },
                new Pokemon { Id = 128, Name = "Tauros", Type = "Normal", Ability = "Intimidate", Category = "Wild Bull", Height = 1.4f, Weight = 88.4f, Description = "Tauros è noto per la sua furia. Può caricare a una velocità incredibile." },
                new Pokemon { Id = 129, Name = "Magikarp", Type = "Water", Ability = "Swift Swim", Category = "Fish", Height = 0.9f, Weight = 10.0f, Description = "Magikarp è noto per la sua debolezza. Può saltare alto fuori dall'acqua." },
                new Pokemon { Id = 130, Name = "Gyarados", Type = "Water/Flying", Ability = "Intimidate", Category = "Atrocious", Height = 6.5f, Weight = 235.0f, Description = "Gyarados è noto per la sua ira. Si dice che possa distruggere interi villaggi con un solo sbuffo." },
                new Pokemon { Id = 131, Name = "Lapras", Type = "Water/Ice", Ability = "Water Absorb", Category = "Transport", Height = 2.5f, Weight = 220.0f, Description = "Lapras è un Pokémon molto gentile. Si dice che possa trasportare persone sul suo dorso attraverso mari ghiacciati." },
                new Pokemon { Id = 132, Name = "Ditto", Type = "Normal", Ability = "Limber", Category = "Transform", Height = 0.3f, Weight = 4.0f, Description = "Ditto può cambiare forma in base al suo ambiente. Può trasformarsi in qualsiasi altro Pokémon." },
                new Pokemon { Id = 133, Name = "Eevee", Type = "Normal", Ability = "Run Away", Category = "Evolution", Height = 0.3f, Weight = 6.5f, Description = "Eevee ha molte evoluzioni. Si dice che evolva in base alla pietra che tiene." },
                new Pokemon { Id = 134, Name = "Vaporeon", Type = "Water", Ability = "Water Absorb", Category = "Bubble Jet", Height = 1.0f, Weight = 29.0f, Description = "Vaporeon ha una coda a forma di fiocco di neve. Può sciogliere il corpo in acqua." },
                new Pokemon { Id = 135, Name = "Jolteon", Type = "Electric", Ability = "Volt Absorb", Category = "Lightning", Height = 0.8f, Weight = 24.5f, Description = "Jolteon ha pelli appuntite. Può sparare fulmini dalle sue punte." },
                new Pokemon { Id = 136, Name = "Flareon", Type = "Fire", Ability = "Flash Fire", Category = "Flame", Height = 0.9f, Weight = 25.0f, Description = "Flareon ha una criniera fiammeggiante. Il suo corpo può raggiungere temperature molto elevate." },
                new Pokemon { Id = 137, Name = "Porygon", Type = "Normal", Ability = "Trace", Category = "Virtual", Height = 0.8f, Weight = 36.5f, Description = "Porygon è un Pokémon artificiale. Si dice che sia stato creato da un programma di computer." },
                new Pokemon { Id = 138, Name = "Omanyte", Type = "Rock/Water", Ability = "Swift Swim", Category = "Spiral", Height = 0.4f, Weight = 7.5f, Description = "Omanyte ha una conchiglia dura come la roccia. Si dice che sia un antico Pokémon marino." },
                new Pokemon { Id = 139, Name = "Omastar", Type = "Rock/Water", Ability = "Swift Swim", Category = "Spiral", Height = 1.0f, Weight = 35.0f, Description = "Omastar ha tentacoli appuntiti. Si dice che sia un predatore dei fondali marini." },
                new Pokemon { Id = 140, Name = "Kabuto", Type = "Rock/Water", Ability = "Swift Swim", Category = "Shellfish", Height = 0.5f, Weight = 11.5f, Description = "Kabuto ha una corazza dura come la roccia. Si dice che sia un Pokémon antico risorto." },
                new Pokemon { Id = 141, Name = "Kabutops", Type = "Rock/Water", Ability = "Swift Swim", Category = "Shellfish", Height = 1.3f, Weight = 40.5f, Description = "Kabutops è noto per le sue lame affilate. Si dice che sia un cacciatore abile." },
                new Pokemon { Id = 142, Name = "Aerodactyl", Type = "Rock/Flying", Ability = "Rock Head", Category = "Fossil", Height = 1.8f, Weight = 59.0f, Description = "Aerodactyl è un Pokémon volante. Si dice che sia un antico predatore del cielo." },
                new Pokemon { Id = 143, Name = "Snorlax", Type = "Normal", Ability = "Immunity", Category = "Sleeping", Height = 2.1f, Weight = 460.0f, Description = "Snorlax è noto per il suo appetito insaziabile. Può dormire per giorni interi." },
                new Pokemon { Id = 144, Name = "Articuno", Type = "Ice/Flying", Ability = "Pressure", Category = "Freeze", Height = 1.7f, Weight = 55.4f, Description = "Articuno è un Pokémon leggendario. Si dice che possa congelare l'aria intorno a sé." },
                new Pokemon { Id = 145, Name = "Zapdos", Type = "Electric/Flying", Ability = "Pressure", Category = "Electric", Height = 1.6f, Weight = 52.6f, Description = "Zapdos è un Pokémon leggendario. Si dice che possa creare temporali con i suoi attacchi elettrici." },
                new Pokemon { Id = 146, Name = "Moltres", Type = "Fire/Flying", Ability = "Pressure", Category = "Flame", Height = 2.0f, Weight = 60.0f, Description = "Moltres è un Pokémon leggendario. Si dice che le sue ali brucino con il fuoco." },
                new Pokemon { Id = 147, Name = "Dratini", Type = "Dragon", Ability = "Shed Skin", Category = "Dragon", Height = 1.8f, Weight = 3.3f, Description = "Dratini è un piccolo drago. Il suo corpo è avvolto da una pelle liscia." },
                new Pokemon { Id = 148, Name = "Dragonair", Type = "Dragon", Ability = "Shed Skin", Category = "Dragon", Height = 4.0f, Weight = 16.5f, Description = "Dragonair ha una coda lunga e sinuosa. Si dice che porti fortuna a chi lo vede." },
                new Pokemon { Id = 149, Name = "Dragonite", Type = "Dragon/Flying", Ability = "Inner Focus", Category = "Dragon", Height = 2.2f, Weight = 210.0f, Description = "Dragonite è un drago possente. Può volare a una velocità incredibile." },
                new Pokemon { Id = 150, Name = "Mewtwo", Type = "Psychic", Ability = "Pressure", Category = "Genetic", Height = 2.0f, Weight = 122.0f, Description = "Mewtwo è un Pokémon leggendario creato artificialmente. Ha un'intelligenza straordinaria e poteri psichici." },
                new Pokemon { Id = 151, Name = "Mew", Type = "Psychic", Ability = "Synchronize", Category = "New Species", Height = 0.4f, Weight = 4.0f, Description = "Mew è un Pokémon leggendario raro. Si dice che contenga il DNA di tutti i Pokémon." }
            };
        }
        
        static void DisplayAllPokemon(List<Pokemon> pokedex, HashSet<int> uniquecapturedPokemonIds)
        {
            foreach (var pokemon in pokedex)
            {
                string capturedMark = uniquecapturedPokemonIds.Contains(pokemon.Id) ? "!" : "";
                Console.WriteLine($"{capturedMark} {pokemon.Id}: {pokemon.Name} - Tipo:  {pokemon.Type}, Abilità: {pokemon.Ability}, Categoria: {pokemon.Category}, Peso: {pokemon.Weight}kg, Altezza: {pokemon.Height}");
            }
        }
        static void SearchPokemonById(List<Pokemon> pokedex)
        {
            Console.Write("Inserisci il numero di Pokedex: ");
            if(int.TryParse(Console.ReadLine(), out int id))
            {
                var pokemon = pokedex.Find(p => p.Id == id);
                if(pokemon != null)
                {
                    Console.WriteLine($"{pokemon.Id}: {pokemon.Name} - Tipo:  {pokemon.Type}, Abilità: {pokemon.Ability}, Categoria: {pokemon.Category}, Peso: {pokemon.Weight}kg, Altezza: {pokemon.Height}m\");");
                    Console.WriteLine($"{pokemon.Description}");
                }
                else
                {
                    Console.WriteLine("Pokemon non trovato.");
                }
            }
            else 
            {
                Console.WriteLine("Numero del pokedex non valido.");
            }
        }

        static void CapturePokemon(List<Pokemon> pokedex, List<Pokemon> capturedPokemon, HashSet<int> uniquecapturedPokemonIds, Player player)
        {
            Random random = new Random();
            Pokemon wildpokemon = pokedex[random.Next(pokedex.Count)];
            Console.WriteLine($"Appare {wildpokemon.Name} selvatico!");

            if (uniquecapturedPokemonIds.Contains(wildpokemon.Id))
            {
                Console.WriteLine("Hai già catturato questo Pokèmon!");
            }
            else
            {
                Console.WriteLine("Questo Pokèmon deve ancora essere catturato!");
            }

            bool catching = true;
            while (catching)
            {
                Console.WriteLine("Scegli un'opzione");
                Console.WriteLine("1. Lancia una Pokèball");
                Console.WriteLine("2. Fuga");

                string input = Console.ReadLine();
                double captureMultiplier = 1.0;

                switch (input)
                {
                    case "1":
                        Console.WriteLine("Seleziona il tipo di Pokéball:");
                        Console.WriteLine("1. Pokéball");
                        Console.WriteLine($"2. Megaball x{player.MegaBallCount}");
                        Console.WriteLine($"3. Ultraball x{player.UltraBallCount}");
                        Console.Write("Seleziona un'opzione: ");

                        string ballchoice = Console.ReadLine();
                        switch (ballchoice)
                        {
                            case "1":
                                Console.WriteLine("Vai Pokèball!");
                                captureMultiplier = 1.0;
                                break;
                            case "2":
                                if (player.MegaBallCount > 0)
                                {
                                    Console.WriteLine("Vai Megaball!");
                                    captureMultiplier = 1.4;
                                    player.MegaBallCount--;
                                }
                                else
                                {
                                    Console.WriteLine("Non hai Megaball nello zaino.");
                                    continue;
                                }
                                break;
                            case "3":
                                if (player.UltraBallCount > 0)
                                {
                                    Console.WriteLine("Vai Ultraball!");
                                    captureMultiplier = 1.6;
                                    player.UltraBallCount--;
                                }
                                else
                                {
                                    Console.WriteLine("Non hai Ultraball nello zaino.");
                                    continue;
                                }
                                break;
                            default:
                                Console.WriteLine("Opzione non valida. Riprova.");
                            continue;
                        }

                        if (AttemptCapture(wildpokemon, random, captureMultiplier))
                        {
                            Console.WriteLine($"{wildpokemon.Name} è stato catturato!");
                            capturedPokemon.Add(wildpokemon);
                            uniquecapturedPokemonIds.Add(wildpokemon.Id);
                            int reward = GetPokeDollarsRewardForCapture();
                            player.PokeDollars += reward;
                            Console.WriteLine($"Hai guadagnato {reward} Pokèdollari! Pokèdollari disponibili: {player.PokeDollars}");

                            if (uniquecapturedPokemonIds.Count % 15 == 0)
                            {
                                string newMedalName = $"Medaglia {uniquecapturedPokemonIds.Count / 15}";
                                if (!player.MedalsTracker.HasMedal(newMedalName))
                                {
                                    Medal newMedal = GetMedalByCount (uniquecapturedPokemonIds.Count);
                                    player.MedalsTracker.AddMedal(newMedal, player);
                                }
                            }
                            catching = false;
                        }
                        else
                        {
                            Console.WriteLine("Oh no, il Pokèmon si è liberato!");
                            if (random.Next(100) < 37)
                            {
                                Console.WriteLine($"Il {wildpokemon.Name} selvatico fugge!");
                                catching = false;
                            }
                            else
                            {
                                Console.WriteLine($"Il {wildpokemon.Name} resta a guardare...");
                            }
                        }
                    break;
                    case "2":
                        Console.WriteLine("Scampato pericolo!");
                        catching = false;
                        break;
                    default:
                        Console.WriteLine("Opzione non valida. Riprova.");
                    break;
                }
            }
        }

        static Medal GetMedalByCount( int count )
        {
            switch (count/15)
            {
                case 1:
                    return new Medal("Medaglia Sasso", 3, 0, 100);
                case 2:
                    return new Medal("Medaglia Cascata", 6, 0, 300);
                case 3:
                    return new Medal("Medaglia Tuono", 4, 2, 500);
                case 4:
                    return new Medal("Medaglia Arcobaleno", 6, 4, 700);
                case 5:
                    return new Medal("Medaglia Palude", 8, 6, 800);
                case 6:
                    return new Medal("Medaglia Anima", 10, 8, 900);
                case 7:
                    return new Medal("Medaglia Vulcano", 10, 10, 1000);
                case 8:
                    return new Medal("Medaglia Terra", 12, 12, 1200);
                case 9:
                    return new Medal("Medaglia Superquattro", 15, 12, 1300);
                case 10:
                    return new Medal("Medaglia Campione", 15, 15, 1500);
                default: 
                    return new Medal ($"Medaglia {count / 15}", 10, 5, 500);
            }
        }

        static int GetPokeDollarsRewardForCapture()
        {
            return 100;
        }

        static void VisitPokeMarket(Player player)
        {
            bool shopping = true;
            while (shopping) 
            {
                Console.WriteLine("Benvenuto al PokéMarket!");
                Console.WriteLine($"Pokèdollari disponibili: {player.PokeDollars}");
                Console.WriteLine("Cosa desideri acquistare?");
                Console.WriteLine($"1. Megaball - Prezzo: {GetMegaballPrice()} PokéDollari - Disponibili: {player.MegaBallCount}");
                Console.WriteLine($"2. Ultraball - Prezzo: {GetUltraballPrice()} PokéDollari - Disponibili: {player.UltraBallCount}");
                Console.WriteLine("3. Torna indietro");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        BuyBall(player, "Megaball", GetMegaballPrice());
                        break;
                    case "2":
                        BuyBall(player, "Ultraball", GetUltraballPrice());
                        break;
                    case "3":
                        Console.WriteLine("Grazie per aver visitato il PokéMarket!");
                        shopping = false;
                        break;
                    default:
                        Console.WriteLine("Opzione non valida. Riprova.");
                        break;
                }
                if (shopping)
                {
                    Console.WriteLine();
                }
            }
        }

        static void BuyBall(Player player, string ballType, int ballPrice)
        {
            if (player.PokeDollars >= ballPrice)
            {
                player.PokeDollars -= ballPrice;

                if (ballType == "Megaball")
                {
                    player.MegaBallCount += 1;
                }
                else if (ballType == "Ultraball")
                {
                    player.UltraBallCount += 1;
                }

                Console.WriteLine($"Hai acquistato 1 {ballType}.");
            }
            else
            {
                Console.WriteLine("Non hai abbastanza PokéDollari per acquistare questa Pokéball.");
            }
        }

        static int GetMegaballPrice()
        {
            return 100;
        }

        static int GetUltraballPrice()
        {
            return 300;
        }

        static bool AttemptCapture(Pokemon pokemon, Random random, double captureMultiplier)
        {
            double baseCaptureRate = 40;
            double captureChance = baseCaptureRate * captureMultiplier;
            return random.Next(100) < captureChance;
        }

        static void DisplayCapturedPokemon(List<Pokemon> capturedPokemon)
        {
            if (capturedPokemon.Count == 0)
            {
                Console.WriteLine("Nessun Pokemon catturato fin'ora.");
            }
            else
            {
                foreach (Pokemon pokemon in capturedPokemon)
                {
                    Console.WriteLine($"{pokemon.Id} : {pokemon.Name} - {pokemon.Type}");
                }
            }
        }

        static void EvaluatePokedex (HashSet<int> uniquecapturedPokemonIds)
        {
            int totalCaptured = uniquecapturedPokemonIds.Count;
            Console.WriteLine($"Hai catturato {totalCaptured} Pokèmon su 151!");

            if (totalCaptured == 151)
            {
                Console.WriteLine("Complimenti per aver completato il Pokèdex Regionale! Sei un allenatore davvero in gamba!");
            }
        }
    }
}