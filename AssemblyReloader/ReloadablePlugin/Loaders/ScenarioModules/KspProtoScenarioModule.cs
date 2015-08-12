extern alias KSP;
using System;
using System.Linq;
using ReeperCommon.Containers;

namespace AssemblyReloader.ReloadablePlugin.Loaders.ScenarioModules
{
    public class KspProtoScenarioModule : IProtoScenarioModule
    {
        private readonly KSP::ProtoScenarioModule _psm;


        public KspProtoScenarioModule(
            KSP::ProtoScenarioModule psm)
        {
            if (psm == null) throw new ArgumentNullException("psm");

            _psm = psm;
        }


        public Maybe<KSP::ScenarioModule> moduleRef
        {
            get { return _psm.moduleRef != null ? Maybe<KSP::ScenarioModule>.With(_psm.moduleRef) : Maybe<KSP::ScenarioModule>.None; }
            set { _psm.moduleRef = value.SingleOrDefault(); }
        }


        public string moduleName
        {
            get { return _psm.moduleName; }
        }


        public KSP::GameScenes[] TargetScenes
        {
            get { return _psm.targetScenes.ToArray(); }
            set { _psm.SetTargetScenes(value); }
        }


        public Maybe<KSP::ScenarioModule> Load()
        {
            return _psm.Load(KSP::ScenarioRunner.fetch) != null ? moduleRef : Maybe<KSP::ScenarioModule>.None;
        }


        public KSP::ConfigNode GetData()
        {
            return _psm.GetData();
        }


        public override bool Equals(object obj)
        {
            var other = obj as KSP::ProtoScenarioModule;
            if (other == null) return false;

            return _psm.moduleName == other.moduleName && _psm.moduleRef == other.moduleRef;
        }


        public override int GetHashCode()
        {
            return _psm.GetHashCode();
        }
    }
}
