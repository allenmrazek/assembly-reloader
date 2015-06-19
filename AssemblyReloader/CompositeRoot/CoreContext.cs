using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using AssemblyReloader.Commands;
using AssemblyReloader.DataObjects;
using AssemblyReloader.Game;
using AssemblyReloader.Generators;
using AssemblyReloader.Gui;
using AssemblyReloader.Queries;
using AssemblyReloader.Queries.AssemblyQueries;
using AssemblyReloader.Queries.FileSystemQueries;
using AssemblyReloader.TypeInstallers;
using Mono.Cecil;
using ReeperCommon.FileSystem;
using ReeperCommon.FileSystem.Providers;
using ReeperCommon.Gui.Window;
using ReeperCommon.Logging;
using ReeperCommon.Repositories;
using ReeperCommon.Serialization;
using strange.extensions.command.api;
using strange.extensions.command.impl;
using strange.extensions.context.impl;
using UnityEngine;

namespace AssemblyReloader.CompositeRoot
{
    class CoreContext : MVCSContext
    {
        public CoreContext(MonoBehaviour view, bool autoStartup)
            : base(view, autoStartup)
        {
        }
        
        protected override void mapBindings()
        {
            
            injectionBinder.Bind<ILog>().To(new DebugLog("ART"));


            injectionBinder.Bind<ITypeIdentifier>().To<TypeIdentifier>().ToSingleton();
            injectionBinder.Bind<IRandomStringGenerator>().To<RandomStringGenerator>().ToSingleton();
            injectionBinder.Bind<IGameObjectProvider>().To<KspGameObjectProvider>().ToSingleton();
            injectionBinder.Bind<IKspFactory>().To<KspFactory>().ToSingleton();
            injectionBinder.Bind<IFileSystemFactory>()
                .ToValue(new KSPFileSystemFactory(new KSPUrlDir(new KSPGameDataUrlDirProvider().Get())));
            injectionBinder.Bind<IGameAssemblyLoader>().To<KspAssemblyLoader>().ToSingleton();

            injectionBinder.Bind<IDirectory>()
                .ToValue(injectionBinder.GetInstance<IFileSystemFactory>().GetGameDataDirectory())
                .ToName(DirectoryTypes.GameData);

            injectionBinder.Bind<IDirectory>()
                .ToValue(new AssemblyDirectoryQuery(
                    injectionBinder.GetInstance<IGameAssemblyLoader>(),
                    Assembly.GetExecutingAssembly(),
                    injectionBinder.GetInstance<IDirectory>(DirectoryTypes.GameData)).Get())
                .ToName(DirectoryTypes.Core);

            injectionBinder.Bind<IConfigNodeSerializer>().ToValue(
                new ConfigNodeSerializer(new DefaultSurrogateSelector(new DefaultSurrogateProvider()),
                    new CompositeFieldInfoQuery(new RecursiveSerializableFieldQuery())));

            var assemblyResolver = new DefaultAssemblyResolver();
            assemblyResolver.AddSearchDirectory(injectionBinder.GetInstance<IDirectory>(DirectoryTypes.Core).FullPath);

            injectionBinder.Bind<BaseAssemblyResolver>().ToValue(assemblyResolver);

            injectionBinder.Bind<IUnityObjectDestroyer>()
                .ToValue(new UnityObjectDestroyer(new PluginReloadRequestedMethodCallCommand()));

            injectionBinder.Bind<IAddonAttributesFromTypeQuery>().To<AddonAttributesFromTypeQuery>().ToSingleton();
            injectionBinder.Bind<IResourceRepository>()
                .ToValue(ConfigureResourceRepository(injectionBinder.GetInstance<IDirectory>(DirectoryTypes.Core)));

            injectionBinder.Bind<IEnumerable<ITypeInstaller>>().ToValue(LocateTypeInstallers());
            injectionBinder.Bind<ILoadedAssemblyFactory>().To<KspLoadedAssemblyFactory>().ToSingleton();


            mediationBinder.Bind<StrangeWindowView>()
                .To<MainWindowMediator>();

            injectionBinder.Bind<IWindowComponent>()
                .ToValue(new MainWindowLogic(new Rect(0, 0, 400, 400), 554, HighLogic.Skin));


            // create main window ...
            //var logic = new MainWindowLogic(new Rect(0, 0, 400, 400), 554, HighLogic.Skin);
            //var view = StrangeWindowView.Create(logic);
            var view = new GameObject("TestView", typeof (StrangeWindowView));

            UnityEngine.Object.DontDestroyOnLoad(view);


            //injectionBinder.Bind<IExampleModel>()
            //    .To<ExampleModel>()
            //    .ToSingleton();
            //injectionBinder.Bind<IExampleService>()
            //    .To<ExampleService>()
            //    .ToSingleton();

            //mediationBinder.Bind<ExampleView>()
            //    .To<ExampleMediator>();

            //commandBinder.Bind(ExampleEvent.REQUEST_WEB_SERVICE)
            //    .To<CallWebServiceCommand>();
            //commandBinder.Bind(ContextEvent.START)
            //    .To<StartCommand>().Once ();

            //injectionBinder.GetInstance<ILog>().Warning("It seems to work!");


        }


