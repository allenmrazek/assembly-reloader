using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssemblyReloader.Config.Keys;
using AssemblyReloader.Game;
using AssemblyReloader.Gui;
using AssemblyReloader.ReloadablePlugin.Weaving;
using AssemblyReloader.StrangeIoC.extensions.command.impl;
using AssemblyReloader.StrangeIoC.extensions.injector;
using ReeperCommon.Containers;
using ReeperCommon.Logging;

namespace AssemblyReloader.ReloadablePlugin
{
    public class CommandReloadPlugin : Command
    {
        [Inject]
        public Maybe<ILoadedAssemblyHandle> LoadedHandle { get; set; }

        [Inject]
        public IAssemblyDefinitionReader DefinitionReader { get; set; }

        [Inject]
        public IAssemblyDefinitionLoader DefinitionLoader { get; set; }

        [Inject]
        public IGameAssemblyLoader GameAssemblyLoader { get; set; }

        [Inject(LogKeys.PluginContext)]
        public ILog Log { get; set; }

        [Inject]
        public IPluginInfo Plugin { get; set; }


        [Inject]
        public SignalInstallPluginTypes InstallPluginTypesSignal { get; set; }

        [Inject]
        public SignalUninstallPluginTypes UninstallPluginTypesSignal { get; set; }

        [Inject]
        public SignalPluginWasLoaded PluginWasLoadedSignal { get; set; }

        [Inject]
        public SignalPluginWasUnloaded PluginWasUnloadedSignal { get; set; }


        public override void Execute()
        {
            Log.Debug("Reloading {0}", Plugin.Name);

            var definition = DefinitionReader.Read();

            if (!definition.Any())
            {
                Log.Error("Failed to read {0} definition", Plugin.Location.Url);
                Fail();
                return;
            }


            // definition loaded successfully, time to uninstall previous version
            if (LoadedHandle.Any())
            {
                Log.Verbose("Unloading previous handle of {0}", Plugin.Location.Url);
                UninstallPluginTypesSignal.Dispatch(LoadedHandle.Single());
                GameAssemblyLoader.RemoveFromLoadedAssemblies(LoadedHandle.Single());
                PluginWasUnloadedSignal.Dispatch();
            }

            var loadedAssembly = DefinitionLoader.Get(definition.Single());

            if (!loadedAssembly.Any())
            {
                Log.Error("Failed to load assembly definition of {0} into memory", Plugin.Location.Url);
                Fail();
                return;
            }

            var handle = GameAssemblyLoader.AddToLoadedAssemblies(loadedAssembly.Single(), Plugin.Location);

            if (!handle.Any())
            {
                Log.Error("Failed to create loaded assembly handle of {0}", Plugin.Location.Url);
                Fail();
                return;
            }

            InstallPluginTypesSignal.Dispatch(handle.Single());
            PluginWasLoadedSignal.Dispatch(handle.Single());
        }


   
    }
}
