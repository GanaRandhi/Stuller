using System.Text.Json.Serialization;
using MyPoki.Repository.Models;

namespace MyPoki.Repository
{    
    /// <summary>
    /// An item is an object in the games which the player can
    /// pick up, keep in their bag, and use in some manner. They
    /// have various uses, including healing, powering up, helping
    /// catch Pokémon, or to access a new area.
    /// </summary>
    public class Item : NamedApiResource
    {
        /// <summary>
        /// The identifier for this resource.
        /// </summary>
        public override int Id { get; set; }

        internal new static string ApiEndpoint { get; } = "item";

        /// <summary>
        /// The name for this resource.
        /// </summary>
        public override string Name { get; set; }

        /// <summary>
        /// The price of this item in stores.
        /// </summary>
        public int Cost { get; set; }

        /// <summary>
        /// The power of the move Fling when used with this item.
        /// </summary>
        [JsonPropertyName("fling_power")]
        public int? FlingPower { get; set; }

        /// <summary>
        /// The effect of this ability listed in different languages.
        /// </summary>
        [JsonPropertyName("effect_entries")]
        public List<VerboseEffect> EffectEntries { get; set; }

        /// <summary>
        /// The name of this item listed in different languages.
        /// </summary>
        public List<Names> Names { get; set; }
    }

    /// <summary>
    /// The default description of this item.
    /// </summary>
    public class ItemSprites
    {
        /// <summary>
        /// The default description of this item.
        /// </summary>
        public string Default { get; set; }
    }

    /// <summary>
    /// Item attributes define particular aspects of items,
    /// e.g. "usable in battle" or "consumable".
    /// </summary>
    public class ItemAttribute : NamedApiResource
    {
        /// <summary>
        /// The identifier for this resource.
        /// </summary>
        public override int Id { get; set; }

        internal new static string ApiEndpoint { get; } = "item-attribute";

        /// <summary>
        /// The name for this resource.
        /// </summary>
        public override string Name { get; set; }

        /// <summary>
        /// A list of items that have this attribute.
        /// </summary>
        public List<NamedApiResource<Item>> Items { get; set; }

        /// <summary>
        /// The name of this item attribute listed in different languages.
        /// </summary>
        public List<Names> Names { get; set; }
    }

    /// <summary>
    /// Item categories determine where items will be placed in the players bag.
    /// </summary>
    public class ItemCategory : NamedApiResource
    {
        /// <summary>
        /// The identifier for this resource.
        /// </summary>
        public override int Id { get; set; }

        internal new static string ApiEndpoint { get; } = "item-category";

        /// <summary>
        /// The name for this resource.
        /// </summary>
        public override string Name { get; set; }

        /// <summary>
        /// A list of items that are a part of this category.
        /// </summary>
        public List<NamedApiResource<Item>> Items { get; set; }

        /// <summary>
        /// The name of this item category listed in different languages.
        /// </summary>
        public List<Names> Names { get; set; }

        /// <summary>
        /// The pocket items in this category would be put in.
        /// </summary>
        public NamedApiResource<ItemPocket> Pocket { get; set; }
    }

    /// <summary>
    /// The various effects of the move "Fling" when used with different items.
    /// </summary>
    public class ItemFlingEffect : NamedApiResource
    {
        /// <summary>
        /// The identifier for this resource.
        /// </summary>
        public override int Id { get; set; }

        internal new static string ApiEndpoint { get; } = "item-fling-effect";

        /// <summary>
        /// The name for this resource.
        /// </summary>
        public override string Name { get; set; }

        /// <summary>
        /// A list of items that have this fling effect.
        /// </summary>
        public List<NamedApiResource<Item>> Items { get; set; }
    }

    /// <summary>
    /// Pockets within the players bag used for storing items by category.
    /// </summary>
    public class ItemPocket : NamedApiResource
    {
        /// <summary>
        /// The identifier for this resource.
        /// </summary>
        public override int Id { get; set; }

        internal new static string ApiEndpoint { get; } = "item-pocket";

        /// <summary>
        /// The name for this resource.
        /// </summary>
        public override string Name { get; set; }

        /// <summary>
        /// A list of item categories that are relevant to this item pocket.
        /// </summary>
        public List<NamedApiResource<ItemCategory>> Categories { get; set; }

        /// <summary>
        /// The name of this resource listed in different languages.
        /// </summary>
        public List<Names> Names { get; set; }
    }
}