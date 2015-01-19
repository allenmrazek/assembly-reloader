using System;
using System.Linq;
using AssemblyReloader.Queries;
using AssemblyReloader.TypeTracking;

namespace AssemblyReloader.Factory.Implementations
{
    class AddonInfoFactory : IAddonInfoFactory
    {
        private readonly AddonAttributeFromTypeQuery _addonFromTypeQuery;

        public AddonInfoFactory(AddonAttributeFromTypeQuery addonFromTypeQuery)
        {
            if (addonFromTypeQuery == null) throw new ArgumentNullException("addonFromTypeQuery");
            _addonFromTypeQuery = addonFromTypeQuery;
        }



        public AddonInfo Create(Type type)
        {
            if (type == null) throw new ArgumentNullException("type");

            var kspAddon = _addonFromTypeQuery.GetKspAddonAttribute(type);

            if (!kspAddon.Any()) throw new InvalidOperationException("type must have KSPAddon attribute");

            return new AddonInfo(type, kspAddon.Single());
        }
    }
}
