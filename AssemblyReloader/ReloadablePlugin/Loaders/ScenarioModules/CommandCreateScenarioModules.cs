using System;
using System.Linq;
using AssemblyReloader.DataObjects;
using AssemblyReloader.Game;
using AssemblyReloader.Game.Providers;
using AssemblyReloader.StrangeIoC.extensions.command.impl;
using AssemblyReloader.Unsorted;
using ReeperCommon.Logging;

namespace AssemblyReloader.ReloadablePlugin.Loaders.ScenarioModules
{

// ReSharper disable once ClassNeverInstantiated.Global
    public class CommandCreateScenarioModules : Command
    {
        private readonly ILoadedAssemblyHandle _loadedHandle;
        private readonly IGetTypeIdentifier _typeIdentifierQuery;
        private readonly IGetTypesDerivedFrom<ScenarioModule> _scenarioModuleTypeQuery;
        private readonly IGetProtoScenarioModules _protoScenarioModuleProvider;
        private readonly IScenarioModuleSettings _scenarioModuleSettings;
        private readonly ILog _log;


        public CommandCreateScenarioModules(
            ILoadedAssemblyHandle loadedHandle,
            IGetTypeIdentifier typeIdentifierQuery,
            IGetTypesDerivedFrom<ScenarioModule> scenarioModuleTypeQuery,
            IGetProtoScenarioModules protoScenarioModuleProvider,
            IScenarioModuleSettings scenarioModuleSettings,
            ILog log)
        {
            if (loadedHandle == null) throw new ArgumentNullException("loadedHandle");
            if (typeIdentifierQuery == null) throw new ArgumentNullException("typeIdentifierQuery");
            if (scenarioModuleTypeQuery == null) throw new ArgumentNullException("scenarioModuleTypeQuery");
            if (protoScenarioModuleProvider == null) throw new ArgumentNullException("protoScenarioModuleProvider");
            if (scenarioModuleSettings == null) throw new ArgumentNullException("scenarioModuleSettings");
            if (log == null) throw new ArgumentNullException("log");

            _loadedHandle = loadedHandle;
            _typeIdentifierQuery = typeIdentifierQuery;
            _scenarioModuleTypeQuery = scenarioModuleTypeQuery;
            _protoScenarioModuleProvider = protoScenarioModuleProvider;
            _scenarioModuleSettings = scenarioModuleSettings;
            _log = log;
        }


        public override void Execute()
        {
            if (!_scenarioModuleSettings.ReloadScenarioModulesImmediately)
                return;

            return;

            _log.Debug("Creating ScenarioModules");

            foreach (var smType in _scenarioModuleTypeQuery.Get(_loadedHandle.LoadedAssembly.assembly))
                LoadScenarioModule(smType);

            _log.Debug("Finished creating ScenarioModules");
        }


        private void LoadScenarioModule(Type scenarioType)
        {
            var identifier = _typeIdentifierQuery.Get(scenarioType);

            // there should be only a single instance; this done in case the target plugin has managed to do
            // something very hacky
            foreach (var psm in _protoScenarioModuleProvider.Get(scenarioType))
            {
                if (psm.moduleRef.Any())
                {
                    _log.Error("A ScenarioModule instance of type " + identifier +
                               " already referenced by ScenarioRunner");
                    continue;
                }

                // todo: update target scenes

                try
                {
                    //if (psm.Load().Any())
                    //    _log.Verbose("Successfully created ScenarioModule " + identifier);
                    /*else*/ _log.Warning("Failed to create ScenarioModule " + identifier);
                }
                catch (Exception e) // large scope intended
                {
                    _log.Error("Failed to load " + identifier + ": " + e);
                }
            }
        }
    }
}
