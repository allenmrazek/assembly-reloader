using System.Reflection;
using AssemblyReloader.CompositeRoot;
using AssemblyReloader.Config;
using AssemblyReloader.StrangeIoC.extensions.context.api;
using ReeperCommon.FileSystem;
using ReeperCommon.FileSystem.Providers;
using ReeperCommon.Logging;
using ReeperCommon.Serialization;
using UnityEngine;

namespace AssemblyReloader.Common
{
    public class ReeperCommonContext : SignalContext
    {
        public ReeperCommonContext(MonoBehaviour view, ContextStartupFlags startup)
            : base(view, startup)
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


            var serializerSelector =
                new DefaultConfigNodeItemSerializerSelector(new SurrogateProvider(new[] 
                    { 
                        Assembly.GetExecutingAssembly(), 
                        typeof(IConfigNodeSerializer).Assembly 
                    }));

            injectionBinder.Bind<IConfigNodeSerializer>().To(
                new ConfigNodeSerializer(serializerSelector, new GetSerializableFieldsRecursiveType())).CrossContext();
        }


    }
}
