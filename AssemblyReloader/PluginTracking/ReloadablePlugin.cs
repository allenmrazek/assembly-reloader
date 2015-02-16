using System;
using System.Linq;
using System.Reflection;
using AssemblyReloader.Addon;
using AssemblyReloader.ILModifications;
using AssemblyReloader.ILModifications.Assembly;
using AssemblyReloader.Loaders;
using AssemblyReloader.Loaders.Addon;
using AssemblyReloader.Queries;
using Mono.Cecil;
using ReeperCommon.Events;
using ReeperCommon.Extensions;
using ReeperCommon.FileSystem;
using ReeperCommon.Logging;

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
        private IAddonLoader _addonLoader;
        private IDisposable _partModuleLoader;

        private readonly IQueryFactory _queryFactory;

        private readonly IFile _location;
        private readonly ILoaderFactory _loaderFactory;
        private readonly IModifiedAssemblyFactory _massemblyFactory;
        private readonly IEventSubscriber<GameScenes> _levelLoadedEvent;
        private readonly ILog _log;
        
        private IEventSubscription _addonSceneChangeSubscription;



        public ReloadablePlugin(
            IFile location,

            ILoaderFactory loaderFactory,
            IModifiedAssemblyFactory massemblyFactory,
            IEventSubscriber<GameScenes> levelLoadedEvent,
            ILog log,
            IQueryFactory queryFactory)
        {
            if (location == null) throw new ArgumentNullException("location");
            if (loaderFactory == null) throw new ArgumentNullException("loaderFactory");
            if (massemblyFactory == null) throw new ArgumentNullException("massemblyFactory");
            if (levelLoadedEvent == null) throw new ArgumentNullException("levelLoadedEvent");
            if (log == null) throw new ArgumentNullException("log");
            if (queryFactory == null) throw new ArgumentNullException("queryFactory");

            _location = location;
            _loaderFactory = loaderFactory;
            _massemblyFactory = massemblyFactory;
            _levelLoadedEvent = levelLoadedEvent;
            _log = log;
            _queryFactory = queryFactory;
        }




        private void ApplyModifications(System.IO.MemoryStream stream)
        {
            if (stream == null) throw new ArgumentNullException("stream");

            var definition = AssemblyDefinition.ReadAssembly(_location.FullPath);

            //var modifier = 

            var modified = _massemblyFactory.Create(definition);

            //modifier.Rename(definition, Guid.NewGuid());
            modified.Rename(Guid.NewGuid());

            //definition.MainModule.GetTypes().ToList().ForEach(td =>
            //{
            //    td.Namespace = Guid.NewGuid() + "." + td.Namespace;
            //    td.Name = Guid.NewGuid() + "." + td.Name;
            //});

            _log.Debug("CustomAttributes");

            definition.CustomAttributes.ToList().ForEach(ca => _log.Normal("Attr: " + ca.AttributeType.Name));

            var attr = definition.CustomAttributes.FirstOrDefault(ca => ca.AttributeType.Name == "GuidAttribute");
            _log.Normal("GuidAttribute: " + attr.ConstructorArguments[0].Value);

            //attr.ConstructorArguments[0].Value = Guid.NewGuid();

            //definition.CustomAttributes.Remove(attr);

            ////var newAttr = new CustomAttribute(attr.Constructor)
            ////{
            ////    ConstructorArguments = 
            ////        {
            ////            new CustomAttributeArgument(
            ////                 attr.ConstructorArguments[0].Type, 
            ////                 Guid.NewGuid())
            ////         }
            ////};


            //definition.CustomAttributes.Add(newAttr);
            

            _log.Normal("Finished modifications; writing to stream");
            definition.Write(stream);

            definition.Write(_location.FullPath + ".modified");

            _log.Normal("done");
        }


        

        public void Load()
        {
            using (var stream = new System.IO.MemoryStream())
            {
                ApplyModifications(stream); // modified assembly written to memory

                // load dll from byte stream. This is done simply to avoid unnecessary file i/o;
                // File.ReadAllBytes works too but then we waste time writing a file and then immediately
                // reading it again a single time

                _log.Normal("loading assembly");
                _loaded = Assembly.Load(stream.GetBuffer());
                _log.Normal("load finished");
                if (_loaded.IsNull())
                    throw new InvalidOperationException("Failed to load byte stream as Assembly");

                // note: looks like if we already have a dependency in memory, the new version of DLL might not
                // be correctly loaded. Might need to tweak dependencies as well, even if they're not reloadable
                // note: but we only want those in GameData of course, otherwise we'll epicfail by duplicating
                // mscorlib, Assembly-CSharp etc
                _log.Normal("Dependencies of " + _loaded.GetName().Name + ":" + System.Environment.NewLine +
                            string.Join(System.Environment.NewLine,
                                _loaded.GetReferencedAssemblies().Select(ra => ra.Name).ToArray()));


                _partModuleLoader = _loaderFactory.CreatePartModuleLoader(_loaded, _log);
                _addonLoader = _loaderFactory.CreateAddonLoader(_loaded, _log);


                _addonSceneChangeSubscription = _levelLoadedEvent.AddListener(OnGameSceneWasLoaded);
            }
        }



        public void Unload()
        {
            _log.Normal("Unloading " + Name);

            _addonSceneChangeSubscription.Dispose(); 
            _addonSceneChangeSubscription = null;

            _addonLoader.Dispose();
            _addonLoader = null;
           

            _partModuleLoader.Dispose();
            _partModuleLoader = null;
        }



        private void OnGameSceneWasLoaded(GameScenes scene)
        {
            _addonLoader.LoadForScene(_queryFactory.GetStartupSceneFromGameSceneQuery().Get(scene));
        }


        public string Name
        {
            get { return _location.Name; }
        }
    }
}
