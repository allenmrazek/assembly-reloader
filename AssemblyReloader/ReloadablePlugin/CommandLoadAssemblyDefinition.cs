extern alias Cecil96;
using System;
using System.Linq;
using System.Reflection;
using AssemblyReloader.Game;
using AssemblyReloader.Gui;
using AssemblyReloader.ReloadablePlugin.Weaving;
using ReeperCommon.Containers;
using ReeperCommon.Extensions;
using ReeperCommon.Logging;
using strange.extensions.command.impl;

namespace AssemblyReloader.ReloadablePlugin
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class CommandLoadAssemblyDefinition : Command
    {
        private readonly IPluginInfo _pluginInfo;
        private readonly Maybe<ILoadedAssemblyHandle> _loadedHandle;
        private readonly Cecil96::Mono.Cecil.AssemblyDefinition _assemblyDefinition;
        private readonly IAssemblyDefinitionLoader _definitionLoader;
        private readonly IGameAssemblyLoader _gameAssemblyLoader;
        private readonly SignalPluginCannotBeLoaded _failSignal;
        private readonly SignalPluginWasLoaded _pluginLoadedSignal;
        private readonly ILog _log;

        public CommandLoadAssemblyDefinition(
            IPluginInfo pluginInfo,
            Maybe<ILoadedAssemblyHandle> loadedHandle,
            Cecil96::Mono.Cecil.AssemblyDefinition assemblyDefinition,
            IAssemblyDefinitionLoader definitionLoader,
            IGameAssemblyLoader gameAssemblyLoader,
            SignalPluginCannotBeLoaded failSignal,
            SignalPluginWasLoaded pluginLoadedSignal,
            ILog log)
        {
            if (pluginInfo == null) throw new ArgumentNullException("pluginInfo");
            if (assemblyDefinition == null) throw new ArgumentNullException("assemblyDefinition");
            if (definitionLoader == null) throw new ArgumentNullException("definitionLoader");
            if (gameAssemblyLoader == null) throw new ArgumentNullException("gameAssemblyLoader");
            if (failSignal == null) throw new ArgumentNullException("failSignal");
            if (pluginLoadedSignal == null) throw new ArgumentNullException("pluginLoadedSignal");
            if (log == null) throw new ArgumentNullException("log");

            _pluginInfo = pluginInfo;
            _loadedHandle = loadedHandle;
            _assemblyDefinition = assemblyDefinition;
            _definitionLoader = definitionLoader;
            _gameAssemblyLoader = gameAssemblyLoader;
            _failSignal = failSignal;
            _pluginLoadedSignal = pluginLoadedSignal;
            _log = log;
        }

        public override void Execute()
        {
            _log.Verbose("Loading assembly definition");

            var loadedAssembly = _definitionLoader.Get(_assemblyDefinition);

            if (!loadedAssembly.Any() || loadedAssembly.Single().IsNull())
            {
                _log.Error("Failed to load assembly definition!");
                _failSignal.Dispatch("Failed to load " + _assemblyDefinition.Name + " definition");
                Fail();
                return;
            }

            if (_loadedHandle.Any())
                if (!RemovePreviousVersionFromAssemblyLoader())
                    return;

            if (!InsertIntoAssemblyLoader(loadedAssembly.Single()))
                return;


            _log.Verbose("Definition loaded successfully!");
        }


        private bool RemovePreviousVersionFromAssemblyLoader()
        {
            try
            {
                _gameAssemblyLoader.RemoveFromLoadedAssemblies(_loadedHandle.Single());
                return true;
            }
            catch (Exception e)
            {
                _log.Error("Exception: " + e);
                _failSignal.Dispatch("Exception occurred while removing plugin from AssemblyLoader: " + e);
                Fail();
            }

            return false;
        }


        private bool InsertIntoAssemblyLoader(Assembly assembly)
        {
            try
            {
                var handle = _gameAssemblyLoader.AddToLoadedAssemblies(assembly,
                    _pluginInfo.Location);

                _pluginLoadedSignal.Dispatch(handle);

                return true;
            }
            catch (DuplicateLoadedAssemblyException e)
            {
                _log.Error("Exception while inserting " + _pluginInfo.Name + " into AssemblyLoader: " + e);
                _failSignal.Dispatch("Duplicate loaded assembly already exists in AssemblyLoader!");
                Fail();
            }
            catch (Exception e)
            {
                _log.Error("Exception: " + e);
                _failSignal.Dispatch("Exception occurred while loading plugin: " + e);
                Fail();
            }

            return false;
        }
    }
}
