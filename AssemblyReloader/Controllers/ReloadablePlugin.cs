//using System;
//using System.Linq;
//using System.Reflection;
//using AssemblyReloader.Annotations;
//using AssemblyReloader.DataObjects;
//using AssemblyReloader.Game;
//using AssemblyReloader.Gui;
//using AssemblyReloader.Loaders;
//using AssemblyReloader.Signals;
//using ReeperCommon.Containers;
//using ReeperCommon.Extensions;
//using ReeperCommon.FileSystem;

//namespace AssemblyReloader.Controllers
//{

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
//}