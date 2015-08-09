using System;
using System.Collections.ObjectModel;
using System.Linq;
using AssemblyReloader.StrangeIoC.extensions.implicitBind;
using AssemblyReloader.StrangeIoC.extensions.injector.api;

namespace AssemblyReloader.ReloadablePlugin.Loaders.ScenarioModules
{
    [Implements(typeof(IGame), InjectionBindingScope.CROSS_CONTEXT)]
    public class KspGame : IGame
    {
        private readonly global::Game _game;
        private readonly IKspFactory _kspFactory;


        public KspGame(global::Game game, IKspFactory kspFactory)
        {
            if (game == null) throw new ArgumentNullException("game");
            if (kspFactory == null) throw new ArgumentNullException("kspFactory");

            _game = game;
            _kspFactory = kspFactory;
        }


        public ReadOnlyCollection<IProtoScenarioModule> Scenarios
        {
            get { return _game.scenarios.Select(psm => _kspFactory.Create(psm)).ToList().AsReadOnly(); }
        }
    }
}
