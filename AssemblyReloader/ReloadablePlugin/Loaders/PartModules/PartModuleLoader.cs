using System;
using AssemblyReloader.Game;
using ReeperCommon.Logging;

namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules
{
    public class PartModuleLoader : IPartModuleLoader
    {
        private readonly ILog _log = new DebugLog("PartModuleLoader");


        public PartModuleLoader(ILog log)
        {
            if (log == null) throw new ArgumentNullException("log");

            _log = log;
        }


        public void Load(ILoadedAssemblyHandle handle)
        {
            if (handle == null) throw new ArgumentNullException("handle");

            _log.Verbose("Loading PartModules from " + handle.LoadedAssembly.name);

            //_log.Verbose("Loading " + type.FullName);

            //var descriptions = _descriptorFactory.Create(type).ToList();

            //descriptions.ForEach(LoadPartModule);
        }



        //private void LoadPartModule([NotNull] PartModuleDescriptor description)
        //{
        //    if (description == null) throw new ArgumentNullException("description");

        //    // not included in list because it won't be started
        //    _partModuleFactory.Create(description.Prefab, description.Type, description.Config);

        //    foreach (var loadedInstance in _loadedPrefabProvider.Get(description.Prefab))
        //    {
        //        var stored = _configNodeQueue.Retrieve(new KeyValuePair<uint, ITypeIdentifier>(loadedInstance.FlightID, description.Identifier));
        //        var config = _useConfigNodeSnapshotIfAvailable() && stored.Any() ? stored.Single() : description.Config;

        //        _partModuleFactory.Create(loadedInstance, description.Type, config);
        //    }
        //}
    }
}
