using System;
using AssemblyReloader.Properties;

namespace AssemblyReloader.Game
{
    [Implements(typeof(IGameObjectProvider))]
    public class KspFactory : IKspFactory
    {
        private readonly IGameObjectProvider _gameObjectProvider;

        public KspFactory([NotNull] IGameObjectProvider gameObjectProvider)
        {
            if (gameObjectProvider == null) throw new ArgumentNullException("gameObjectProvider");
            _gameObjectProvider = gameObjectProvider;
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
            return new KspProtoScenarioModule(psm, _gameObjectProvider);
        }
    }
}
