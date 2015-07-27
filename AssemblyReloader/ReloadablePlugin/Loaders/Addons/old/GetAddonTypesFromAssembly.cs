//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using AssemblyReloader.Unsorted;

//namespace AssemblyReloader.ReloadablePlugin.Loaders.Addons
//{
//    public class GetAddonTypesFromAssembly : IGetTypesFromAssembly<KSPAddonType>
//    {
//        private readonly IGetAttributesOfType<KSPAddon> _getAttributes;

//        public GetAddonTypesFromAssembly(IGetAttributesOfType<KSPAddon> getAttributes)
//        {
//            if (getAttributes == null) throw new ArgumentNullException("getAttributes");

//            _getAttributes = getAttributes;
//        }


//        public IEnumerable<KSPAddonType> Get(Assembly assembly)
//        {
//            if (assembly == null) throw new ArgumentNullException("assembly");

//            return assembly.GetTypes()
//                .Where(ty => _getAttributes.Get(ty).Any())
//                .Select(t => new KSPAddonType(t));
//        }
//    }
//}
