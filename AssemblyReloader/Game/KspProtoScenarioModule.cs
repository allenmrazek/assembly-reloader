using System;
using System.Linq;
using AssemblyReloader.Properties;
using ReeperCommon.Containers;

namespace AssemblyReloader.Game
{
    public class KspProtoScenarioModule : IProtoScenarioModule
    {
        private readonly ProtoScenarioModule _psm;
        private readonly IGameObjectProvider _gameObjectProvider;

        public KspProtoScenarioModule(
            ProtoScenarioModule psm, 
            [NotNull] IGameObjectProvider gameObjectProvider)
        {
            if (gameObjectProvider == null) throw new ArgumentNullException("gameObjectProvider");
            _psm = psm;
            _gameObjectProvider = gameObjectProvider;
        }


        public Maybe<ScenarioModule> moduleRef
        {
            get { return _psm.moduleRef != null ? Maybe<ScenarioModule>.With(_psm.moduleRef) : Maybe<ScenarioModule>.None; }
            set { _psm.moduleRef = value.SingleOrDefault(); }
        }


        public string moduleName
        {
            get { return _psm.moduleName; }
        }


        public Maybe<ScenarioModule> Load()
        {
            var inst = _gameObjectProvider.Get();

            if (!inst.Any())
                throw new ArgumentException("Failed to retrieve scenario runner");

            return _psm.Load(inst.First()) != null ? moduleRef : Maybe<ScenarioModule>.None;
        }

        public GameScenes[] TargetScenes
        {
            get { return _psm.targetScenes.ToArray(); }
            set { _psm.SetTargetScenes(value); }
        }
    }
}
