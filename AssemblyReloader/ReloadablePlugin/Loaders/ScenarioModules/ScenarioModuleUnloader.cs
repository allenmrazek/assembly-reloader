using System;
using System.Linq;
using AssemblyReloader.Game;
using AssemblyReloader.StrangeIoC.extensions.implicitBind;
using AssemblyReloader.StrangeIoC.extensions.injector;
using ReeperCommon.Logging;

namespace AssemblyReloader.ReloadablePlugin.Loaders.ScenarioModules
{
    [Implements(typeof(IScenarioModuleUnloader))]
    public class ScenarioModuleUnloader : IScenarioModuleUnloader
    {
        private readonly IGetTypesDerivedFrom<ScenarioModule> _smTypeQuery;
        private readonly IGetProtoScenarioModules _psmQuery;
        private readonly IScenarioModuleDestroyer _smDestroyer;
        private readonly ILog _log;


        public ScenarioModuleUnloader(
            IGetTypesDerivedFrom<ScenarioModule> smTypeQuery,
            IGetProtoScenarioModules psmQuery,
            IScenarioModuleDestroyer smDestroyer,
            [Name(LogKeys.ScenarioModuleUnloader)] ILog log)
        {
            if (smTypeQuery == null) throw new ArgumentNullException("smTypeQuery");
            if (psmQuery == null) throw new ArgumentNullException("psmQuery");
            if (smDestroyer == null) throw new ArgumentNullException("smDestroyer");
            if (log == null) throw new ArgumentNullException("log");

            _smTypeQuery = smTypeQuery;
            _psmQuery = psmQuery;
            _smDestroyer = smDestroyer;
            _log = log;
        }


        public void Unload(ILoadedAssemblyHandle handle)
        {
            if (handle == null) throw new ArgumentNullException("handle");

            foreach (var smType in _smTypeQuery.Get(handle.LoadedAssembly.assembly))
                UnloadScenarioModule(smType);
        }


        private void UnloadScenarioModule(Type smType)
        {
            _log.Debug("Unloading ScenarioModule " + smType.FullName);

            // there should only be at most one instance ever unless something is broken or has changed
            var psms = _psmQuery.Get(smType).ToList();

            if (psms.Count > 1)
                throw new MultipleScenarioModuleInstancesException(smType);

            if (!psms.Any())
            {
                _log.Debug("No ProtoScenarioModules for " + smType.FullName + " found in current scene");
                return;
            }

            var psm = psms.Single();

            if (!psm.moduleRef.Any())
            {
                // how odd: could be a problem. Better make noise
                _log.Warning("ProtoScenarioModule for " + psm.moduleName +
                             " does not contain a reference to an existing instance");
                return;
            }

            _smDestroyer.Destroy(psm);
        }
    }
}