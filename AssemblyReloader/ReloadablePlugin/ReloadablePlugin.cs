using System;
using System.Linq;
using AssemblyReloader.Game;
using AssemblyReloader.Gui;
using AssemblyReloader.Properties;
using AssemblyReloader.ReloadablePlugin.Definition;
using ReeperCommon.Containers;
using ReeperCommon.Extensions;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.ReloadablePlugin
{
    public class ReloadablePlugin : IPluginInfo, IReloadablePlugin
    {
        private readonly IFile _reloadableFile;
        private readonly IGameAssemblyLoader _assemblyLoader;
        private readonly IAssemblyProvider _assemblyProvider;
        private readonly IReloadableTypeSystem _reloadableTypeSystem;

        private Maybe<ILoadedAssemblyHandle> _loaded;


        public ReloadablePlugin(
            [NotNull] IFile reloadableFile,
            [NotNull] IGameAssemblyLoader assemblyLoader,
            [NotNull] IAssemblyProvider assemblyProvider,
            [NotNull] IReloadableTypeSystem reloadableTypeSystem)
        {
            if (reloadableFile == null) throw new ArgumentNullException("reloadableFile");
            if (assemblyLoader == null) throw new ArgumentNullException("assemblyLoader");
            if (assemblyProvider == null) throw new ArgumentNullException("assemblyProvider");
            if (reloadableTypeSystem == null) throw new ArgumentNullException("reloadableTypeSystem");

            _reloadableFile = reloadableFile;
            _assemblyLoader = assemblyLoader;
            _assemblyProvider = assemblyProvider;
            _reloadableTypeSystem = reloadableTypeSystem;
        }


        public string Name
        {
            get { return _reloadableFile.Name; }
        }


        public IFile Location
        {
            get { return _reloadableFile; }
        }


        private bool Load()
        {
            if (_loaded.Any())
                throw new InvalidOperationException("Previous instance was not unloaded");

            var assembly = _assemblyProvider.Get(Location);
            if (!assembly.Any())
                throw new Exception("Failed to read assembly at " + Location.FullPath);

            _loaded = _assemblyLoader.AddToLoadedAssemblies(assembly.Single(), Location);

            if (_loaded.Any())
                _reloadableTypeSystem.CreateReloadableTypesFrom(_loaded.Single());

            return _loaded.Any();
        }


        private void Unload()
        {
            if (!_loaded.Any())
                throw new InvalidOperationException("No assembly loaded");

            _reloadableTypeSystem.DestroyReloadableTypesFrom(_loaded.Single());

            _assemblyLoader.RemoveFromLoadedAssemblies(_loaded.Single());
            _loaded = Maybe<ILoadedAssemblyHandle>.None;
        }


        public bool Reload()
        {
            if (_loaded.Any())
                Unload();

            return Load();
        }
    }
}