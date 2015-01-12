using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssemblyReloader.AddonTracking;
using AssemblyReloader.Loaders;
using AssemblyReloader.Providers;
using AssemblyReloader.Queries;

namespace AssemblyReloader.Factory
{
    class AddonInfoFactoryDead
    {
        private readonly AddonsFromAssemblyQuery _provider;

        public AddonInfoFactoryDead(AddonsFromAssemblyQuery provider)
        {
            if (provider == null) throw new ArgumentNullException("provider");

            _provider = provider;
        }


        public AddonInfo Create(Type type, ReloadableAssembly fromAssembly)
        {
            var addon = _provider.GetKSPAddonFromType(type);

            if (!addon.Any())
                throw new InvalidOperationException(type.FullName + " does not have KSPAddon Attribute");

            return new AddonInfo(type, addon.First(), fromAssembly);
        }
    }
}
