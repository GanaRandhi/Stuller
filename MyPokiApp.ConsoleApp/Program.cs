using MyPoki.Repository;
using MyPoki.Repository.Models;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Hello From Poki World! Please Enter your Name:");
        var playerName = Console.ReadLine();
        string pokiName = "";
        bool isEntryGood = false;
        do
        {
            Console.WriteLine(playerName + " Please Enter Pokemon Name:");
            pokiName = Console.ReadLine();
            if(String.IsNullOrEmpty(pokiName) || String.IsNullOrWhiteSpace(pokiName))
                {
                    Console.WriteLine("Are you sure you enter a name??");
                }
            else 
                isEntryGood = true;
        } while (!isEntryGood);
        
            MyPokeApiClient pokeClient = new MyPokeApiClient();

            try
            {
                Console.WriteLine("Thats an interesting Pokemon name ");

                // Get the first page of Pokémon (default limit is 20)
                var pokemonPage = await pokeClient.GetResourceAsync<Pokemon>(pokiName);
                var pokeAbility = await pokeClient.GetResourceAsync<Ability>(pokemonPage.Id.ToString());

                Console.WriteLine("\n" + playerName + " Your Pokemon Name: " +pokemonPage.Name);
                string types = string.Join(", ", pokemonPage.Types.Select(x=>x.Type.Name));
                Console.WriteLine("Your "+pokiName +" has Types : " + types);
                Console.WriteLine("Your "+pokiName +"'s Resource Name: "+ pokeAbility.Name);

                //Print Abilities
                string ability = string.Join(", ",pokemonPage.Abilities.Select(x=>x.Ability.Name));
                Console.WriteLine("Your "+pokiName +"'s Abilities are: "+ ability);
                Console.WriteLine("Your "+pokiName +"'s strengths and weaknesses are: "+pokeAbility.EffectEntries[1].Effect.ToString());   

                Console.WriteLine("\n");
                Console.WriteLine("Please press Enter to exit");
                Console.ReadLine();             
            }
            catch (System.Exception)
            {            
                Console.WriteLine("You have entered Invalid Pokemon Name. ", pokiName);
                Console.WriteLine("\n Please press Enter to exit");
                Console.ReadLine();
            } 
    }
}