using System;
using System.Linq;
using System.Reflection;
using AssemblyReloader.Config;
using AssemblyReloader.Game;
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

        private readonly IAssemblyLoader _assemblyLoader;
        private readonly IFile _location;


        public event PluginLoadedHandler OnLoaded = delegate { };
        public event PluginUnloadedHandler OnUnloaded = delegate { }; 

        public ReloadablePlugin(
            IAssemblyLoader assemblyLoader,
            IFile location,
            IConfiguration configuration)
        {
            if (assemblyLoader == null) throw new ArgumentNullException("assemblyLoader");
            if (location == null) throw new ArgumentNullException("location");
            if (configuration == null) throw new ArgumentNullException("configuration");

            _assemblyLoader = assemblyLoader;
            _location = location;
            Configuration = configuration;
        }


        public void Load()
        {
            if (!_loaded.IsNull())
                Unload();

            _loaded = _assemblyLoader.Load().SingleOrDefault();

            if (_loaded.IsNull())
                throw new InvalidOperationException("ReloadablePlugin: received NULL Assembly");

            OnLoaded(_loaded, _location);
        }


        public void Unload()
        {
            if (_loaded.IsNull()) return;

            _assemblyLoader.Unload();

            OnUnloaded(_loaded, _location);
            _loaded = null;
        }


        public string Name
        {
            get { return _location.Name; }
        }

        public Maybe<Assembly> Assembly
        {
            get { return _loaded.IsNull() ? Maybe<Assembly>.None : Maybe<Assembly>.With(_loaded); }
        }

        public IConfiguration Configuration { get; private set; }
    }
}