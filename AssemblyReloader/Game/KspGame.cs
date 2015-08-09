using System;
using System.Linq;
using AssemblyReloader.Properties;
using AssemblyReloader.Unsorted;
using ReeperCommon.Extensions;

namespace AssemblyReloader.Game
{
    public class KspGame : IGame
    {
        private readonly global::Game _game;
        private readonly IGetTypeIdentifier _getTypeIdentifier;


        public KspGame(
            [NotNull] global::Game game, 
            [NotNull] IGetTypeIdentifier getTypeIdentifier)
        {
            if (game == null) throw new ArgumentNullException("game");
            if (getTypeIdentifier == null) throw new ArgumentNullException("getTypeIdentifier");

            _game = game;
            _getTypeIdentifier = getTypeIdentifier;
        }


        public bool RemoveProtoScenarioModule([NotNull] Type scnType)
        {
            if (scnType == null) throw new ArgumentNullException("scnType");
            return _game.RemoveProtoScenarioModule(scnType);
        }


        public void AddProtoScenarioModule(ConfigNode scnConfig)
        {
            CheckForDuplicateProtoScenarioModule(scnConfig.Parse("name", string.Empty));


            _game.scenarios.Add(new ProtoScenarioModule(scnConfig));
            ScenarioRunner.SetProtoModules(_game.scenarios);
        }

        public void AddProtoScenarioModule(Type scnType, params GameScenes[] targetScenes)
        {
            CheckForDuplicateProtoScenarioModule(_getTypeIdentifier.Get(scnType).Identifier);

            _game.AddProtoScenarioModule(scnType, targetScenes);
            ScenarioRunner.SetProtoModules(_game.scenarios);
        }



        private void CheckForDuplicateProtoScenarioModule([NotNull] string moduleName)
        {
            if (string.IsNullOrEmpty(moduleName)) throw new ArgumentException("Cannot be null or empty", "moduleName");

            if (_game.scenarios.Any(psm => psm.moduleName == moduleName))
                throw new DuplicateProtoScenarioModuleException(moduleName);
        }
    }
}
