﻿using System;
using System.Reflection;
using ReeperCommon.Extensions;

namespace AssemblyReloader.Game
{
    public class KspAddonLoader : IAddonLoader
    {
        public void StartAddons(Assembly assembly, KSPAddon.Startup scene)
        {
            var loadedAssembly = AssemblyLoader.loadedAssemblies.GetByAssembly(assembly);

            if (loadedAssembly.IsNull())
                throw new InvalidOperationException(assembly.FullName + " has not been loaded into KSP AssemblyLoader!");

            // this is pretty hacky but the goal here is to sneak addons that should be created
            // for this scene into AddonLoader's runOnce list without double-creating other addons
            //
            // todo: intercept calls to AssemblyLoader.loadedAssemblies and redirect them to a proxy object
            // which contains all of them in case client plugin is searching through that list
            var cache = AssemblyLoader.loadedAssemblies;

            try
            {
                AssemblyLoader.loadedAssemblies = new AssemblyLoader.LoadedAssembyList { loadedAssembly };
                AddonLoader.Instance.StartAddons(scene);
            }
            finally
            {
                AssemblyLoader.loadedAssemblies = cache;
            }
        }
    }
}
