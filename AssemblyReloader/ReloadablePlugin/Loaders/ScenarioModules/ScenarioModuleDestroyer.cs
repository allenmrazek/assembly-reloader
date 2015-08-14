extern alias KSP;
using System;
using System.Linq;
using ReeperCommon.Containers;
using ReeperCommon.Logging;

namespace AssemblyReloader.ReloadablePlugin.Loaders.ScenarioModules
{
    [Implements(typeof(IScenarioModuleDestroyer))]
// ReSharper disable once UnusedMember.Global
    public class ScenarioModuleDestroyer : IScenarioModuleDestroyer
    {
        private readonly SignalAboutToDestroyMonoBehaviour _aboutToDestroySignal;
        private readonly ILog _log;

        public ScenarioModuleDestroyer(
            SignalAboutToDestroyMonoBehaviour aboutToDestroySignal,
            [Name(LogKey.ScenarioModuleDestroyer)] ILog log)
        {
            if (aboutToDestroySignal == null) throw new ArgumentNullException("aboutToDestroySignal");
            if (log == null) throw new ArgumentNullException("log");

            _aboutToDestroySignal = aboutToDestroySignal;
            _log = log;
        }


        public void Destroy(IProtoScenarioModule protoScenarioModule)
        {
            if (protoScenarioModule == null) throw new ArgumentNullException("protoScenarioModule");

            if (!protoScenarioModule.moduleRef.Any())
                throw new NoScenarioModuleReferenceException(protoScenarioModule);

            var target = protoScenarioModule.moduleRef.Single();

            _log.Debug("Destroying ScenarioModule " + protoScenarioModule.moduleName);
            _aboutToDestroySignal.Dispatch(target);

            KSP::ScenarioRunner.RemoveModule(target);
            protoScenarioModule.moduleRef = Maybe<KSP::ScenarioModule>.None;
        }
    }
}
