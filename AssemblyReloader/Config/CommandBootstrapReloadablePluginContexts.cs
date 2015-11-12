using System;
using System.Collections.Generic;
using System.Linq;
using AssemblyReloader.Gui;
using AssemblyReloader.ReloadablePlugin;
using AssemblyReloader.ReloadablePlugin.Config;
using ReeperCommon.Logging;
using strange.extensions.command.impl;
using strange.extensions.context.api;
using strange.extensions.injector;
using UnityEngine;

namespace AssemblyReloader.Config
{
    public class CommandBootstrapReloadablePluginContexts : Command
    {
        private readonly GameObject _contextView;

        public CommandBootstrapReloadablePluginContexts([Name(ContextKeys.CONTEXT_VIEW)] GameObject contextView)
        {
            if (contextView == null) throw new ArgumentNullException("contextView");
            _contextView = contextView;
        }


        public override void Execute()
        {
            var pluginContexts =
                injectionBinder.GetInstance<GetReloadableAssemblyFilesFromDirectoryRecursive>().Get()
                    .Select(reloadableFile =>
                    {
                        injectionBinder.GetInstance<ILog>().Normal("Bootstrapping context for " + reloadableFile.Url);

                        var reloadablePluginBootstrap = new GameObject("ContextView." + reloadableFile.Name);
                        reloadablePluginBootstrap.transform.parent = _contextView.transform;
                        UnityEngine.Object.DontDestroyOnLoad(reloadablePluginBootstrap);

                        var pluginContextView = reloadablePluginBootstrap.AddComponent<BootstrapReloadablePlugin>();

                        pluginContextView.Bootstrap(reloadableFile);

                        return pluginContextView.context as ReloadablePluginContext;
                    })
                    .ToList();

            var pluginInfoMapping = pluginContexts.ToDictionary(context => context.Info, context => context.Plugin);


            // core context bindings
            injectionBinder.Bind<IEnumerable<ReloadablePluginContext>>().To(pluginContexts);
            injectionBinder.Bind<IEnumerable<IPluginInfo>>().To(pluginInfoMapping.Keys);
            injectionBinder.Bind<IEnumerable<IReloadablePlugin>>().To(pluginInfoMapping.Values);
            injectionBinder.Bind<IDictionary<IPluginInfo, IReloadablePlugin>>().To(pluginInfoMapping);
        }
    }
}
