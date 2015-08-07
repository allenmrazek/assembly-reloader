using System;
using AssemblyReloader.Properties;
using AssemblyReloader.ReloadablePlugin.Loaders.ScenarioModules;
using AssemblyReloader.StrangeIoC.extensions.implicitBind;
using AssemblyReloader.StrangeIoC.extensions.injector.api;

namespace AssemblyReloader.Game
{
    [Implements(typeof(IKspFactory), InjectionBindingScope.CROSS_CONTEXT)]
// ReSharper disable once ClassNeverInstantiated.Global
    public class KspFactory : IKspFactory
    {
        private readonly IScenarioRunnerProvider _scenarioRunnerProvider;

        public KspFactory([NotNull] IScenarioRunnerProvider scenarioRunnerProvider)
        {
            if (scenarioRunnerProvider == null) throw new ArgumentNullException("scenarioRunnerProvider");
            _scenarioRunnerProvider = scenarioRunnerProvider;
        }

        public IPart Create(Part part)
        {
            return new KspPart(part, this);
        }

        public IAvailablePart Create(AvailablePart part)
        {
            return new KspAvailablePart(part, this);
        }

        public IVessel Create(Vessel vessel)
        {
            return new KspVessel(vessel, this);
        }

        public IProtoScenarioModule Create(ProtoScenarioModule psm)
        {
            return new KspProtoScenarioModule(psm, _scenarioRunnerProvider);
        }

        public IScenarioRunner Create(ScenarioRunner runner)
        {
            if (runner == null) throw new ArgumentNullException("runner");

            return new KspScenarioRunner(runner, this);
        }
    }
}
