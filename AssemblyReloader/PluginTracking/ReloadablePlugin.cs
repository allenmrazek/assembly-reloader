using System;
using System.Linq;
using System.Reflection;
using AssemblyReloader.ILModifications;
using ReeperCommon.Containers;
using ReeperCommon.Extensions;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.PluginTracking
{

    /// <summary>
    /// This object tracks a particular reloadable dll (based on its location). This is necessary
    /// because the state of a previous version of the assembly may affect how we load the next one;
    /// specifically, we may need to modify changes the previous version made such as removing PartModules
    /// from prefabs
    /// </summary>
    public class ReloadablePlugin : IReloadablePlugin
    {
        private Assembly _loaded;

        private readonly IFile _location;
        private readonly IModifiedAssemblyFactory _massemblyFactory;

        public event PluginLoadedHandler OnLoaded = delegate { };
        public event PluginUnloadedHandler OnUnloaded = delegate { }; 

        public ReloadablePlugin(
            IFile location,
            IModifiedAssemblyFactory massemblyFactory)
        {
            if (location == null) throw new ArgumentNullException("location");
            if (massemblyFactory == null) throw new ArgumentNullException("massemblyFactory");

            _location = location;

            _massemblyFactory = massemblyFactory;
        }


        public void Load()
        {
            using (var stream = new System.IO.MemoryStream())
            {
                var original = _massemblyFactory.Create(_location);

                original.Rename(Guid.NewGuid());

                original.Write(stream);

                var result = original.Load(stream);

                if (!result.Any())
                    return;
                
                _loaded = result.Single();
                OnLoaded(_loaded);
            }
        }


        public void Unload()
        {
            _loaded = null;
            OnUnloaded(_location);
        }


        public string Name
        {
            get { return _location.Name; }
        }
    }
}