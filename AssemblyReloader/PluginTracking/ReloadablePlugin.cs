using System;
using System.Linq;
using System.Reflection;
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

        private readonly IAssemblyProvider _assemblyProvider;
        private readonly IFile _location;

        public event PluginLoadedHandler OnLoaded = delegate { };
        public event PluginUnloadedHandler OnUnloaded = delegate { }; 

        public ReloadablePlugin(
            IAssemblyProvider assemblyProvider,
            IFile location)
        {
            if (assemblyProvider == null) throw new ArgumentNullException("assemblyProvider");
            if (location == null) throw new ArgumentNullException("location");

            _assemblyProvider = assemblyProvider;
            _location = location;
        }


        public void Load()
        {
            if (!_loaded.IsNull())
                Unload();

            _loaded = _assemblyProvider.Get().SingleOrDefault();

            if (_loaded.IsNull())
                throw new InvalidOperationException("ReloadablePlugin: received NULL Assembly");

            OnLoaded(_loaded, _location);
        }


        public void Unload()
        {
            if (_loaded.IsNull()) return;

            OnUnloaded(_loaded, _location);
            _loaded = null;
        }


        public string Name
        {
            get { return _assemblyProvider.Name; }
        }

        public Maybe<Assembly> Assembly
        {
            get { return _loaded.IsNull() ? Maybe<Assembly>.None : Maybe<Assembly>.With(_loaded); }
        }
    }
}