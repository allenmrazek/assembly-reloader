extern alias KSP;
using System.Linq;
using System;
using AssemblyReloader.ReloadablePlugin.Config;
using ReeperAssemblyLibrary;
using ReeperCommon.Containers;
using ReeperCommon.Logging;
using strange.extensions.injector;

namespace AssemblyReloader.Config
{
    /// 
    /// AssemblyUnloader is logically linked to the loader because the loader keepts a list of 
    /// "active" ReeperAssemblies that will eventually be used to resolve references between reloadable 
    /// assemblies. It makes sense to compose the unload functionality here too to keep that list 
    /// up to date
    /// 
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

            if (!_uninstaller.Uninstall(loadedHandle.LoadedAssembly.assembly))
                Log.Error("Failed to uninstall " + loadedHandle.LoadedAssembly.name + "; was it removed by the plugin itself?");
        }
    }
}
