using MyPoki.Repository;
using MyPoki.Repository.Models;

class RunPoki
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Hello From Poki World! Please Enter your Name:");
        var playerName = Console.ReadLine();
        MyPokeApiClient pokeClient = new MyPokeApiClient();
        Pokemon pokemonPage = new();
        string pokiName = "";       
        
        bool isEntryGood = false;
        do
        {
            Console.WriteLine(playerName + " Please Enter Pokemon Name:");
            pokiName = Console.ReadLine();
            if(String.IsNullOrEmpty(pokiName) || String.IsNullOrWhiteSpace(pokiName))
            {
                Console.WriteLine("You sure you enter a name??");
            }
            else 
                isEntryGood = true;
        } while (!isEntryGood);
        
        try
        {            
            pokemonPage = await pokeClient.GetResourceAsync<Pokemon>(pokiName);

        }
        catch (Exception)
        {
            Console.WriteLine("The " + pokiName + " entered is Not Available.");
            Console.WriteLine("Or Try a different pokemon next time.\n Please press Enter to exit");
            Console.ReadLine();
            Environment.Exit(-1);
        }

        try
        {
            Console.WriteLine("Thats an interesting Pokemon name ");

            // Get the first page of Pokémon (default limit is 20)
            var pokeAbility = await pokeClient.GetResourceAsync<Ability>(pokemonPage.Id.ToString());
                
            string types = string.Join(", ", pokemonPage.Types.Select(x=>x.Type.Name));

            //Print Pokemon Types, Resource.
            Console.WriteLine("Your "+pokiName +" has Types : " + types);
            Console.WriteLine("\nYour "+pokiName +"'s Resource Name: "+ pokeAbility.Name);

            //Print Abilities
            string ability = string.Join(", ",pokemonPage.Abilities.Select(x=>x.Ability.Name));
            Console.WriteLine("Your "+pokiName +"'s Abilities are: "+ ability);
            Console.WriteLine("Your "+pokiName +"'s strengths and weaknesses are: "+pokeAbility.EffectEntries[1].Effect.ToString());   
             
        }
        catch (Exception)
        {            
            Console.WriteLine("The " + pokiName + " entered is still in an Egg and yet to born. \n Please try after few more months. It needs time to HATCH, PATCH and CATCH.");
            Console.WriteLine("\n Please press Enter to exit");
            Console.ReadLine();
            Environment.Exit(-1);
        } 
        //check Damage Stats
        try
        {
            var pokeTypes = await pokeClient.GetResourceAsync<MyPoki.Repository.Models.Type>(pokemonPage.Id);
            //Strong or Weak
            string strongAgainst = GetStrongTo(pokeTypes);
            string weakAgainst = GetWeakTo(pokeTypes);
                
            Console.WriteLine("\nYour "+pokiName +"'s Stronger against : \n" + strongAgainst);
            Console.WriteLine("\nYour "+pokiName +"'s Seaker against : \n" + weakAgainst);
            Console.WriteLine("\n Please press Enter to exit");
            Console.ReadLine();
        }
        catch (System.Exception)
        {
            Console.WriteLine("\nThe " + pokiName + " has issues pulling the damage i.e., Attack and Defence details. \n Please try after few more months. It needs time to HATCH, PATCH and CATCH.\n\nOr try Pokemon Name: Bulbasaur");
            Console.WriteLine("\n Please press Enter to exit");
            Console.ReadLine();
         }
    }

    private static string GetStrongTo(MyPoki.Repository.Models.Type pokeTypes)
    {
        string doubleDamageTo = string.Join(", ", pokeTypes.DamageRelations.DoubleDamageTo.Select(x=>x.Name));
        string halfDamageTo = string.Join(", ", pokeTypes.DamageRelations.HalfDamageTo.Select(x=>x.Name));
        return "Strong Attack Double:"+doubleDamageTo +"\nStrong Attack Half:"+ halfDamageTo;
    }

    private static string GetWeakTo(MyPoki.Repository.Models.Type pokeTypes)
    {        
        string doubleDamageFrom = string.Join(", ", pokeTypes.DamageRelations.DoubleDamageFrom.Select(x=>x.Name));
        string halfDamageFrom = string.Join(", ", pokeTypes.DamageRelations.HalfDamageFrom.Select(x=>x.Name));
        return "Weak Defence Double:"+doubleDamageFrom +"\nWeak Defence Half: "+ halfDamageFrom;
    }
}