using System;
using System.Linq;
using AssemblyReloader.StrangeIoC.extensions.implicitBind;
using AssemblyReloader.StrangeIoC.extensions.injector;
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
            [Name(LogKeys.ScenarioModuleDestroyer)] ILog log)
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

            UnityEngine.Object.Destroy(target);
            protoScenarioModule.moduleRef = Maybe<ScenarioModule>.None;
        }
    }
}
