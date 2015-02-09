using System;
using System.Collections.Generic;
using ReeperCommon.Logging;

namespace AssemblyReloader.Loaders.PMLoader
{
    public class PartModuleLoader : IDisposable
    {
        private readonly IEnumerable<Type> _partModules;
        private readonly IPartModuleInfoFactory _pmiFactory;
        private readonly ILog _log;

        public PartModuleLoader(
            IEnumerable<Type> partModules, 
            IPartModuleInfoFactory pmiFactory,
            ILog log)
        {
            if (partModules == null) throw new ArgumentNullException("partModules");
            if (pmiFactory == null) throw new ArgumentNullException("pmiFactory");
            if (log == null) throw new ArgumentNullException("log");

            _partModules = partModules;
            _pmiFactory = pmiFactory;
            _log = log;
        }


        ~PartModuleLoader()
        {
            Dispose(false);
        }


        public void Dispose()
        {
            Dispose(true);
        }


        private void Dispose(bool managed)
        {
            if (managed)
            {
                // todo: destroy
            }

            GC.SuppressFinalize(this);
        }


        public void LoadPartModulesIntoPrefabs()
        {
            _log.Verbose("Begin loading PartModules into prefab GameObjects");

            foreach (var pm in _partModules)
                LoadPartModuleIntoPrefab(pm);

            _log.Verbose("Finished loading PartModules into prefabs");
        }


        public void LoadPartModulesIntoFlight()
        {
            throw new NotImplementedException();
        }


        private void LoadPartModuleIntoPrefab(Type pm)
        {
            var info = _pmiFactory.Create(pm);

        }


        //private IEnumerable<PartModuleInfo> FindPrefabs()
        //{
        //    var prefabInfo = new List<PartModuleInfo>();

        //    PartLoader.LoadedPartsList.ForEach(ap =>
        //    {
        //        var cfg = _configProvider.Get(ap);
        //        if (!cfg.Any()) return;

        //        var moduleCfgs = _partModules.SelectMany(pmType =>
        //            _moduleConfigProvider.Get(cfg.Single(), pmType.Name));

        //        prefabInfo.AddRange(CreatePartModuleInfoForTypeOnPart(
        //    });

        //    return prefabInfo;
        //}


        //// note: IEnumerable because it's conceivable the part has multiple duplicate entries for the same PartModule
        //private IEnumerable<PartModuleInfo> CreatePartModuleInfoForTypeOnPart(
        //    Type pmType, 
        //    ConfigNode partConfig,
        //    AvailablePart part)
        //{
        //    var configs =
        //        _moduleConfigProvider.Get(partConfig, pmType.Name)
        //            .Select(config => new PartModuleInfo(part.partPrefab, config, pmType));
        //}
    }
}
