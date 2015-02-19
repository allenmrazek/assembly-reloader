using System;
using System.Reflection;
using ReeperCommon.Extensions;

namespace AssemblyReloader.Loaders.PMLoader
{
    class PartModuleFactory : IPartModuleFactory
    {
        public PartModule AddPseudoModule(Type type, Part part, ConfigNode config, bool forceAwake = false)
        {
            var pseudoModule = AddModule(type, part, config, forceAwake);
            //var wa

            // unfinished!
            return pseudoModule;
        }

        /// <summary>
        /// The main difference is that this version of the method doesn't add a watch dog PartModule to help
        /// prevent
        /// </summary>
        /// <param name="type"></param>
        /// <param name="part"></param>
        /// <param name="config"></param>
        /// <param name="forceAwake"></param>
        /// <returns></returns>
        public PartModule AddModule(Type type, Part part, ConfigNode config, bool forceAwake = false)
        {


            return DoAddModule(type, part, config, forceAwake);
        }


        private PartModule DoAddModule(Type type, Part part, ConfigNode config, bool forceAwake = false)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (part == null) throw new ArgumentNullException("part");
            if (!typeof(PartModule).IsAssignableFrom(type))
                throw new ArgumentException(type.FullName + " must be a PartModule");

            var pm = part.gameObject.AddComponent(type) as PartModule;

            if (pm.IsNull())
            {
                throw new InvalidOperationException(string.Format("Failed to add {0} to {1}; AddComponent returned null", type.FullName,
                    part.name));
            }

            part.Modules.Add(pm);

            if (forceAwake)
                typeof(PartModule).GetMethod("Awake", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy)
                    .Invoke(pm, null);

            pm.Load(config);

            return pm;
        }
    }
}
