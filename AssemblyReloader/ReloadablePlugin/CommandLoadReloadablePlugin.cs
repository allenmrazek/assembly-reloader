extern alias Cecil96;
using System;
using System.Linq;
using System.Reflection;
using AssemblyReloader.Game;
using AssemblyReloader.Gui;
using ReeperAssemblyLibrary;
using ReeperCommon.Containers;
using ReeperCommon.Logging;
using strange.extensions.command.impl;

namespace AssemblyReloader.ReloadablePlugin
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class CommandLoadReloadablePlugin : Command
    {
        private readonly ReeperAssembly _reeperAssembly;
        private readonly IReeperAssemblyLoader _loader;
        private readonly SignalPluginCannotBeLoaded _failSignal;
        private readonly SignalPluginWasLoaded _pluginLoadedSignal;
        private readonly ILog _log;

        public CommandLoadReloadablePlugin(
            ReeperAssembly reeperAssembly,
            IReeperAssemblyLoader loader,
            SignalPluginCannotBeLoaded failSignal,
            SignalPluginWasLoaded pluginLoadedSignal,
            ILog log)
        {
            if (reeperAssembly == null) throw new ArgumentNullException("reeperAssembly");
            if (loader == null) throw new ArgumentNullException("loader");
            if (failSignal == null) throw new ArgumentNullException("failSignal");
            if (pluginLoadedSignal == null) throw new ArgumentNullException("pluginLoadedSignal");
            if (log == null) throw new ArgumentNullException("log");

            _reeperAssembly = reeperAssembly;
            _loader = loader;
            _failSignal = failSignal;
            _pluginLoadedSignal = pluginLoadedSignal;
            _log = log;
        }

        public override void Execute()
        {
            _log.Verbose("Loading plugin " + _reeperAssembly.File.name);

            try
            {
                var handle = _loader.Load(_reeperAssembly);
                _pluginLoadedSignal.Dispatch(handle);
            }
            catch (Exception e)
            {
                _log.Error("Exception while loading plugin: " + _reeperAssembly.File.name);
                _log.Error("Exception: " + e);

                _failSignal.Dispatch("Exception while loading plugin: " + e);
                Fail();
            }
            //throw new NotImplementedException();

            //var loadedAssembly = _definitionLoader.Get(_assemblyDefinition);

            //if (!loadedAssembly.Any() || loadedAssembly.Single().IsNull())
            //{
            //    _log.Error("Failed to load assembly definition!");
            //    _failSignal.Dispatch("Failed to load " + _assemblyDefinition.Name + " definition");
            //    Fail();
            //    return;
            //}

            //if (_loadedHandle.Any())
            //    if (!RemovePreviousVersionFromAssemblyLoader())
            //        return;

            //if (!InsertIntoAssemblyLoader(loadedAssembly.Single()))
            //    return;


            _log.Verbose("Plugin loaded successfully!");
        }


        //private bool RemovePreviousVersionFromAssemblyLoader()
        //{
        //    try
        //    {
        //        _gameAssemblyLoader.RemoveFromLoadedAssemblies(_loadedHandle.Single());
        //        return true;
        //    }
        //    catch (Exception e)
        //    {
        //        _log.Error("Exception: " + e);
        //        _failSignal.Dispatch("Exception occurred while removing plugin from AssemblyLoader: " + e);
        //        Fail();
        //    }

        //    return false;
        //}


        //private bool InsertIntoAssemblyLoader(Assembly assembly)
        //{
        //    try
        //    {
        //        var handle = _gameAssemblyLoader.AddToLoadedAssemblies(assembly,
        //            _pluginInfo.Location);

        //        _pluginLoadedSignal.Dispatch(handle);

        //        return true;
        //    }
        //    catch (DuplicateLoadedAssemblyException e)
        //    {
        //        _log.Error("Exception while inserting " + _pluginInfo.Name + " into AssemblyLoader: " + e);
        //        _failSignal.Dispatch("Duplicate loaded assembly already exists in AssemblyLoader!");
        //        Fail();
        //    }
        //    catch (Exception e)
        //    {
        //        _log.Error("Exception: " + e);
        //        _failSignal.Dispatch("Exception occurred while loading plugin: " + e);
        //        Fail();
        //    }

        //    return false;
        //}
    }
}
