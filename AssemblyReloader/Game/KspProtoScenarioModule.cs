﻿using System;
using System.Linq;
using AssemblyReloader.Properties;
using AssemblyReloader.ReloadablePlugin.Loaders.ScenarioModules;
using ReeperCommon.Containers;
using ReeperCommon.Logging;

namespace AssemblyReloader.Game
{
    public class KspProtoScenarioModule : IProtoScenarioModule
    {
        private readonly ProtoScenarioModule _psm;
        private readonly IScenarioRunnerProvider _scenarioRunnerProvider;


        public KspProtoScenarioModule(
            ProtoScenarioModule psm, 
            IScenarioRunnerProvider scenarioRunnerProvider)
        {
            if (psm == null) throw new ArgumentNullException("psm");
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


        public GameScenes[] TargetScenes
        {
            get { return _psm.targetScenes.ToArray(); }
            set { _psm.SetTargetScenes(value); }
        }


        public Maybe<ScenarioModule> Load()
        {
            return _psm.Load(ScenarioRunner.fetch) != null ? moduleRef : Maybe<ScenarioModule>.None;
        }


        public ConfigNode GetData()
        {
            return _psm.GetData();
        }


        public override bool Equals(object obj)
        {
            var other = obj as ProtoScenarioModule;
            if (other == null) return false;

            return _psm.moduleName == other.moduleName && _psm.moduleRef == other.moduleRef;
        }


        public override int GetHashCode()
        {
            return _psm.GetHashCode();
        }
    }
}
