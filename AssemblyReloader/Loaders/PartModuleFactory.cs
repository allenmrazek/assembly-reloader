using System;
using System.Reflection;
using AssemblyReloader.Game;

namespace AssemblyReloader.Loaders
{
    public class PartModuleFactory : IPartModuleFactory
    {
        public void Create(IPart part, Type pmType, ConfigNode config)
        {
            if (part == null) throw new ArgumentNullException("part");
            if (pmType == null) throw new ArgumentNullException("pmType");
            if (config == null) throw new ArgumentNullException("config");

            if (!pmType.IsSubclassOf(typeof (PartModule)))
                throw new ArgumentException("type " + pmType.FullName + " is not a PartModule");

            var result = part.GameObject.AddComponent(pmType) as PartModule;

            if (result == null)
                throw new Exception("Failed to add " + pmType.FullName + " to " + part.PartName);

            var method = typeof(PartModule).GetMethod("Awake", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);

            if (method != null)
                method.Invoke(result, null);

            result.OnLoad(config);
        }
    }
}
