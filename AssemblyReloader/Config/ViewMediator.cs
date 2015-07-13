using System;
using System.Collections.Generic;
using AssemblyReloader.ReloadablePlugin;
using AssemblyReloader.StrangeIoC.extensions.injector;
using AssemblyReloader.StrangeIoC.extensions.mediation.impl;

namespace AssemblyReloader.Config
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class ViewMediator : Mediator
    {
        [Inject] public MainView MainView { get; set; }
        [Inject] public IEnumerable<IReloadablePlugin> Plugins { get; set; }



        public override void OnRegister()
        {
            base.OnRegister();

            

            //MainView.Mediator = SettingsView.Mediator = this;
        }


        public override void OnRemove()
        {
            base.OnRemove();

        }


        public void Reload(IPluginInfo plugin)
        {
            if (plugin == null) throw new ArgumentNullException("plugin");

            throw new NotImplementedException();
        }


        public void TogglePluginOptions(IPluginInfo plugin)
        {
            if (plugin == null) throw new ArgumentNullException("plugin");

            throw new NotImplementedException();
        }


        public void ToggleOptions()
        {
            //SettingsView.Visible = !SettingsView.Visible;
        }


        public void HideMainWindow()
        {
            //MainView.Visible = SettingsView.Visible = false; // hide all windows
        }


        public void SaveConfiguration()
        {
            //_mainConfigurationSaver.Save();
        }
    }
}
