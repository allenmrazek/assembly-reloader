using System;
using System.Linq;
using System.Reflection;
using AssemblyReloader.ILModifications;
using AssemblyReloader.Providers;
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

        private readonly IReloadableAssemblyProvider _assemblyProvider;

        public event PluginLoadedHandler OnLoaded = delegate { };
        public event PluginUnloadedHandler OnUnloaded = delegate { }; 

        public ReloadablePlugin(
            IReloadableAssemblyProvider assemblyProvider)
        {
            if (assemblyProvider == null) throw new ArgumentNullException("assemblyProvider");

            _assemblyProvider = assemblyProvider;
        }


        public void Load()
        {
            if (!_loaded.IsNull())
                Unload();

            _loaded = _assemblyProvider.Get();
        }


        public void Unload()
        {
            if (!_loaded.IsNull()) return;

            OnUnloaded(_loaded);
            _loaded = null;
        }


        public string Name
        {
            get { return _assemblyProvider.Name; }
        }
    }
}