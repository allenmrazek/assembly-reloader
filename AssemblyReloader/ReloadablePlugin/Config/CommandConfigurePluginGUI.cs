using AssemblyReloader.Gui;
using AssemblyReloader.ReloadablePlugin.Gui;
using AssemblyReloader.StrangeIoC.extensions.command.impl;
using AssemblyReloader.StrangeIoC.extensions.context.api;
using AssemblyReloader.StrangeIoC.extensions.injector;
using ReeperCommon.Logging;
using UnityEngine;

namespace AssemblyReloader.ReloadablePlugin.Config
{
    public class CommandConfigurePluginGUI : Command
    {
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

            //viewGo.gameObject.PrintComponents(Log);
            //gameObject.transform.parent.gameObject.PrintComponents(Log);
        }
    }
}