        protected override void addCoreComponents()
        {
            base.addCoreComponents();
            injectionBinder.Unbind<ICommandBinder>();
            injectionBinder.Bind<ICommandBinder>().To<SignalCommandBinder>().ToSingleton();
        }


        //private ILoadedAssemblyFactory ConfigureLoadedAssemblyFactory()
        //{
        //    return new KspLoadedAssemblyFactory(
        //        new LoadedAssemblyFileUrlQuery(),
        //        new DisposeLoadedAssemblyCommandFactory(),
        //        // note: no KSPAddon type installer required; game looks through LoadedAssemblies on every scene checking all types
        //        new GenericTypeInstaller<Part>(new TypesDerivedFromQuery<Part>()),
        //        new GenericTypeInstaller<PartModule>(new TypesDerivedFromQuery<PartModule>()),
        //        new GenericTypeInstaller<ScenarioModule>(new TypesDerivedFromQuery<ScenarioModule>()));
        //}

        private IEnumerable<ITypeInstaller> LocateTypeInstallers()
        {
            return new ITypeInstaller[]
            {
                new GenericTypeInstaller<Part>(new TypesDerivedFromQuery<Part>()),
                new GenericTypeInstaller<PartModule>(new TypesDerivedFromQuery<PartModule>()),
                new GenericTypeInstaller<ScenarioModule>(new TypesDerivedFromQuery<ScenarioModule>())
            };
        }




        private IResourceRepository ConfigureResourceRepository(IDirectory dllDirectory)
        {
            return new ResourceRepositoryComposite(
                // search GameDatabase first
                //   note: GameDatabase expects extensionless urls
                    new ResourceIdentifierAdapter(id =>
                    {
                        if (!Path.HasExtension(id) || string.IsNullOrEmpty(id)) return id;

                        var dir = Path.GetDirectoryName(id) ?? "";
                        var woExt = Path.Combine(dir, Path.GetFileNameWithoutExtension(id)).Replace('\\', '/');

                        return !string.IsNullOrEmpty(woExt) ? woExt : id;
                    }, new ResourceFromGameDatabase()
                    ),

                // then look at physical file system. These work on a list of items cached
                // by GameDatabase rather than working directly with the disk (unless a resource 
                // is accessed from here, of course)
                    new ResourceFromDirectory(dllDirectory),


                // finally search embedded resource
                //   note: embedded resource ids should be appended by assembly namespace
                    new ResourceIdentifierAdapter(id => Assembly.GetExecutingAssembly().GetName().Name + "." + id,

                //   note: expects resource manifest ids (periods instead of slashes), so 
                //         identifier transformer is needed if we want to specify input identifiers
                //         as though they were paths
                    new ResourceIdentifierAdapter(id => id.Replace('/', '.').Replace('\\', '.'),
                        new ResourceFromEmbeddedResource(Assembly.GetExecutingAssembly()))
                    ));
        }
    }
}
