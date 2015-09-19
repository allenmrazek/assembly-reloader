extern alias KSP;
using System.Linq;
using System;
using AssemblyReloader.ReloadablePlugin.Config;
using ReeperAssemblyLibrary;
using ReeperCommon.Logging;
using strange.extensions.injector;

namespace AssemblyReloader.Config
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class ReloadableReeperAssemblyLoader : ReeperAssemblyLoader, IReeperAssemblyUnloader
    {
        private readonly ILoadedAssemblyUninstaller _uninstaller;


        public ReloadableReeperAssemblyLoader(
            ILoadedAssemblyInstaller installer,
            ILoadedAssemblyUninstaller uninstaller,
            [Name(RawAssemblyDataKey.Woven)] IRawAssemblyDataFactory rawAssemblyDataFactory,
            ILog log) : base(installer, rawAssemblyDataFactory, log)
        {
            if (uninstaller == null) throw new ArgumentNullException("uninstaller");
            _uninstaller = uninstaller;
        }


        public void Unload(ILoadedAssemblyHandle loadedHandle)
        {
            if (loadedHandle == null) throw new ArgumentNullException("loadedHandle");

            if (!_cache.ContainsValue(loadedHandle.LoadedAssembly.assembly))
                throw new ReeperAssemblyNotInCacheException(loadedHandle); // todo: better exception

            var matchingEntry = _cache.Single(kvp => ReferenceEquals(kvp.Value, loadedHandle.LoadedAssembly.assembly));

            if (!_uninstaller.Uninstall(loadedHandle.LoadedAssembly.assembly))
                throw new Exception("Failed to uninstall " + loadedHandle.LoadedAssembly.name); // todo: better exception

            _cache.Remove(matchingEntry.Key);
        }
    }
}
