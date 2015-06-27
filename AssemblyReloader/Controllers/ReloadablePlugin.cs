using System;
using System.Linq;
using AssemblyReloader.Annotations;
using AssemblyReloader.Game;
using AssemblyReloader.Gui;
using AssemblyReloader.Providers;
using ReeperCommon.Extensions;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.Controllers
{
    public class ReloadablePlugin : IPluginInfo, IReloadablePlugin
    {
        private readonly IFile _reloadableFile;
        private readonly IGameAssemblyLoader _assemblyLoader;
        private readonly IAssemblyProvider _assemblyProvider;

        private IDisposable _loaded;

        public ReloadablePlugin(
            [NotNull] IFile reloadableFile, 
            [NotNull] IGameAssemblyLoader assemblyLoader,
            [NotNull] IAssemblyProvider assemblyProvider)
        {
            if (reloadableFile == null) throw new ArgumentNullException("reloadableFile");
            if (assemblyLoader == null) throw new ArgumentNullException("assemblyLoader");
            if (assemblyProvider == null) throw new ArgumentNullException("assemblyProvider");

            _reloadableFile = reloadableFile;
            _assemblyLoader = assemblyLoader;
            _assemblyProvider = assemblyProvider;
        }


        public string Name
        {
            get { return _reloadableFile.Name; }
        }


        public IFile Location
        {
            get { return _reloadableFile; }
        }


        private bool Load()
        {
            if (!_loaded.IsNull())
                throw new InvalidOperationException("Previous instance was not unloaded");

            var assembly = _assemblyProvider.Get(Location);
            if (!assembly.Any())
                throw new Exception("Failed to read assembly at " + Location.FullPath);

            _loaded = _assemblyLoader.Load(assembly.Single(), Location);

            return _loaded != null;
        }


        private void Unload()
        {
            if (_loaded.IsNull())
                throw new InvalidOperationException("No assembly loaded");
        }


        public bool Reload()
        {
            if (!_loaded.IsNull())
                Unload();

            return Load();
        }
    }


//    /// <summary>
//    /// This object tracks a particular reloadable dll (based on its location). This is necessary
//    /// because the state of a previous version of the assembly may affect how we load the next one;
//    /// specifically, we may need to modify changes the previous version made such as removing PartModules
//    /// from prefabs
//    /// </summary>
//    public class ReloadablePlugin : IReloadablePlugin, IPluginInfo
//    {
//        private Maybe<Assembly> _loaded = Maybe<Assembly>.None;
//        private readonly IAssemblyLoader _assemblyLoader;
//        private readonly IFile _location;


//        //public event PluginLoadedHandler OnLoaded = delegate { };
//        //public event PluginUnloadedHandler OnUnloaded = delegate { }; 

//        [Inject] public PluginLoadedSignal Loaded { get; set; }

//        [Inject] public PluginUnloadedSignal Unloaded { get; set; }


//        public ReloadablePlugin(
//            [NotNull] IAssemblyLoader assemblyLoader,
//            [NotNull] IFile location,
//            [NotNull] PluginConfiguration pluginConfiguration)
//        {
//            if (assemblyLoader == null) throw new ArgumentNullException("assemblyLoader");
//            if (location == null) throw new ArgumentNullException("location");
//            if (pluginConfiguration == null) throw new ArgumentNullException("pluginConfiguration");

//            _assemblyLoader = assemblyLoader;
//            _location = location;
//            Configuration = pluginConfiguration;
//        }


//        public void Load()
//        {
//            if (!_loaded.IsNull())
//                Unload();

//            _loaded = _assemblyLoader.Load();

//            if (!_loaded.Any())
//                throw new InvalidOperationException("ReloadablePlugin: received NULL Assembly");

//            Loaded.Dispatch(this);
//            //OnLoaded(_loaded.Single(), _location);
//        }


//        public void Unload()
//        {
//            if (!_loaded.Any()) return;

//            _assemblyLoader.Unload();

//            try
//            {
//                //OnUnloaded(_loaded.Single(), _location);
//                Unloaded.Dispatch(this);
//            }
//            finally
//            {
//                _loaded = Maybe<Assembly>.None;
//            }
//        }


//        public string Name
//        {
//            get { return _location.Name; }
//        }

//        public Maybe<Assembly> Assembly
//        {
//            get { return _loaded; }
//        }

//        public PluginConfiguration Configuration { get; private set; }

//        public IFile Location
//        {
//            get { return _location; }
//        }
//    }
}