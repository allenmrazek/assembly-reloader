using System;
using System.Linq;
using AssemblyReloader.Properties;
using AssemblyReloader.Queries;
using ReeperCommon.Extensions;

namespace AssemblyReloader.Game
{
    public class KspGame : IGame
    {
        private readonly global::Game _game;
        private readonly ITypeIdentifierQuery _typeIdentifierQuery;


        public KspGame(
            [NotNull] global::Game game, 
            [NotNull] ITypeIdentifierQuery typeIdentifierQuery)
        {
            if (game == null) throw new ArgumentNullException("game");
            if (typeIdentifierQuery == null) throw new ArgumentNullException("typeIdentifierQuery");

            _game = game;
            _typeIdentifierQuery = typeIdentifierQuery;
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
            CheckForDuplicateProtoScenarioModule(_typeIdentifierQuery.Get(scnType).Identifier);

            _game.AddProtoScenarioModule(scnType, targetScenes);
            ScenarioRunner.SetProtoModules(_game.scenarios);
        }



        private void CheckForDuplicateProtoScenarioModule([NotNull] string moduleName)
        {
            if (string.IsNullOrEmpty(moduleName)) throw new ArgumentException("moduleName");

            if (_game.scenarios.Any(psm => psm.moduleName == moduleName))
                throw new Exception("A ProtoScenarioModule of type " + moduleName +
                                    " already exists");
        }
    }
}
