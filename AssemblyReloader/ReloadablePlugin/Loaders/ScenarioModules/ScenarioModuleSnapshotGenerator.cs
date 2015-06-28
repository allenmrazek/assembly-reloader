using System;
using System.Linq;
using AssemblyReloader.Game;
using AssemblyReloader.Game.Providers;
using AssemblyReloader.Properties;
using ReeperCommon.Logging;

namespace AssemblyReloader.ReloadablePlugin.Loaders.ScenarioModules
{
    public class ScenarioModuleSnapshotGenerator : IScenarioModuleSnapshotGenerator
    {
        private readonly ICurrentGameProvider _gameProvider;
        private readonly ILog _log;

        public ScenarioModuleSnapshotGenerator([NotNull] ICurrentGameProvider gameProvider,
            [NotNull] ILog log)
        {
            if (gameProvider == null) throw new ArgumentNullException("gameProvider");
            if (log == null) throw new ArgumentNullException("log");

            _gameProvider = gameProvider;
            _log = log;
        }


        // works a bit differently than PartModules; the game internally keeps a ConfigNode
        // of ScenarioModule state so a "snapshot" here is really just updating that node
        // and fixing things if the snapshot fails
        public void Snapshot([NotNull] ScenarioModule instance, [NotNull] IProtoScenarioModule psm)
        {
            if (instance == null) throw new ArgumentNullException("instance");
            if (psm == null) throw new ArgumentNullException("psm");
            if (!_gameProvider.Get().Any()) throw new InvalidOperationException("Game is not in a valid state");

            // take a snapshot of the current state of the ScenarioModule so we can reuse it to load
            // the next version
            var snapshot = new ConfigNode("SCENARIO");
            bool snapshotSuccess = TryToSaveScenarioModuleState(instance, snapshot);
            var game = _gameProvider.Get().Single();

            if (!game.RemoveProtoScenarioModule(instance.GetType()))
                throw new Exception("Failed to remove proto scenario module of " + instance.GetType().FullName);
            
            if (snapshotSuccess)
                game.AddProtoScenarioModule(snapshot);
            else game.AddProtoScenarioModule(instance.GetType(), psm.TargetScenes);


        }


        private bool TryToSaveScenarioModuleState(ScenarioModule sm, ConfigNode node)
        {
            try
            {
                // note: scene is correctly saved inside node
                sm.Save(node);

                return true;
            }
            catch (Exception)
            {
                _log.Warning("Failed to save snapshot of " + sm.GetType().FullName);

                return false;
            }
        }
    }
}
