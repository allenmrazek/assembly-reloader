using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssemblyReloader.Annotations;
using AssemblyReloader.Destruction;
using AssemblyReloader.Providers.Game;
using ReeperCommon.Logging;
using ReeperCommon.Logging.Implementations;

namespace AssemblyReloader.Loaders
{
    public class ScenarioModuleUnloader : IScenarioModuleUnloader
    {
        private readonly IProtoScenarioModuleProvider _psmProvider;
        private readonly IUnityObjectDestroyer _objectDestroyer;
        private readonly bool _reuseConfigNode;
        private readonly ILog _log;

        public ScenarioModuleUnloader(
            [NotNull] IProtoScenarioModuleProvider psmProvider, 
            [NotNull] IUnityObjectDestroyer objectDestroyer,
            bool reuseConfigNode,
            [NotNull] ILog log)
        {
            if (psmProvider == null) throw new ArgumentNullException("psmProvider");
            if (objectDestroyer == null) throw new ArgumentNullException("objectDestroyer");
            if (log == null) throw new ArgumentNullException("log");

            _psmProvider = psmProvider;
            _objectDestroyer = objectDestroyer;
            _reuseConfigNode = reuseConfigNode;
            _log = log;
        }


        public void Unload([NotNull] Type type)
        {
            if (type == null) throw new ArgumentNullException("type");

            var psms = _psmProvider.Get(type).ToArray();

            foreach (var psm in psms)
                UninstallScenarioModule(type, psm);
        }


        private void UninstallScenarioModule(Type type, [NotNull] ProtoScenarioModule psm)
        {
            if (psm == null) throw new ArgumentNullException("psm");

            if (psm.moduleRef == null)
            {
                _log.Warning("Psm.ModuleRef is null for " + psm.moduleName);
                return; //?
            }


            var sm = ScenarioRunner.fetch.GetComponent(type) as ScenarioModule;
            if (sm == null)
            {
                _log.Error("Failed to locate " + psm.moduleName + " ScenarioModule on ScenarioRunner object");
                return;
            }

            // take a snapshot of the current state of the ScenarioModule so we can reuse it to load
            // the next version
            if (_reuseConfigNode)
            {
                try
                {
                    var snapshot = new ConfigNode("SCENARIO");
                    sm.Save(snapshot);

                    if (!HighLogic.CurrentGame.RemoveProtoScenarioModule(type))
                        throw new Exception("Failed to remove proto scenario module of " + type.FullName);

                    HighLogic.CurrentGame.scenarios.Add(new ProtoScenarioModule(snapshot));
                    ScenarioRunner.SetProtoModules(HighLogic.CurrentGame.scenarios);
                }
                catch (Exception)
                {
                    _log.Warning("Failed to save snapshot of " + type.FullName +
                                 "; default ConfigNode will be used for next instance");
                }
            }

            _log.Normal("Destroying ScenarioModule " + type.FullName + " (called " + psm.moduleName + ")");

            _objectDestroyer.Destroy(sm);
            psm.moduleRef = null;
        }
    }
}
