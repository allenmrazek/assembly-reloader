using System;
using AssemblyReloader.Game;
using AssemblyReloader.Properties;
using AssemblyReloader.StrangeIoC.extensions.implicitBind;
using AssemblyReloader.StrangeIoC.extensions.injector.api;
using AssemblyReloader.Unsorted;
using ReeperCommon.Containers;

namespace AssemblyReloader.ReloadablePlugin.Loaders.ScenarioModules
{
    [Implements(typeof(IGetCurrentGame), InjectionBindingScope.CROSS_CONTEXT)]
    public class GetCurrentGame : IGetCurrentGame
    {
        private readonly IGetTypeIdentifier _getTypeIdentifier;

        public GetCurrentGame([NotNull] IGetTypeIdentifier getTypeIdentifier)
        {
            if (getTypeIdentifier == null) throw new ArgumentNullException("getTypeIdentifier");
            _getTypeIdentifier = getTypeIdentifier;
        }


        public Maybe<IGame> Get()
        {
            return HighLogic.CurrentGame == null ? Maybe<IGame>.None : Maybe<IGame>.With(new KspGame(HighLogic.CurrentGame, _getTypeIdentifier));
        }
    }
}
