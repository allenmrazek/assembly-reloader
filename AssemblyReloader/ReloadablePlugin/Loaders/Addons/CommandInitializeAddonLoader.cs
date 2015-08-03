﻿using System;
using AssemblyReloader.Game;
using ReeperCommon.Containers;
using ReeperCommon.Logging;

namespace AssemblyReloader.ReloadablePlugin.Loaders.Addons
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class CommandInitializeAddonLoader : CommandCreateAddonsForScene
    {
        private readonly ILoadedAssemblyHandle _handle;

        public CommandInitializeAddonLoader(
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