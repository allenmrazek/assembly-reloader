using System;
using ReeperAssemblyLibrary;
using ReeperCommon.Containers;
using ReeperCommon.Logging;

namespace AssemblyReloader.ReloadablePlugin.Loaders.Addons
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class CommandInitializeAddonLoaderWithNewHandle : CommandCreateAddonsForScene
    {
        private readonly ILoadedAssemblyHandle _handle;
        private readonly IAddonSettings _settings;
        private readonly SignalLoadersFinished _finished;

        public CommandInitializeAddonLoaderWithNewHandle(
            IReloadableAddonLoader addonLoader, 
            IGetCurrentStartupScene getCurrentScene, 
            ILoadedAssemblyHandle handle,
            IAddonSettings settings,
            SignalLoadersFinished finished,
            ILog log) : base(addonLoader, getCurrentScene, log)
        {
            if (handle == null) throw new ArgumentNullException("handle");
            if (settings == null) throw new ArgumentNullException("settings");
            if (finished == null) throw new ArgumentNullException("finished");

            _handle = handle;
            _settings = settings;
            _finished = finished;
        }

        public override void Execute()
        {
            AddonLoader.Handle = Maybe<ILoadedAssemblyHandle>.With(_handle);

            if (!_settings.CreateAddonsForCurrentSceneOnReload)
                return;

            _finished.AddOnce(CreateAddons); // wait until all loaders have a chance to run before creating addons
            Retain();
        }

        private void CreateAddons()
        {
            base.Execute();
            Release();
        }
    }
}
