using System;
using AssemblyReloader.Annotations;

namespace AssemblyReloader.Game
{
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
    }
}
