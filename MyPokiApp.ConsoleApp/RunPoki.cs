using MyPoki.Repository;
using MyPoki.Repository.Models;

class RunPoki
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Hello From Poki World! Please Enter your Name:");
        var playerName = Console.ReadLine();
        bool continueApp = true;
        while (continueApp)
        {
            MyPokeApiClient pokeClient = new MyPokeApiClient();
            Pokemon pokemonPage = new();
            string pokiName = "";

            bool isEntryGood = false;
            bool isPokemonValid = false;
            do
            {
                Console.WriteLine(playerName + " Please Enter Pokemon Name:");
                pokiName = Console.ReadLine();
                if (string.IsNullOrEmpty(pokiName.Trim()))                
                    Console.WriteLine("You sure you enter a name??");                
                else
                    isEntryGood = true;

                if (isEntryGood)
                {
                    try
                    {
                        pokemonPage = await pokeClient.GetResourceAsync<Pokemon>(pokiName);
                        isPokemonValid = true;
    
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("The " + pokiName + " is Not Available.");
                        isEntryGood = !WannaContinue(isEntryGood);
                        continueApp = !isEntryGood;
                    }
                }

            } while (!isEntryGood);


           if (isPokemonValid)
           {
             try
             {
                 Console.WriteLine("Thats an interesting Pokemon name ");
                 string types = string.Join(", ", pokemonPage.Types.Select(x => x.Type.Name));
                 
                 //Print Abilities
                 string ability = string.Join(", ", pokemonPage.Abilities.Select(x => x.Ability.Name));
                 Console.WriteLine("Your " + pokiName + "'s Abilities are: " + ability);
                 Console.WriteLine("Your " + pokiName + " has Types : " + types);
                 
                 var pokeAbility = await pokeClient.GetResourceAsync<Ability>(pokemonPage.Id.ToString());
                 Console.WriteLine("\nYour " + pokiName + "'s Resource Name: " + pokeAbility.Name);                
                 Console.WriteLine("Your " + pokiName + "'s strengths and weaknesses are: " + pokeAbility.EffectEntries[1].Effect.ToString()); 
             }
             catch (Exception)
             {
                 Console.WriteLine("The " + pokiName + " entered is still in an Egg and yet to born. \n Please try after few more months. It needs time to HATCH, PATCH and CATCH.");
                 
             }
 
             //check Damage Stats
             try
             {
                 var pokeTypes = await pokeClient.GetResourceAsync<MyPoki.Repository.Models.Type>(pokemonPage.Id);
                 //Strong or Weak
                 string strongAgainst = GetStrongTo(pokeTypes);
                 string weakAgainst = GetWeakTo(pokeTypes);
 
                 Console.WriteLine("\nYour " + pokiName + "'s Stronger against : \n" + strongAgainst);
                 Console.WriteLine("\nYour " + pokiName + "'s Weaker against : \n" + weakAgainst);
             }
             catch (System.Exception)
             {
                 Console.WriteLine("\nThe " + pokiName + " has issues pulling the damage i.e., Attack and Defence details.\n Try these Pokemons instead - Bulbasaur, Ivysaur, Venusaur");
             }           
            continueApp = WannaContinue(continueApp);
            }
        }
    }

    private static bool WannaContinue(bool continueApp)
    {
        Console.WriteLine("\n Do you want to enter new Pokemon? (Y/N)");
        string userInput = Console.ReadLine();
        if (!userInput.Equals("y", StringComparison.CurrentCultureIgnoreCase))
            continueApp = false;     

        return continueApp;
    }

    private static string GetStrongTo(MyPoki.Repository.Models.Type pokeTypes)
    {
        string doubleDamageTo = string.Join(", ", pokeTypes.DamageRelations.DoubleDamageTo.Select(x=>x.Name));        
        string halfDamageFrom = string.Join(", ", pokeTypes.DamageRelations.HalfDamageFrom.Select(x=>x.Name));
        string noDamageFrom = string.Join(",", pokeTypes.DamageRelations.NoDamageFrom.Select(x=>x.Name));
        return "Double Damage To:"+doubleDamageTo +"\nHalf Damage From:"+ halfDamageFrom + "\nNo Damage From:" + noDamageFrom;
    }

    private static string GetWeakTo(MyPoki.Repository.Models.Type pokeTypes)
    {        
        string doubleDamageFrom = string.Join(", ", pokeTypes.DamageRelations.DoubleDamageFrom.Select(x=>x.Name));
        string halfDamageTo = string.Join(", ", pokeTypes.DamageRelations.HalfDamageTo.Select(x=>x.Name));        
        string noDamageTo = string.Join(",", pokeTypes.DamageRelations.NoDamageTo.Select(x=>x.Name));
        return "Double Damage From::"+doubleDamageFrom +"\nHalf Damage To: "+ halfDamageTo + "\nNo Damage To:" + noDamageTo;
    }
}
