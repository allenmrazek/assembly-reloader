using System;
using System.Collections;
using System.Linq;
using AssemblyReloader.Game;
using ReeperAssemblyLibrary;
using ReeperCommon.Containers;
using ReeperCommon.Logging;
using strange.extensions.command.impl;
using UnityEngine;

namespace AssemblyReloader.ReloadablePlugin
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class CommandUnloadPreviousPlugin : Command
    {
        private readonly Maybe<ILoadedAssemblyHandle> _previousAssemblyHandle;
        private readonly IRoutineRunner _coroutineRunner;
        private readonly SignalUnloadPlugin _unloadPluginSignal;
        private readonly SignalPluginWasUnloaded _pluginWasUnloadedSignal;
        private readonly SignalErrorWhileUnloading _failSignal;
        private readonly ILog _log;

        public CommandUnloadPreviousPlugin(
            Maybe<ILoadedAssemblyHandle> previousAssemblyHandle,
            IRoutineRunner coroutineRunner,
            SignalUnloadPlugin unloadPluginSignal,
            SignalPluginWasUnloaded pluginWasUnloadedSignal,
            SignalErrorWhileUnloading failSignal,
            ILog log)
        {
            if (coroutineRunner == null) throw new ArgumentNullException("coroutineRunner");
            if (unloadPluginSignal == null) throw new ArgumentNullException("unloadPluginSignal");
            if (pluginWasUnloadedSignal == null) throw new ArgumentNullException("pluginWasUnloadedSignal");
            if (failSignal == null) throw new ArgumentNullException("failSignal");
            if (log == null) throw new ArgumentNullException("log");

            _previousAssemblyHandle = previousAssemblyHandle;
            _coroutineRunner = coroutineRunner;
            _unloadPluginSignal = unloadPluginSignal;
            _pluginWasUnloadedSignal = pluginWasUnloadedSignal;
            _failSignal = failSignal;
            _log = log;
        }


        public override void Execute()
        {
            if (!_previousAssemblyHandle.Any())
            {
                _log.Debug("No previous assembly handle");
                return;
            }

            Retain();
            _coroutineRunner.StartCoroutine(DoExecute());
        }


        private IEnumerator DoExecute()
        {
            _log.Verbose("Unloading previous plugin handle");

            try
            {
                _unloadPluginSignal.Dispatch(_previousAssemblyHandle.Single());
            }
            catch (Exception e)
            {
                _log.Error("Exception while unloading previous handle! " + e);
                _failSignal.Dispatch("An exception occurred while unloading previous handle. See log");
                Fail();
                yield break;
            }

            // wait for OnDestroys to run (they're scheduled for end of frame)
            yield return new WaitForEndOfFrame();


            try
            {
                _pluginWasUnloadedSignal.Dispatch();
                Release();
            }
            catch (Exception e)
            {
                _log.Error("Exception while dispatching plugin unloaded signal! " + e);
                _failSignal.Dispatch("An exception occurred while dispatching plugin unloaded signal. See log");
                Fail();
            }
        }
    }
}
