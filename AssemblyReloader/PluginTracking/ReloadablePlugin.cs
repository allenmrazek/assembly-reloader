using System;
using System.Linq;
using System.Reflection;
using AssemblyReloader.ILModifications;
using AssemblyReloader.Loaders.AddonLoader;
using AssemblyReloader.Loaders.PMLoader;
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

        private readonly IQueryFactory _queryFactory;

        private readonly IFile _location;
        private readonly IAddonLoaderFactory _addonLoaderFactory;
        private readonly IModifiedAssemblyFactory _massemblyFactory;
        private readonly ILog _log;
        
        private IEventSubscription _addonSceneChangeSubscription;



        public ReloadablePlugin(
            IFile location,

            IAddonLoaderFactory addonLoaderFactory,
            IModifiedAssemblyFactory massemblyFactory,

            ILog log,
            IQueryFactory queryFactory)
        {
            if (location == null) throw new ArgumentNullException("location");
            if (addonLoaderFactory == null) throw new ArgumentNullException("addonLoaderFactory");
            if (massemblyFactory == null) throw new ArgumentNullException("massemblyFactory");
            if (log == null) throw new ArgumentNullException("log");
            if (queryFactory == null) throw new ArgumentNullException("queryFactory");

            _location = location;
            _addonLoaderFactory = addonLoaderFactory;
            _massemblyFactory = massemblyFactory;
            _log = log;
            _queryFactory = queryFactory;
        }




        

        public void Load()
        {
            using (var stream = new System.IO.MemoryStream())
            {
                var original = _massemblyFactory.Create(_location);

                _log.Debug("Renaming assembly");
                original.Rename(Guid.NewGuid());

                original.Write(stream);

                var result = original.Load(stream);

                if (!result.Any())
                {
                    _log.Error("Failed to read modified assembly definition from memory stream!");
                    return;
                }

                _loaded = result.Single();

                _addonLoader = _addonLoaderFactory.Create(_loaded, _log);
            }
        }



        public void Unload()
        {
            if (_loaded.IsNull()) return; // nothing to unload in the first place

            _log.Normal("Unloading " + Name);

            _addonSceneChangeSubscription.Dispose(); 
            _addonSceneChangeSubscription = null;

            _addonLoader.Dispose();
            _addonLoader = null;
        }



        public string Name
        {
            get { return _location.Name; }
        }
    }
}
