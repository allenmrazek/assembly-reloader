using System;
using System.Linq;
using AssemblyReloader.Game;
using ReeperAssemblyLibrary;
using ReeperCommon.Containers;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.ReloadablePlugin
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class ReloadablePlugin : IReloadablePlugin
    {
        private readonly IFile _reloadableFile;
        private readonly SignalReloadPlugin _reloadPluginSignal;
        private readonly SignalReinstallPlugin _reinstallPluginSignal;
        private readonly SignalGameDatabaseReloadTriggered _assemblyLoaderWipedSignal;
        private readonly SignalPluginWasLoaded _pluginWasLoadedSignal;
        private readonly SignalPluginWasUnloaded _pluginWasUnloadedSignal;

        private Maybe<ILoadedAssemblyHandle> _loaded = Maybe<ILoadedAssemblyHandle>.None;
  

        public ReloadablePlugin(
            IFile reloadableFile,
            SignalReloadPlugin reloadPluginSignal,
            SignalReinstallPlugin reinstallPluginSignal,
            SignalGameDatabaseReloadTriggered assemblyLoaderWipedSignal,
            SignalPluginWasLoaded pluginWasLoadedSignal,
            SignalPluginWasUnloaded pluginWasUnloadedSignal)
        {
            if (reloadableFile == null) throw new ArgumentNullException("reloadableFile");
            if (reloadPluginSignal == null) throw new ArgumentNullException("reloadPluginSignal");
            if (reinstallPluginSignal == null) throw new ArgumentNullException("reinstallPluginSignal");
            if (assemblyLoaderWipedSignal == null) throw new ArgumentNullException("assemblyLoaderWipedSignal");
            if (pluginWasLoadedSignal == null) throw new ArgumentNullException("pluginWasLoadedSignal");
            if (pluginWasUnloadedSignal == null) throw new ArgumentNullException("pluginWasUnloadedSignal");

            _reloadableFile = reloadableFile;
            _reloadPluginSignal = reloadPluginSignal;
            _reinstallPluginSignal = reinstallPluginSignal;
            _assemblyLoaderWipedSignal = assemblyLoaderWipedSignal;
            _pluginWasLoadedSignal = pluginWasLoadedSignal;
            _pluginWasUnloadedSignal = pluginWasUnloadedSignal;

            _pluginWasLoadedSignal.AddListener(OnPluginLoaded);
            _pluginWasUnloadedSignal.AddListener(OnPluginUnloaded);
            _assemblyLoaderWipedSignal.AddListener(OnReloadablePluginHandleWipedByReload);
        }


        ~ReloadablePlugin()
        {
            _pluginWasLoadedSignal.RemoveListener(OnPluginLoaded);
            _pluginWasUnloadedSignal.RemoveListener(OnPluginUnloaded);
            _assemblyLoaderWipedSignal.RemoveListener(OnReloadablePluginHandleWipedByReload);
        }


        public string Name
        {
            get { return _reloadableFile.Name; }
        }


        public IFile Location
        {
            get { return _reloadableFile; }
        }


        private void OnPluginLoaded(ILoadedAssemblyHandle handle)
        {
            if (handle == null) throw new ArgumentNullException("handle");
            if (_loaded.Any())
                throw new Exception("Received new handle but previous assembly was not unloaded");

            _loaded = Maybe<ILoadedAssemblyHandle>.With(handle);
        }


        private void OnPluginUnloaded()
        {
            if (!_loaded.Any())
                throw new Exception("Received OnPluginUnloaded but no previous version was loaded");

            _loaded = Maybe<ILoadedAssemblyHandle>.None;
        }


        private void OnReloadablePluginHandleWipedByReload()
        {
            _reinstallPluginSignal.Dispatch(_loaded);
        }


        public void Reload()
        {
            _reloadPluginSignal.Dispatch(_loaded);
        }
    }
}