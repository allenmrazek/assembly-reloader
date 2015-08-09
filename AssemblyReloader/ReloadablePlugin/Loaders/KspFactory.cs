using System;
using AssemblyReloader.Game;
using AssemblyReloader.ReloadablePlugin.Loaders.ScenarioModules;
using AssemblyReloader.StrangeIoC.extensions.implicitBind;
using AssemblyReloader.StrangeIoC.extensions.injector.api;

namespace AssemblyReloader.ReloadablePlugin.Loaders
{
    [Implements(typeof(IKspFactory), InjectionBindingScope.CROSS_CONTEXT)]
// ReSharper disable once ClassNeverInstantiated.Global
    public class KspFactory : IKspFactory
    {
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
            return new KspProtoScenarioModule(psm);
        }

        public IGame Create(global::Game game)
        {
            if (game == null) throw new ArgumentNullException("game");

            return new KspGame(game, this);
        }
    }
}
