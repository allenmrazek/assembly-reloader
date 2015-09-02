extern alias KSP;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using strange.extensions.implicitBind;
using strange.extensions.injector.api;

namespace AssemblyReloader.ReloadablePlugin.Loaders.ScenarioModules
{
    [Implements(typeof(IGame), InjectionBindingScope.CROSS_CONTEXT)]
    public class KspGame : IGame
    {
        private readonly KSP::Game _game;
        private readonly IKspFactory _kspFactory;


        public KspGame(KSP::Game game, IKspFactory kspFactory)
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
