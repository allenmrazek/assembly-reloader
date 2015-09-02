using AssemblyReloader.Gui;
using AssemblyReloader.ReloadablePlugin.Gui;
using ReeperCommon.Logging;
using strange.extensions.command.impl;
using strange.extensions.context.api;
using strange.extensions.injector;
using UnityEngine;

namespace AssemblyReloader.ReloadablePlugin.Config
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class CommandConfigurePluginGui : Command
    {
// ReSharper disable once InconsistentNaming
        [Inject(ContextKeys.CONTEXT_VIEW)] public GameObject gameObject { get; set; }
        [Inject] public ILog Log { get; set; }
        [Inject] public IPluginInfo Plugin { get; set; }

        public override void Execute()
        {
            Log.Debug("Configuring GUI for " + Plugin.Name);

            var viewGo = new GameObject("PluginOptionsView." + Plugin.Name);
            viewGo.transform.parent = gameObject.transform;

            Object.DontDestroyOnLoad(viewGo);

            viewGo.AddComponent<PluginConfigurationView>();
        }
    }
}
