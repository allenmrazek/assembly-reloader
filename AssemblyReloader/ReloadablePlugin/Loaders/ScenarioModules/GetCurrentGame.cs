extern alias KSP;
using System;
using ReeperCommon.Containers;
using strange.extensions.injector.api;

namespace AssemblyReloader.ReloadablePlugin.Loaders.ScenarioModules
{
    [Implements(typeof(IGetCurrentGame), InjectionBindingScope.CROSS_CONTEXT)]
    public class GetCurrentGame : IGetCurrentGame
    {
        private readonly IKspFactory _kspFactory;

        public GetCurrentGame(IKspFactory kspFactory)
        {
            if (kspFactory == null) throw new ArgumentNullException("kspFactory");

            _kspFactory = kspFactory;
        }


        public Maybe<IGame> Get()
        {
            return KSP::HighLogic.CurrentGame != null
            ? Maybe<IGame>.With(_kspFactory.Create(KSP::HighLogic.CurrentGame)) 
            : Maybe<IGame>.None;
        }
    }
}
