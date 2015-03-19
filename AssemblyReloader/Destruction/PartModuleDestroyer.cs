using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssemblyReloader.Game;
using AssemblyReloader.Generators;
using AssemblyReloader.Queries;
using AssemblyReloader.Repositories;
using ReeperCommon.Logging;

namespace AssemblyReloader.Destruction
{
    public class PartModuleDestroyer : IPartModuleDestroyer
    {
        private readonly IUnityObjectDestroyer _unityDestroyer;
        private readonly IPartIsPrefabQuery _isPrefabQuery;
        private readonly IKspFactory _kspFactory;
        private readonly IUniqueFlightIdGenerator _uniqueIdGenerator;
        private readonly IFlightConfigRepository _configRepository;
        private readonly ITypeIdentifierQuery _typeIdentifieryQuery;
        private readonly ILog _log;

        public PartModuleDestroyer(
            IUnityObjectDestroyer unityDestroyer,
            IPartIsPrefabQuery isPrefabQuery,
            IKspFactory kspFactory,
            IUniqueFlightIdGenerator uniqueIdGenerator,
            IFlightConfigRepository configRepository,
            ITypeIdentifierQuery typeIdentifieryQuery,
            ILog log)
        {
            if (unityDestroyer == null) throw new ArgumentNullException("unityDestroyer");
            if (kspFactory == null) throw new ArgumentNullException("kspFactory");
            if (uniqueIdGenerator == null) throw new ArgumentNullException("uniqueIdGenerator");
            if (configRepository == null) throw new ArgumentNullException("configRepository");
            if (typeIdentifieryQuery == null) throw new ArgumentNullException("typeIdentifieryQuery");
            if (log == null) throw new ArgumentNullException("log");

            _unityDestroyer = unityDestroyer;
            _isPrefabQuery = isPrefabQuery;
            _kspFactory = kspFactory;
            _uniqueIdGenerator = uniqueIdGenerator;
            _configRepository = configRepository;
            _typeIdentifieryQuery = typeIdentifieryQuery;
            _log = log;
        }


        public void Destroy(PartModule pm)
        {
            if (pm == null) throw new ArgumentNullException("pm");

            _log.Normal("Destroying " + pm.GetType().FullName + " on " + pm.part.flightID);

            if (!_isPrefabQuery.Get(_kspFactory.Create(pm.part)))
            {
                if (pm.part.flightID == 0)
                    pm.part.flightID = _uniqueIdGenerator.Get();

                _log.Normal("FlightID is now " + pm.part.flightID);

                var cfg = new ConfigNode();

                pm.Save(cfg);

                _log.Normal("Saving ConfigNode: {0}", cfg.ToString());

                _configRepository.Store(pm.part.flightID, _typeIdentifieryQuery.Get(pm.GetType()), cfg);
            }

            _unityDestroyer.Destroy(pm);
        }
    }
}
