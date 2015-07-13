using System;
using System.Linq;
using AssemblyReloader.Config;
using AssemblyReloader.Game;
using AssemblyReloader.Gui;
using AssemblyReloader.ReloadablePlugin.Config;
using ReeperCommon.Containers;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.ReloadablePlugin
{
    public class ReloadablePlugin : IPluginInfo, IReloadablePlugin
    {
        private readonly IFile _reloadableFile;
        private readonly SignalLoadAssembly _signalLoad;
        private readonly SignalUnloadAssembly _signalUnload;
        private readonly SignalAssemblyWasUnloaded _signalWasUnloaded;
        private readonly SignalAssemblyWasLoaded _signalWasLoaded;
        private Maybe<ILoadedAssemblyHandle> _loaded = Maybe<ILoadedAssemblyHandle>.None;
  
        public ReloadablePlugin(
            IFile reloadableFile, 
            SignalLoadAssembly signalLoad, 
            SignalAssemblyWasLoaded signalWasLoaded,
            SignalUnloadAssembly signalUnload, 
            SignalAssemblyWasUnloaded signalWasUnloaded)
        {
            if (reloadableFile == null) throw new ArgumentNullException("reloadableFile");
            if (signalLoad == null) throw new ArgumentNullException("signalLoad");
            if (signalUnload == null) throw new ArgumentNullException("signalUnload");
            if (signalWasUnloaded == null) throw new ArgumentNullException("signalWasUnloaded");
            if (signalWasLoaded == null) throw new ArgumentNullException("signalWasLoaded");

            _reloadableFile = reloadableFile;
            _signalLoad = signalLoad;
            _signalUnload = signalUnload;
            _signalWasUnloaded = signalWasUnloaded;
            _signalWasLoaded = signalWasLoaded;

            _signalWasLoaded.AddListener(AssemblyLoaded);
            _signalWasUnloaded.AddListener(AssemblyUnloaded);
        }


        ~ReloadablePlugin()
        {
            _signalWasLoaded.RemoveListener(AssemblyLoaded);
            _signalWasUnloaded.RemoveListener(AssemblyUnloaded);
        }


        public string Name
        {
            get { return _reloadableFile.Name; }
        }


        public IFile Location
        {
            get { return _reloadableFile; }
        }


        private void AssemblyLoaded(ILoadedAssemblyHandle handle)
        {
            if (handle == null) throw new ArgumentNullException("handle");
            if (_loaded.Any())
                throw new Exception("Received new handle but previous assembly was not unloaded");

            _loaded = Maybe<ILoadedAssemblyHandle>.With(handle);
        }


        private void AssemblyUnloaded()
        {
            _loaded = Maybe<ILoadedAssemblyHandle>.None;
        }


        private void Load()
        {
            _signalLoad.Dispatch(_reloadableFile);

            //if (_loaded.Any())
            //    throw new InvalidOperationException("Previous instance was not unloaded");

            //var assembly = _assemblyProvider.Get(Location);
            //if (!assembly.Any())
            //    throw new Exception("Failed to read assembly at " + Location.FullPath);

            //_loaded = _assemblyLoader.AddToLoadedAssemblies(assembly.Single(), Location);

            ////if (_loaded.Any())
            ////    _reloadableTypeSystem.CreateReloadableTypesFrom(_loaded.Single());

            //if (_loaded.Any())
            //    _signalLoaded.Dispatch(_loaded.Single());

            //return _loaded.Any();
        }


        private void Unload()
        {
            _signalUnload.Dispatch(_loaded.Single());

            //if (!_loaded.Any())
            //    throw new InvalidOperationException("No assembly loaded");

            ////_reloadableTypeSystem.DestroyReloadableTypesFrom(_loaded.Single());
            //_signalUnloading.Dispatch(_loaded.Single());

            //_assemblyLoader.RemoveFromLoadedAssemblies(_loaded.Single());
            //_loaded = Maybe<ILoadedAssemblyHandle>.None;
        }


        public void Reload()
        {
            if (_loaded.Any())
                Unload();

            Load();
        }
    }
}