using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using AssemblyReloader.Events;
using AssemblyReloader.ILModifications;
using AssemblyReloader.Loaders;
using AssemblyReloader.Loaders.Factories;
using AssemblyReloader.Providers;
using Mono.Cecil;
using ReeperCommon.Extensions.Object;
using ReeperCommon.Logging;

namespace AssemblyReloader.AssemblyTracking.Implementations
{

    /// <summary>
    /// This object tracks a particular reloadable dll (based on its location). This is necessary
    /// because the state of a previous version of the assembly may affect how we load the next one;
    /// specifically, we may need to modify changes the previous version made such as removing PartModules
    /// from prefabs
    /// </summary>
    class ReloadableAssembly : IReloadableAssembly
    {
        private Assembly _loaded;
        private IAddonLoader _addonLoader;


        private readonly IReloadableIdentity _reloadableIdentity;
        private readonly ILoaderFactory _loaderFactory;
        private readonly IGameEventSubscriber<GameScenes> _levelLoadedEvent;
        private readonly ILog _log;
        private readonly QueryProvider _queryProvider;

        private IGameEventSubscription _addonSceneChangeSubscription;



        public ReloadableAssembly(
            IReloadableIdentity reloadableIdentity,
            ILoaderFactory loaderFactory,
            IGameEventSubscriber<GameScenes> levelLoadedEvent,
                ILog log,
            QueryProvider queryProvider)
        {
            if (reloadableIdentity == null) throw new ArgumentNullException("reloadableIdentity");
            if (loaderFactory == null) throw new ArgumentNullException("loaderFactory");
            if (levelLoadedEvent == null) throw new ArgumentNullException("levelLoadedEvent");
            if (log == null) throw new ArgumentNullException("log");
            if (queryProvider == null) throw new ArgumentNullException("queryProvider");

            _reloadableIdentity = reloadableIdentity;
            _loaderFactory = loaderFactory;
            _levelLoadedEvent = levelLoadedEvent;
            _log = log;
            _queryProvider = queryProvider;
        }




        private void ApplyModifications(System.IO.MemoryStream stream)
        {
            if (stream == null) throw new ArgumentNullException("stream");

            var definition = AssemblyDefinition.ReadAssembly(_reloadableIdentity.Location);

            var modifier = new ModifyPluginIdentity();

            modifier.Rename(definition, Guid.NewGuid());

            definition.MainModule.GetTypes().ToList().ForEach(td =>
            {
                td.Namespace = Guid.NewGuid() + "." + td.Namespace;
                td.Name = Guid.NewGuid() + "." + td.Name;
            });

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

            definition.Write(_reloadableIdentity.Location + ".modified");

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

                // note: looks like if we already have a dependency in memory, the new version of DLL won't
                // be correctly loaded. Might need to tweak dependencies as well, even if they're not reloadable
                // note: but we only want those in GameData of course, otherwise we'll epicfail by duplicating
                // mscorlib, Assembly-CSharp etc
                _log.Normal("Dependencies of " + _loaded.GetName().Name + ":" + System.Environment.NewLine +
                            string.Join(System.Environment.NewLine,
                                _loaded.GetReferencedAssemblies().Select(ra => ra.Name).ToArray()));


                _addonLoader = _loaderFactory.CreateAddonLoader(_loaded);


                _addonSceneChangeSubscription = _levelLoadedEvent.AddListener(_addonLoader.LoadAddonsForScene);
            }
        }



        public void Unload()
        {
            _addonSceneChangeSubscription.Dispose(); 
            _addonSceneChangeSubscription = null;

            _addonLoader.Dispose();
            _addonLoader = null;
        }

        public IReloadableIdentity ReloadableIdentity
        {
            get { return _reloadableIdentity; }
        }


        public void StartAddons(GameScenes scene)
        {
            _addonLoader.LoadAddonsForScene(scene);
        }
    }
}
