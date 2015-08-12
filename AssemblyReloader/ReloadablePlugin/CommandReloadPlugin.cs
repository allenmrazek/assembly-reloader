extern alias KSP;
using System;
using System.Collections;
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
using UnityEngine;
using HighLogic = KSP::HighLogic;
using PopupDialog = KSP::PopupDialog;

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

        [Inject]
        public IRoutineRunner CoroutineRunner { get; set; }

        [Inject]
        public ILog Log { get; set; }

        [Inject]
        public IPluginInfo Plugin { get; set; }




        [Inject]
        public SignalPluginWasLoaded PluginWasLoadedSignal { get; set; }

        [Inject]
        public SignalPluginWasUnloaded PluginWasUnloadedSignal { get; set; }

        [Inject]
        public SignalUnloadPlugin PluginUnloadSignal { get; set; }


        public override void Execute()
        {
            Retain();
            CoroutineRunner.StartCoroutine(DoExecute());
        }


        private IEnumerator DoExecute()
        {
            Log.Debug("Reloading {0}", Plugin.Name);

            var definition = DefinitionReader.Read();

            if (!definition.Any())
            {
                Log.Error("Failed to read {0} definition", Plugin.Location.Url);
                Fail();
                Release();
                yield break;
            }


            // definition loaded successfully, time to uninstall previous version
            if (LoadedHandle.Any())
            {
                Log.Verbose("Unloading previous handle of {0}", Plugin.Location.Url);
                PluginUnloadSignal.Dispatch(LoadedHandle.Single());

                // wait for OnDestroys to run
                yield return new WaitForEndOfFrame();

                GameAssemblyLoader.RemoveFromLoadedAssemblies(LoadedHandle.Single());
                PluginWasUnloadedSignal.Dispatch();
            }

            var loadedAssembly = DefinitionLoader.Get(definition.Single());

            if (!loadedAssembly.Any())
            {
                Log.Error("Failed to load assembly definition of {0} into memory", Plugin.Location.Url);
                Fail();
                Release();
                yield break;;
            }

            var handle = GameAssemblyLoader.AddToLoadedAssemblies(loadedAssembly.Single(), Plugin.Location);

            if (!handle.Any())
            {
                Log.Error("Failed to create loaded assembly handle of {0}", Plugin.Location.Url);
                Fail();
                Release();
                yield break;
            }

            // todo: the context exception handler won't catch exceptions in here, so this command needs
            // to be shredded into manageable pieces that each handle exceptions appropriately like below
            // so things don't silently fail if configuration is wrong (due to unexpected exception or
            // programmer fail)
            try
            {
                PluginWasLoadedSignal.Dispatch(handle.Single());
            }
            catch (Exception e)
            {
                Debug.LogError("AssemblyReloader: Encountered an uncaught exception while reloading plugin: " + e);

                PopupDialog.SpawnPopupDialog("AssemblyReloader unhandled exception",
                    "AssemblyReloader encountered an exception with the following message: " + e.Message + "\n\nAssemblyReloader has been disabled.", "Okay",
                    true, HighLogic.Skin);
            }
        }
    }
}
