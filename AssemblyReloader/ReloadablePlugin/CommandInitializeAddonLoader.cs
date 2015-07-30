using System;
using AssemblyReloader.Game;
using AssemblyReloader.ReloadablePlugin.Loaders;
using AssemblyReloader.ReloadablePlugin.Loaders.Addons;
using ReeperCommon.Containers;
using ReeperCommon.Logging;

namespace AssemblyReloader.ReloadablePlugin
{
    public class CommandInitializeAddonLoader : CommandCreateAddonsForScene
    {
        private readonly ILoadedAssemblyHandle _handle;

        public CommandInitializeAddonLoader(
            IReloadableAddonLoader addonLoader, 
            IGetCurrentStartupScene getCurrentScene, 
            ILoadedAssemblyHandle handle,
            ILog log) : base(addonLoader, getCurrentScene, log)
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
