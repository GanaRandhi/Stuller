using System.Text.Json;
using MyPoki.Repository;
using MyPoki.Repository.Models;
using RichardSzalay.MockHttp;

namespace MyPokiApi.Tests;

public class MyPokiApiTest
{
    private readonly MockHttpMessageHandler mockHttp;

    public MyPokiApiTest()
    {
        mockHttp = new MockHttpMessageHandler();
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task GetResourceByNameCase()
    {
        // assemble
        mockHttp.Expect("*berry/cheri/")
            .Respond("application/json", JsonSerializer.Serialize(new Berry { Name = "cheri" }));
        MyPokeApiClient client = CreateSut();

        // act
        var result = await client.GetResourceAsync<Berry>("CHERI");

        // assert
        mockHttp.VerifyNoOutstandingExpectation();
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task GetNamedResourcePageAsync()
    {
        // assemble
        NamedApiResourceList<Berry> berryPage = new();

        mockHttp.Expect("*berry")
            .Respond("application/json", JsonSerializer.Serialize(berryPage));

        MyPokeApiClient client = CreateSut();

        // act
        var berry = await client.GetNamedResourcePageAsync<Berry>();

        // assert
        mockHttp.VerifyNoOutstandingExpectation();
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task UrlNavigationAsyncSinglePikachu()
    {
        // assemble
        Pokemon responsePikachu = new()
        {
            Name = "pikachu",
            Id = 25,
            Species = new()
            {
                Name = "pikachu",
                Url = "https://pokeapi.co/api/v2/pokemon-species/25/"
            }
        };
        PokemonSpecies responseSpecies = new() { Name = "pikachu" };

        mockHttp.Expect("*pokemon/pikachu/")
            .Respond("application/json", JsonSerializer.Serialize(
                responsePikachu,
                options: new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower })
        );
        mockHttp.Expect("*pokemon-species/25/")
            .Respond("application/json", JsonSerializer.Serialize(
                responseSpecies,
                options: new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower })
        );
        MyPokeApiClient client = CreateSut();

        var pikachu = await client.GetResourceAsync<Pokemon>("pikachu");

        // act
        _ = await client.GetResourceAsync(pikachu.Species);

        // assert
        mockHttp.VerifyNoOutstandingExpectation();
    }
    
    [Fact]
    [Trait("Category", "Unit")]
    public async Task UrlNavigationAsyncSingleCached()
    {
        // assemble
        Pokemon responsePikachu = new()
        {
            Name = "pikachu",
            Id = 25,
            Species = new()
            {
                Name = "pikachu",
                Url = "https://pokeapi.co/api/v2/pokemon-species/25/"
            }
        };
        PokemonSpecies responseSpecies = new() { Name = "pikachu", Id = 25 };

        mockHttp.Expect("*pokemon/pikachu/")
            .Respond("application/json", JsonSerializer.Serialize(
                responsePikachu,
                options: new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower })
        );
        mockHttp.Expect("*pokemon-species/25/")
            .Respond("application/json", JsonSerializer.Serialize(
                responseSpecies,
                options: new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower })
        );
        MyPokeApiClient client = CreateSut();

        var pikachu = await client.GetResourceAsync<Pokemon>("pikachu");
        _ = await client.GetResourceAsync(pikachu.Species);

        mockHttp.ResetExpectations();
        mockHttp.Expect("*pokemon-species/25/")
            .Respond("application/json", JsonSerializer.Serialize(
                responseSpecies,
                options: new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower })
        );

        // act
        _ = await client.GetResourceAsync(pikachu.Species);

        // assert
        Assert.Throws<InvalidOperationException>(mockHttp.VerifyNoOutstandingExpectation);
    }
    
    [Fact]
    [Trait("Category", "Integration")]
    public async Task GetTypePagedResourceIntegrationTest()
    {
        // assemble
        using MyPokeApiClient client = new();

        // act
        var page = await client.GetNamedResourcePageAsync<MyPoki.Repository.Models.Type>();

        // assert
        Assert.True(page.Results.Any());
    }    

    [Fact]
    [Trait("Category", "Integration")]
    public async Task GetTypeResourceAsyncIntegrationTest()
    {
        // assemble
        using MyPokeApiClient client = new();
        string damageD = "";
        string damageH = "";
        string damageN = "";

        // act
        var type = await client.GetResourceAsync<MyPoki.Repository.Models.Type>(1);
        var poke = await client.GetResourceAsync<Pokemon>(1);
        foreach(var damage in type.DamageRelations.DoubleDamageFrom)
        {
            damageD = damageD + ", " + damage.Name;
        }
        foreach(var damage in type.DamageRelations.HalfDamageFrom)
        {
            damageH = damageH + ", " + damage.Name;
        }
        foreach(var damage in type.DamageRelations.NoDamageFrom)
        {
            damageN = damageN + ", " + damage.Name;
        }

        // assert
        Assert.True(type.Id != default);
        Assert.True(!String.IsNullOrEmpty(damageD));        
        Assert.True(String.IsNullOrEmpty(damageD));
    }

    private MyPokeApiClient CreateSut() => new(mockHttp);    

    private IEnumerable<KeyValuePair<string, string>> ToPairs(int limit, int offset)
        => new KeyValuePair<string, string>[] 
        {
            new KeyValuePair<string, string>(nameof(limit),limit.ToString()),
            new KeyValuePair<string, string>(nameof(offset),offset.ToString())
        };
}
