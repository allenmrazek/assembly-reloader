using System;
using AssemblyReloader.Game;
using AssemblyReloader.StrangeIoC.extensions.injector;
using ReeperCommon.Logging;

namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules
{
    public class PartModuleLoader : IPartModuleLoader
    {
        private readonly IGetTypesDerivedFrom<PartModule> _getPartModuleTypes;
        private readonly ILog _log = new DebugLog("PartModuleLoader");


        public PartModuleLoader(
            IGetTypesDerivedFrom<PartModule> getPartModuleTypes,
            [Name(LogKeys.PartModuleLoader)] ILog log)
        {
            if (getPartModuleTypes == null) throw new ArgumentNullException("getPartModuleTypes");
            if (log == null) throw new ArgumentNullException("log");

            _getPartModuleTypes = getPartModuleTypes;
            _log = log;
        }


        //public void Load(ILoadedAssemblyHandle handle)
        //{
        //    if (handle == null) throw new ArgumentNullException("handle");

        //    //_log.Verbose("Loading " + type.FullName);

        //    //var descriptions = _descriptorFactory.Create(type).ToList();

        //    //descriptions.ForEach(LoadPartModule);
        //}



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
        public void LoadPrefabs(ILoadedAssemblyHandle handle)
        {
            _log.Verbose("Load prefabs");
        }

        public void LoadInstances(ILoadedAssemblyHandle handle)
        {
            _log.Verbose("Load instances");
        }
    }
}
