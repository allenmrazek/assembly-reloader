using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using AssemblyReloader.Annotations;
using AssemblyReloader.Commands;
using AssemblyReloader.Controllers;
using AssemblyReloader.DataObjects;
using AssemblyReloader.Game;
using AssemblyReloader.Generators;
using AssemblyReloader.Gui;
using AssemblyReloader.Loaders;
using AssemblyReloader.Providers;
using AssemblyReloader.Queries;
using AssemblyReloader.Queries.AssemblyQueries;
using AssemblyReloader.Queries.CecilQueries;
using AssemblyReloader.Queries.FileSystemQueries;
using AssemblyReloader.TypeInstallers;
using AssemblyReloader.TypeInstallers.Impl;
using AssemblyReloader.Weaving;
using AssemblyReloader.Weaving.Operations;
using Mono.Cecil;
using Mono.Cecil.Cil;
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
using AssemblyLoader = AssemblyReloader.Loaders.AssemblyLoader;

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
            var log = new DebugLog("ART");

            injectionBinder.Bind<ILog>().To(log);
            

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

            injectionBinder.Bind<IEnumerable<ITypeInstaller>>().ToValue(CreateTypeInstallers());
            injectionBinder.Bind<ILoadedAssemblyFactory>().To<KspLoadedAssemblyFactory>().ToSingleton();
            injectionBinder.Bind<IPluginConfigurationFilePathQuery>()
                .To(new PluginConfigurationFilePathQuery());
            injectionBinder.Bind<IPluginConfigurationProvider>().To<PluginConfigurationProvider>().ToSingleton();
            injectionBinder.Bind<IAssemblyLoader>().To<Loaders.AssemblyLoader>();
            injectionBinder.Bind<IAssemblyProvider>().To<AssemblyProvider>();
            injectionBinder.Bind<IAssemblyDefinitionReader>().To<AssemblyDefinitionFromDiskReader>();
            injectionBinder.Bind<IAssemblyDefinitionLoader>().To<AssemblyDefinitionLoader>().ToSingleton();
            injectionBinder.Bind<IDebugSymbolFileExistsQuery>().To<DebugSymbolFileExistsQuery>();
            injectionBinder.Bind<ITemporaryFileFactory>().To<TemporaryFileFactory>();
            injectionBinder.Bind<ILoadedAssemblyFileUrlQuery>().To<LoadedAssemblyFileUrlQuery>().ToSingleton();
            injectionBinder.Bind<IDisposeLoadedAssemblyCommandFactory>()
                .To<DisposeLoadedAssemblyCommandFactory>()
                .ToSingleton();

            injectionBinder.Bind<IReloadablePlugin>().To<IPluginInfo>().To<ReloadablePlugin>();


            var reloadableFiles =
                new ReloadableAssemblyFilesInDirectoryQuery(
                    injectionBinder.GetInstance<IDirectory>(DirectoryTypes.GameData));

            log.Normal("Reloadable files count: " + reloadableFiles.Get().Count());

            var reloadablePlugins = reloadableFiles.Get().Select(file =>
            {
                log.Normal("Loading " + file.Name);
                injectionBinder.Bind<IFile>().ToValue(file);
                injectionBinder.Bind<IDirectory>().ToValue(file.Directory);
                injectionBinder.Bind<PluginConfiguration>()
                    .ToValue(injectionBinder.GetInstance<IPluginConfigurationProvider>().Get(file));
                injectionBinder.Bind<IAssemblyDefinitionWeaver>().To(ConfigureDefinitionWeaver(
                    file, injectionBinder.GetInstance<PluginConfiguration>(), injectionBinder.GetInstance<ILog>().CreateTag("Weaver")));
                

                var r = injectionBinder.GetInstance<IReloadablePlugin>();

                

                r.Load();


                injectionBinder.Unbind<IAssemblyDefinitionWeaver>();
                injectionBinder.Unbind<IDirectory>();
                injectionBinder.Unbind<IFile>();
                injectionBinder.Unbind<PluginConfiguration>();

                return r;
            })
            .ToList();


            injectionBinder.Bind<IEnumerable<IPluginInfo>>().ToValue(reloadablePlugins.Cast<IPluginInfo>());
            injectionBinder.Bind<IEnumerable<IReloadablePlugin>>().ToValue(reloadablePlugins);














            //mediationBinder.Bind<StrangeWindowView>()
            //    .To<MainWindowMediator>();








            //var mainLogic = new MainWindowLogic(new Rect(0, 0, 400, 400), 554, HighLogic.Skin);
            //injectionBinder.Bind<IWindowComponent>()
            //    .ToValue(mainLogic);




            // create main window ...
            //var logic = new MainWindowLogic(new Rect(0, 0, 400, 400), 554, HighLogic.Skin);
            //var view = StrangeWindowView.Create(logic);
            //var view = new GameObject("TestView", typeof (StrangeWindowView));
            //view.transform.parent = ((GameObject)contextView).transform;
            //Object.DontDestroyOnLoad(view);

            
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

         


        public override void Launch()
        {
            base.Launch();
            //Make sure you've mapped this to a StartCommand!
            //StartSignal startSignal = (StartSignal)injectionBinder.GetInstance<StartSignal>();
            //startSignal.Dispatch();
        }


        private IEnumerable<ITypeInstaller> CreateTypeInstallers()
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


        private IAssemblyDefinitionWeaver ConfigureDefinitionWeaver([NotNull] IFile location,
            [NotNull] PluginConfiguration pluginConfiguration,
            [NotNull] ILog weaverLog)
        {
            if (location == null) throw new ArgumentNullException("location");
            if (pluginConfiguration == null) throw new ArgumentNullException("pluginConfiguration");
            if (weaverLog == null) throw new ArgumentNullException("weaverLog");

            var getCodeBaseProperty = typeof(Assembly).GetProperty("CodeBase",
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);

            var getLocationProperty = typeof(Assembly).GetProperty("Location",
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);

            if (getCodeBaseProperty == null || getCodeBaseProperty.GetGetMethod() == null)
                throw new MissingMethodException(typeof(Assembly).FullName, "CodeBase");

            if (getLocationProperty == null || getCodeBaseProperty.GetGetMethod() == null)
                throw new MissingMethodException(typeof(Assembly).FullName, "Location");


            var uri = new Uri(location.FullPath);
            var injectedHelperTypeQuery = new InjectedHelperTypeQuery();

            var allTypesFromAssemblyExceptInjected = new ExcludingTypeDefinitions(
                new AllTypesFromDefinitionQuery(), new InjectedHelperTypeQuery());

            var renameAssembly = new RenameAssemblyOperation(new UniqueAssemblyNameGenerator(new RandomStringGenerator()));

            var writeInjectedHelper =
                    new InjectedHelperTypeDefinitionWriter(
                    new CompositeCommand<TypeDefinition>(
                        new ProxyAssemblyMethodWriter(Uri.UnescapeDataString(uri.AbsoluteUri), getCodeBaseProperty.GetGetMethod()),
                        new ProxyAssemblyMethodWriter(uri.LocalPath, getLocationProperty.GetGetMethod())));

            var interceptAssemblyCodeBaseCalls = new InterceptExecutingAssemblyLocationQueries(
                new MethodCallInMethodBodyQuery(
                    getCodeBaseProperty.GetGetMethod(),
                    OpCodes.Callvirt),
                    new InjectedHelperTypeMethodQuery(injectedHelperTypeQuery, getCodeBaseProperty.GetGetMethod().Name)
                );

            var interceptAssemblyLocationCalls = new InterceptExecutingAssemblyLocationQueries(
                new MethodCallInMethodBodyQuery(
                    getLocationProperty.GetGetMethod(),
                    OpCodes.Callvirt),
                new InjectedHelperTypeMethodQuery(injectedHelperTypeQuery, getLocationProperty.GetGetMethod().Name)
                );

            return new AssemblyDefinitionWeaver(
                weaverLog,
                allTypesFromAssemblyExceptInjected,
                new AllMethodsFromDefinitionQuery(),
                renameAssembly,
                new ConditionalWeaveOperation(writeInjectedHelper, () => pluginConfiguration.InjectHelperType),
                new ConditionalWeaveOperation(interceptAssemblyCodeBaseCalls, () => pluginConfiguration.RewriteAssemblyLocationCalls),
                new ConditionalWeaveOperation(interceptAssemblyLocationCalls, () => pluginConfiguration.RewriteAssemblyLocationCalls));

        }
    }
}
