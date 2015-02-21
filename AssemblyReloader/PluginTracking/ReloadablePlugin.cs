using System;
using System.Linq;
using System.Reflection;
using AssemblyReloader.CompositeRoot.Commands;
using AssemblyReloader.ILModifications;
using AssemblyReloader.Loaders.AddonLoader;
using AssemblyReloader.Loaders.PMLoader;
using AssemblyReloader.Providers.SceneProviders;
using AssemblyReloader.Queries;
using Mono.Cecil;
using ReeperCommon.Containers;
using ReeperCommon.Events;
using ReeperCommon.Extensions;
using ReeperCommon.FileSystem;
using ReeperCommon.Logging;

namespace AssemblyReloader.PluginTracking
{

    /// <summary>
    /// This object tracks a particular reloadable dll (based on its location). This is necessary
    /// because the state of a previous version of the assembly may affect how we load the next one;
    /// specifically, we may need to modify changes the previous version made such as removing PartModules
    /// from prefabs
    /// </summary>
    public class ReloadablePlugin : IReloadablePlugin
    {
        private Assembly _loaded;

        private readonly IFile _location;
        private readonly IAddonLoader _addonLoader;
        private readonly ICurrentStartupSceneProvider _currentSceneProvider;
        private readonly IModifiedAssemblyFactory _massemblyFactory;



        public ReloadablePlugin(
            IFile location,
            IAddonLoader addonLoader,
            ICurrentStartupSceneProvider currentSceneProvider,
            IModifiedAssemblyFactory massemblyFactory)
        {
            if (location == null) throw new ArgumentNullException("location");
            if (addonLoader == null) throw new ArgumentNullException("addonLoader");
            if (currentSceneProvider == null) throw new ArgumentNullException("currentSceneProvider");
            if (massemblyFactory == null) throw new ArgumentNullException("massemblyFactory");

            _location = location;
            _addonLoader = addonLoader;
            _currentSceneProvider = currentSceneProvider;
            _massemblyFactory = massemblyFactory;
        }


        public void Load()
        {
            using (var stream = new System.IO.MemoryStream())
            {
                var original = _massemblyFactory.Create(_location);

                original.Rename(Guid.NewGuid());

                original.Write(stream);

                var result = original.Load(stream);

                if (!result.Any())
                    return;
                

                _loaded = result.Single();
                _addonLoader.LoadAddonTypesFrom(_loaded);
                _addonLoader.CreateForScene(_currentSceneProvider.Get());
            }
        }


        public void Unload()
        {
            if (_loaded.IsNull()) return; // nothing to unload in the first place

            _addonLoader.DestroyLiveAddons();
            _addonLoader.ClearAddonTypes();
        }


        public string Name
        {
            get { return _location.Name; }
        }


        public Maybe<Assembly> Assembly
        {
            get { return _loaded.IsNull() ? Maybe<Assembly>.None : Maybe<Assembly>.With(_loaded); }
        }
    }
}