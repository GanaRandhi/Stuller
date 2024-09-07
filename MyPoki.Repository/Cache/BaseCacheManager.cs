using System.Collections.Immutable;
using System.Reflection;
using MyPoki.Repository.Models;

namespace MyPoki.Repository.Cache
{
    internal abstract class BaseCacheManager : IDisposable
    {
        protected static readonly ImmutableHashSet<System.Type> ResourceTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(type => type.IsSubclassOf(typeof(ApiResource)) || type.IsSubclassOf(typeof(NamedApiResource)))
                .ToImmutableHashSet();

        protected bool IsTypeSupported(System.Type type) => ResourceTypes.Contains(type);

        public abstract void Dispose();

        public abstract void ClearAll();
    }
}