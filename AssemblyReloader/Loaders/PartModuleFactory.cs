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

            // note: why not use Part.AddModule here? When the PartLoader is loading parts, those
            // prefab GameObjects are active so MonoBehaviour.Awake gets called right away BEFORE
            // OnLoad.
            //
            // If we use Part.AddModule after PartLoader has run, the prefab will be
            // inactive causing Awake NOT to be called and skipping straight to OnLoad. In most cases 
            // this is probably okay but should a plugin rely on that behaviour
            // (Awake/OnAwake before OnLoad) for their prefab object, we might break
            // something. And thusly, we handle this ourselves
            var result = part.GameObject.AddComponent(pmType) as PartModule;

            if (result == null)
                throw new Exception("Failed to add " + pmType.FullName + " to " + part.PartName);

            part.Modules.Add(result);


            // if this is the prefab GameObject, it will never become active again and awake will never
            // get called
            if (!part.GameObject.activeInHierarchy && ReferenceEquals(part.PartInfo.PartPrefab, part))
            {
                var method = typeof (PartModule).GetMethod("Awake",
                    BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);

                if (method != null)
                    method.Invoke(result, null);
            }

            result.OnLoad(config);
        }
    }
}
