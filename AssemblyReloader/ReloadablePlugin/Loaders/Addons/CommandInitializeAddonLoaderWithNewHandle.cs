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

        public CommandInitializeAddonLoaderWithNewHandle(
            IReloadableAddonLoader addonLoader, 
            IGetCurrentStartupScene getCurrentScene, 
            ILoadedAssemblyHandle handle,
            IAddonSettings settings,
            ILog log) : base(addonLoader, getCurrentScene, settings, log)
        {
            if (handle == null) throw new ArgumentNullException("handle");

            _handle = handle;
        }

        public override void Execute()
        {
            AddonLoader.Handle = Maybe<ILoadedAssemblyHandle>.With(_handle);
            base.Execute();
        }
    }
}
