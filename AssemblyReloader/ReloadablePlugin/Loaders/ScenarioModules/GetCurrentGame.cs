using System;
using ReeperCommon.Containers;
using strange.extensions.implicitBind;
using strange.extensions.injector.api;

namespace AssemblyReloader.ReloadablePlugin.Loaders.ScenarioModules
{
    [Implements(typeof(IGetCurrentGame), InjectionBindingScope.CROSS_CONTEXT)]
// ReSharper disable once UnusedMember.Global
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
            return HighLogic.CurrentGame
                .Return(g => _kspFactory.Create(g).ToMaybe(), Maybe<IGame>.None);
        }
    }
}
