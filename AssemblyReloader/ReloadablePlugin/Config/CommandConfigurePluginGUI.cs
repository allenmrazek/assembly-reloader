using System;
using System.Collections;
using AssemblyReloader.Gui;
using AssemblyReloader.ReloadablePlugin.Gui;
using ReeperCommon.Logging;
using strange.extensions.command.impl;
using strange.extensions.context.api;
using strange.extensions.injector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AssemblyReloader.ReloadablePlugin.Config
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class CommandConfigurePluginGui : Command
    {
        private readonly GameObject _contextView;
        private readonly IPluginInfo _plugin;
        private readonly IRoutineRunner _routineRunner;
        private readonly ILog _log;

        public CommandConfigurePluginGui(
            [Name(ContextKeys.CONTEXT_VIEW)] GameObject contextView,
            IPluginInfo plugin,
            IRoutineRunner routineRunner,
            ILog log)
        {
            if (contextView == null) throw new ArgumentNullException("contextView");
            if (plugin == null) throw new ArgumentNullException("plugin");
            if (routineRunner == null) throw new ArgumentNullException("routineRunner");
            if (log == null) throw new ArgumentNullException("log");
            _contextView = contextView;
            _plugin = plugin;
            _routineRunner = routineRunner;
            _log = log;
        }

        public override void Execute()
        {
            _log.Debug("Configuring GUI for " + _plugin.Name);

            Retain();
            _routineRunner.StartCoroutine(CreateView());
        }

        private IEnumerator CreateView()
        {
            var viewGo = new GameObject("PluginOptionsView." + _plugin.Name);
            viewGo.transform.parent = _contextView.transform;

            Object.DontDestroyOnLoad(viewGo);

            viewGo.AddComponent<PluginConfigurationView>();

            yield return 0; // wait for view to initialize
            Release();
        }
    }
}
