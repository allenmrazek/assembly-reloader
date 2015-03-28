using System;
using System.Linq;
using AssemblyReloader.Annotations;
using ReeperCommon.Containers;

namespace AssemblyReloader.Game
{
    public class KspProtoScenarioModule : IProtoScenarioModule
    {
        private readonly ProtoScenarioModule _psm;
        private readonly IScenarioRunnerProvider _scenarioRunnerProvider;

        public KspProtoScenarioModule(
            ProtoScenarioModule psm, 
            [NotNull] IScenarioRunnerProvider scenarioRunnerProvider)
        {
            if (scenarioRunnerProvider == null) throw new ArgumentNullException("scenarioRunnerProvider");
            _psm = psm;
            _scenarioRunnerProvider = scenarioRunnerProvider;
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
            var inst = _scenarioRunnerProvider.Get();

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
