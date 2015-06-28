using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AssemblyReloader.ReloadablePlugin.Loaders.Addons
{
    public class GetAddonTypesFromAssembly : IGetTypesFromAssembly<AddonType>
    {
        private readonly IGetAttributesOfType<KSPAddon> _getAttributes;

        public GetAddonTypesFromAssembly(IGetAttributesOfType<KSPAddon> getAttributes)
        {
            if (getAttributes == null) throw new ArgumentNullException("getAttributes");

            _getAttributes = getAttributes;
        }


        public IEnumerable<AddonType> Get(Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");

            return assembly.GetTypes()
                .Where(ty => _getAttributes.Get(ty).Any())
                .Select(t => new AddonType(t));
        }
    }
}
