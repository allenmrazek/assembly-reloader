using ReeperCommon.FileSystem;
using ReeperCommon.FileSystem.Providers;
using ReeperCommon.Logging;
using UnityEngine;

namespace AssemblyReloader.Common
{
    public class ReeperCommonContext : SignalContext
    {
        public ReeperCommonContext(MonoBehaviour view) : base(view)
        {
        }


        protected override void mapBindings()
        {
            base.mapBindings();

            var log = new DebugLog("ART");

            // cross context bindings
            injectionBinder.Bind<ILog>().To(log).ToSingleton().CrossContext();

            injectionBinder.Bind<IUrlDirProvider>().To<KSPGameDataUrlDirProvider>().ToSingleton();
            injectionBinder.Bind<IUrlDir>().To(new KSPUrlDir(injectionBinder.GetInstance<IUrlDirProvider>().Get())).CrossContext();
            injectionBinder.Bind<IFileSystemFactory>().To<KSPFileSystemFactory>().ToSingleton().CrossContext();

        }


    }
}
