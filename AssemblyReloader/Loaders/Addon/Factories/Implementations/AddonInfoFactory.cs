using System;
using System.Linq;
using AssemblyReloader.Queries;
using AssemblyReloader.Queries.Implementations;
using AssemblyReloader.TypeTracking;

namespace AssemblyReloader.Loaders.Addon.Factories.Implementations
{
    class AddonInfoFactory : IAddonInfoFactory
    {
        private readonly IAddonAttributeFromTypeQuery _addonFromTypeQuery;

        public AddonInfoFactory(IAddonAttributeFromTypeQuery addonFromTypeQuery)
        {
            if (addonFromTypeQuery == null) throw new ArgumentNullException("addonFromTypeQuery");
            _addonFromTypeQuery = addonFromTypeQuery;
        }



        public AddonInfo Create(Type type)
        {
            if (type == null) throw new ArgumentNullException("type");

            var kspAddon = _addonFromTypeQuery.Get(type);

            if (!kspAddon.Any()) throw new InvalidOperationException("type must have KSPAddon attribute");

            return new AddonInfo(type, kspAddon.Single());
        }
    }
}
