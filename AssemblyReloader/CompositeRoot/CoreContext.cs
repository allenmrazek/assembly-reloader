using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using AssemblyReloader.Commands;
using AssemblyReloader.Commands.old;
using AssemblyReloader.DataObjects;
using AssemblyReloader.FileSystem;
using AssemblyReloader.Game;
using AssemblyReloader.Names;
using AssemblyReloader.Properties;
using AssemblyReloader.Queries;
using AssemblyReloader.Queries.CecilQueries;
using AssemblyReloader.Queries.FileSystemQueries;
using AssemblyReloader.ReloadablePlugin;
using AssemblyReloader.ReloadablePlugin.Definition;
using AssemblyReloader.ReloadablePlugin.Definition.Operations;
using AssemblyReloader.ReloadablePlugin.Definition.Operations.old;
using AssemblyReloader.ReloadablePlugin.Loaders;
using AssemblyReloader.ReloadablePlugin.Loaders.Addons;
using AssemblyReloader.StrangeIoC.extensions.command.api;
using AssemblyReloader.StrangeIoC.extensions.command.impl;
using AssemblyReloader.StrangeIoC.extensions.context.impl;
using AssemblyReloader.TypeInstallers;
using AssemblyReloader.TypeInstallers.Impl;
using AssemblyReloader.Weaving.old;
using Mono.Cecil;
using Mono.Cecil.Cil;
using ReeperCommon.FileSystem;
using ReeperCommon.FileSystem.Providers;
using ReeperCommon.Logging;
using ReeperCommon.Repositories;
using ReeperCommon.Serialization;
using UnityEngine;
using AssemblyDefinitionWeaver = AssemblyReloader.Weaving.old.AssemblyDefinitionWeaver;

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

            var namespaces = new string[]
            {
                "AssemblyReloader.FileSystem",
                "AssemblyReloader.Game",
                "AssemblyReloader.Generators",
                "AssemblyReloader.ReloadablePlugin"
            };
            implicitBinder.ScanForAnnotatedClasses(namespaces);

            injectionBinder.Bind<ILog>().To(log);

            injectionBinder.Bind<IPluginConfigurationFilePathQuery>().To(new PluginConfigurationFilePathQuery());
            injectionBinder.Bind<IFileSystemFactory>()
                .ToValue(new KSPFileSystemFactory(new KSPUrlDir(new KSPGameDataUrlDirProvider().Get())));
            injectionBinder.Bind<IEnumerable<ILoadedAssemblyTypeInstaller>>().To(CreateTypeInstallers());

            injectionBinder.Bind<IDirectory>()
                .ToValue(injectionBinder.GetInstance<IFileSystemFactory>().GetGameDataDirectory())
                .ToName(DirectoryNames.GameData);

            injectionBinder.Bind<IDirectory>()
                .ToValue(new AssemblyDirectoryQuery(
                    injectionBinder.GetInstance<IGameAssemblyLoader>(),
                    Assembly.GetExecutingAssembly(),
                    injectionBinder.GetInstance<IDirectory>(DirectoryNames.GameData)).Get())
                .ToName(DirectoryNames.Core);

            injectionBinder.Bind<IConfigNodeSerializer>().ToValue(
                new ConfigNodeSerializer(new DefaultSurrogateSelector(new DefaultSurrogateProvider()),
                    new GetFieldInfoComposite(new GetSerializableFieldsRecursiveType())));

            var assemblyResolver = new DefaultAssemblyResolver();
            assemblyResolver.AddSearchDirectory(injectionBinder.GetInstance<IDirectory>(DirectoryNames.Core).FullPath);

            injectionBinder.Bind<BaseAssemblyResolver>().ToValue(assemblyResolver);

            injectionBinder.Bind<IUnityObjectDestroyer>()
                .ToValue(new UnityObjectDestroyer(new PluginReloadRequestedMethodCallCommand()));

            injectionBinder.Bind<IResourceRepository>()
                .ToValue(ConfigureResourceRepository(injectionBinder.GetInstance<IDirectory>(DirectoryNames.Core)));

            injectionBinder.Bind<IGetAttributesOfType<KSPAddon>>().To<GetAttributesOfType<KSPAddon>>().ToSingleton();
            injectionBinder.Bind<IGetTypesFromAssembly<AddonType>>().To<GetAddonTypesFromAssembly>().ToSingleton();

            injectionBinder.Bind<IAssemblyProviderFactory>().To(new AssemblyProviderFactory(
                injectionBinder.GetInstance<BaseAssemblyResolver>(),
                injectionBinder.GetInstance<IGetDebugSymbolsExistForDefinition>(),
                injectionBinder.GetInstance<IWeaveOperationFactory>(),
                injectionBinder.GetInstance<IGetTypeDefinitions>(),
                injectionBinder.GetInstance<IGetMethodDefinitions>(),
                () => true)).ToSingleton();

            var reloadableFileQuery =
                new ReloadableAssemblyFilesInDirectoryQuery(
                    injectionBinder.GetInstance<IDirectory>(DirectoryNames.GameData));

            log.Normal("Reloadable files count: " + reloadableFileQuery.Get().Count());

            var reloadablePluginFactory = injectionBinder.GetInstance<ReloadablePluginFactory>();

            if (reloadablePluginFactory == null) throw new Exception("failed to create plugin factory");



            foreach (var file in reloadableFileQuery.Get())
            {
                log.Normal("Performing initial load of " + file.FileName);

                var r = reloadablePluginFactory.Create(file);
            }

            log.Normal("Finished loading initial reloadable plugins.");

            //injectionBinder.Bind<IEnumerable<IPluginInfo>>().ToValue(reloadablePlugins.Cast<IPluginInfo>());
            //injectionBinder.Bind<IEnumerable<IReloadablePlugin>>().ToValue(reloadablePlugins);














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


        private IEnumerable<ILoadedAssemblyTypeInstaller> CreateTypeInstallers()
        {
            return new ILoadedAssemblyTypeInstaller[]
            {
                new GenericLoadedAssemblyTypeInstaller<Part>(new GetTypesDerivedFrom<Part>()),
                new GenericLoadedAssemblyTypeInstaller<PartModule>(new GetTypesDerivedFrom<PartModule>()),
                new GenericLoadedAssemblyTypeInstaller<ScenarioModule>(new GetTypesDerivedFrom<ScenarioModule>())
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
            var injectedHelperTypeQuery = new GetInjectedHelperTypeFromDefinition();

            var allTypesFromAssemblyExceptInjected = new GetTypeDefinitionsExcluding(
                new GetAllTypesFromDefinition(), new GetInjectedHelperTypeFromDefinition());

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
                    new InjectedHelperTypeGetMethod(injectedHelperTypeQuery, getCodeBaseProperty.GetGetMethod().Name)
                );

            var interceptAssemblyLocationCalls = new InterceptExecutingAssemblyLocationQueries(
                new MethodCallInMethodBodyQuery(
                    getLocationProperty.GetGetMethod(),
                    OpCodes.Callvirt),
                new InjectedHelperTypeGetMethod(injectedHelperTypeQuery, getLocationProperty.GetGetMethod().Name)
                );

            return new AssemblyDefinitionWeaver(
                weaverLog,
                allTypesFromAssemblyExceptInjected,
                new GetAllMethodDefinitions(),
                renameAssembly,
                new ConditionalWeaveOperation(writeInjectedHelper, () => pluginConfiguration.InjectHelperType),
                new ConditionalWeaveOperation(interceptAssemblyCodeBaseCalls, () => pluginConfiguration.RewriteAssemblyLocationCalls),
                new ConditionalWeaveOperation(interceptAssemblyLocationCalls, () => pluginConfiguration.RewriteAssemblyLocationCalls));

        }
    }
}
