using System;
using System.Linq;
using AssemblyReloader.Game;
using ReeperCommon.Containers;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.ReloadablePlugin
{
    public class ReloadablePlugin : IReloadablePlugin
    {
        private readonly IFile _reloadableFile;
        private readonly SignalReloadPlugin _reloadPluginSignal;
        private readonly SignalPluginWasLoaded _pluginWasLoadedSignal;
        private readonly SignalPluginWasUnloaded _pluginWasUnloadedSignal;

        private Maybe<ILoadedAssemblyHandle> _loaded = Maybe<ILoadedAssemblyHandle>.None;
  

        public ReloadablePlugin(
            IFile reloadableFile,
            SignalReloadPlugin reloadPluginSignal,
            SignalPluginWasLoaded pluginWasLoadedSignal,
            SignalPluginWasUnloaded pluginWasUnloadedSignal)
        {
            if (reloadableFile == null) throw new ArgumentNullException("reloadableFile");
            if (reloadPluginSignal == null) throw new ArgumentNullException("reloadPluginSignal");
            if (pluginWasLoadedSignal == null) throw new ArgumentNullException("pluginWasLoadedSignal");
            if (pluginWasUnloadedSignal == null) throw new ArgumentNullException("pluginWasUnloadedSignal");

            _reloadableFile = reloadableFile;
            _reloadPluginSignal = reloadPluginSignal;
            _pluginWasLoadedSignal = pluginWasLoadedSignal;
            _pluginWasUnloadedSignal = pluginWasUnloadedSignal;

            _pluginWasLoadedSignal.AddListener(OnPluginLoaded);
            _pluginWasUnloadedSignal.AddListener(OnPluginUnloaded);
        }


        ~ReloadablePlugin()
        {
            _pluginWasLoadedSignal.RemoveListener(OnPluginLoaded);
            _pluginWasUnloadedSignal.RemoveListener(OnPluginUnloaded);
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


        public void Reload()
        {

            _reloadPluginSignal.Dispatch(_loaded);

        }
    }
}