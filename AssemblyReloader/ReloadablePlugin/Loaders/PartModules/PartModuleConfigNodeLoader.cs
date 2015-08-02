using System;
using System.Linq;
using AssemblyReloader.Game;
using AssemblyReloader.StrangeIoC.extensions.injector;
using ReeperCommon.Logging;

namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules
{
    public class PartModuleConfigNodeLoader
    {
        private readonly IPartModuleSettings _partModuleSettings;
        private readonly IGetPartIsPrefab _partIsPrefabQuery;
        private readonly IPartModuleConfigNodeSnapshotRepository _configNodeRepository;
        private readonly ILog _log;


        public PartModuleConfigNodeLoader(
            IPartModuleSettings partModuleSettings,
            IGetPartIsPrefab partIsPrefabQuery,
            IPartModuleConfigNodeSnapshotRepository configNodeRepository,
            [Name(LogKeys.PartModuleLoader)] ILog log)
        {
            if (partModuleSettings == null) throw new ArgumentNullException("partModuleSettings");
            if (partIsPrefabQuery == null) throw new ArgumentNullException("partIsPrefabQuery");
            if (configNodeRepository == null) throw new ArgumentNullException("configNodeRepository");
            if (log == null) throw new ArgumentNullException("log");

            _partModuleSettings = partModuleSettings;
            _partIsPrefabQuery = partIsPrefabQuery;
            _configNodeRepository = configNodeRepository;
            _log = log;
        }


        public void LoadConfigNodeFor(IPart part, PartModule target, PartModuleDescriptor descriptor)
        {
            if (target == null) throw new ArgumentNullException("target");
            if (descriptor == null) throw new ArgumentNullException("descriptor");

            var stored = _configNodeRepository.Retrieve(target.part.flightID, descriptor.Identifier);
            var config = 
                !_partIsPrefabQuery.Get(part) &&
                _partModuleSettings.SaveAndReloadPartModuleConfigNodes && 
                stored.Any() ? stored.Single() : descriptor.Config;

            try
            {
                target.Load(config);
            }
            catch (Exception e)
            {
                _log.Warning("Caught exception while loading PartModule " + descriptor.Identifier + " on " +
                             part.FlightID + ": " + e);
            }
        }
    }
}
