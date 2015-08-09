using System;
using AssemblyReloader.StrangeIoC.extensions.implicitBind;
using AssemblyReloader.StrangeIoC.extensions.injector.api;
using ReeperCommon.Containers;

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
            return HighLogic.CurrentGame != null
            ? Maybe<IGame>.With(_kspFactory.Create(HighLogic.CurrentGame)) 
            : Maybe<IGame>.None;
        }
    }
}
