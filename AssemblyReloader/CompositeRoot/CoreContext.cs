﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using AssemblyReloader.Commands;
using AssemblyReloader.Commands.old;
using AssemblyReloader.Configuration;
using AssemblyReloader.Configuration.Names;
using AssemblyReloader.DataObjects;
using AssemblyReloader.FileSystem;
using AssemblyReloader.Game;
using AssemblyReloader.Gui;
using AssemblyReloader.Properties;
using AssemblyReloader.Providers;
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
using AssemblyReloader.Weaving.old;
using Mono.Cecil;
using Mono.Cecil.Cil;
using ReeperCommon.FileSystem;
using ReeperCommon.FileSystem.Providers;
using ReeperCommon.Gui.Window;
using ReeperCommon.Gui.Window.View;
using ReeperCommon.Logging;
using ReeperCommon.Repositories;
using ReeperCommon.Serialization;
using UnityEngine;
using AssemblyDefinitionWeaver = AssemblyReloader.Weaving.old.AssemblyDefinitionWeaver;
using Object = UnityEngine.Object;

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

            injectionBinder.Bind<ILog>().To(log).ToSingleton();

            injectionBinder.Bind<IGetConfigurationFilePath>().To(new GetConfigurationFilePath());
            injectionBinder.Bind<IFileSystemFactory>()
                .ToValue(new KSPFileSystemFactory(new KSPUrlDir(new KSPGameDataUrlDirProvider().Get())));

            injectionBinder.Bind<IDirectory>()
                .ToValue(injectionBinder.GetInstance<IFileSystemFactory>().GetGameDataDirectory())
                .ToName(DirectoryNames.GameData);

            injectionBinder.Bind<IDirectory>()
                .ToValue(new GetAssemblyDirectory(
                    injectionBinder.GetInstance<IGameAssemblyLoader>(),
                    Assembly.GetExecutingAssembly(),
                    injectionBinder.GetInstance<IDirectory>(DirectoryNames.GameData)).Get())
                .ToName(DirectoryNames.Core);

            injectionBinder.Bind<IConfigNodeSerializer>().To(
                new ConfigNodeSerializer(new DefaultSurrogateSelector(new DefaultSurrogateProvider()),
                    new GetSerializableFieldsRecursiveType())).ToSingleton();


            var assemblyResolver = new DefaultAssemblyResolver();
            assemblyResolver.AddSearchDirectory(injectionBinder.GetInstance<IDirectory>(DirectoryNames.Core).FullPath);

            injectionBinder.Bind<BaseAssemblyResolver>().To(assemblyResolver).ToSingleton();

            injectionBinder.Bind<Assembly>().To(Assembly.GetExecutingAssembly()).ToName(AssemblyNames.Core).ToSingleton();

            injectionBinder.Bind<IUnityObjectDestroyer>()
                .ToValue(new UnityObjectDestroyer(new PluginReloadRequestedMethodCallCommand()));

            injectionBinder.Bind<IResourceRepository>()
                .ToValue(ConfigureResourceRepository(injectionBinder.GetInstance<IDirectory>(DirectoryNames.Core)));

            injectionBinder.Bind<WindowFactory>().To<WindowFactory>();
            injectionBinder.Bind<GUIStyle>().To(ConfigureTitleBarButtonStyle()).ToName(Styles.TitleBarButtonStyle).ToSingleton();

            BindTextureToName(injectionBinder.GetInstance<IResourceRepository>(), "Resources/btnClose", TextureNames.CloseButton);
            BindTextureToName(injectionBinder.GetInstance<IResourceRepository>(), "Resources/btnWrench", TextureNames.SettingsButton);
            BindTextureToName(injectionBinder.GetInstance<IResourceRepository>(), "Resources/cursor", TextureNames.ResizeCursor);


            injectionBinder.Bind<IGetAttributesOfType<KSPAddon>>().To<GetAttributesOfType<KSPAddon>>().ToSingleton();
            injectionBinder.Bind<IGetTypesFromAssembly<AddonType>>().To<GetAddonTypesFromAssembly>().ToSingleton();
            injectionBinder.Bind<IGetAssemblyFileLocation>().To<GetAssemblyFileLocation>().ToSingleton();

            injectionBinder.Bind<IFile>()
                .ToValue(GetAssemblyFileLocation(Assembly.GetExecutingAssembly()))
                .ToName(FileNames.Core);

            injectionBinder.Bind<IFilePathProvider<Assembly>>().To<GetAssemblyConfigPath>().ToSingleton();

            injectionBinder.Bind<ConfigurationProvider>().To<ConfigurationProvider>().ToSingleton();

            injectionBinder.Bind<Configuration.Configuration>()
                .To(injectionBinder.GetInstance<ConfigurationProvider>().Get())
                .ToSingleton();

            //injectionBinder.Bind<IConfigurationProvider>().To<ConfigurationProvider>().ToSingleton();


            //injectionBinder.Bind<Configuration>().To(injectionBinder.GetInstance<IConfigurationProvider>().Get()).ToSingleton();
  


            //injectionBinder.Bind<IAssemblyProviderFactory>().To(new AssemblyProviderFactory(
            //    injectionBinder.GetInstance<BaseAssemblyResolver>(),
            //    injectionBinder.GetInstance<IGetDebugSymbolsExistForDefinition>(),
            //    injectionBinder.GetInstance<IWeaveOperationFactory>(),
            //    new GetTypeDefinitionsExcluding(new GetAllTypesFromDefinition(),
            //        new GetInjectedHelperTypeFromDefinition()),
            //    new GetAllMethodDefinitions(),
            //    log.CreateTag("ILWeaver"),
            //    () => true)).ToSingleton();

            var reloadableFileQuery =
                new ReloadableAssemblyFilesInDirectoryQuery(
                    injectionBinder.GetInstance<IDirectory>(DirectoryNames.GameData));

            log.Normal("Reloadable files count: " + reloadableFileQuery.Get().Count());

            var reloadablePluginFactory = injectionBinder.GetInstance<ReloadablePluginFactory>();

            if (reloadablePluginFactory == null) throw new Exception("failed to create plugin factory");



            var reloadablePlugins = reloadableFileQuery.Get().Select(file =>
            {
                var plugin = reloadablePluginFactory.Create(file);

                plugin.Reload();

                return plugin;
            }).ToList();

            injectionBinder.Bind<IEnumerable<IPluginInfo>>().To(reloadablePlugins.Cast<IPluginInfo>()).ToSingleton();

            log.Normal("Finished loading initial reloadable plugins.");







            injectionBinder.Bind<IViewMediator>().To<ViewMediator>().ToSingleton();

            // create main window ...
            injectionBinder.Bind<IMainView>()
                .To<MainWindowLogic>();
                //.ToSingleton();


            var windowFactory = injectionBinder.GetInstance<WindowFactory>();

            windowFactory.CreateMainWindow(injectionBinder.GetInstance<IMainView>(),
                injectionBinder.GetInstance<IViewMediator>());


            // create settings window
            injectionBinder.Bind<ISettingsView>()
                .To<SettingsWindowLogic>()
                .ToSingleton();

            windowFactory.CreateSettingsWindow(injectionBinder.GetInstance<ISettingsView>(),
                injectionBinder.GetInstance<IViewMediator>());


        }


        protected override void addCoreComponents()
        {
            base.addCoreComponents();
            injectionBinder.Unbind<ICommandBinder>();
            injectionBinder.Bind<ICommandBinder>().To<SignalCommandBinder>().ToSingleton();
        }

      
        private IResourceRepository ConfigureResourceRepository(IDirectory dllDirectory)
        {
            // Removes extension from string, if an extension exists
            // Also converts windows-style \\ to / 
            Func<string, string> stripExtensionFromId = id =>
            {
                if (!Path.HasExtension(id) || string.IsNullOrEmpty(id)) return id;

                var dir = Path.GetDirectoryName(id) ?? "";
                var woExt = Path.Combine(dir, Path.GetFileNameWithoutExtension(id)).Replace('\\', '/');

                return !string.IsNullOrEmpty(woExt) ? woExt : id;
            };

            // Appends assembly name and replaces slashes of both types with periods to come up with a manifest resource name
            Func<string, string> convertUrlToResourceName = id =>
            {
                var prepend = Assembly.GetExecutingAssembly().GetName().Name + ".";

                if (!id.StartsWith(prepend))
                    id = prepend + id;

                return id.Replace('/', '.').Replace('\\', '.');
            };

            var currentAssemblyResource = new ResourceFromEmbeddedResource(Assembly.GetExecutingAssembly());


            return new ResourceRepositoryComposite(
                // search GameDatabase first
                //   note: GameDatabase expects extensionless urls
                new ResourceIdentifierAdapter(stripExtensionFromId, new ResourceFromGameDatabase()),

                // then look at physical file system. These work on a list of items cached
                // by GameDatabase rather than working directly with the disk (unless a resource 
                // is accessed from here, of course)
                new ResourceFromDirectory(dllDirectory),

                // finally search embedded resource
                // we need to handle both cases of the url containing the extension or not...
                new ResourceRepositoryComposite(
                    // don't strip extension
                    new ResourceIdentifierAdapter(convertUrlToResourceName, currentAssemblyResource),

                    // strip extension
                    new ResourceIdentifierAdapter(stripExtensionFromId,
                        new ResourceIdentifierAdapter(convertUrlToResourceName, currentAssemblyResource)),
                        
                    // there's a potential third issue: if the incoming id doesn't contain an extension,
                    // we might not fully match any of the manifest resource names. We'll add a special adapter
                    // that will select the most-closely matching name if there's only one match
                    new ResourceIdentifierAdapter(convertUrlToResourceName,
                        new ResourceIdentifierAdapter(s =>
                        {
                            var resourceNames = Assembly.GetExecutingAssembly().GetManifestResourceNames()
                                .Where(name => name.StartsWith(s))
                                .ToList();

                            return resourceNames.Count == 1 ? resourceNames.First() : s;
                        }, currentAssemblyResource)
                        )));   
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
                    new GetInjectedHelperTypeMethod(injectedHelperTypeQuery, getCodeBaseProperty.GetGetMethod().Name)
                );

            var interceptAssemblyLocationCalls = new InterceptExecutingAssemblyLocationQueries(
                new MethodCallInMethodBodyQuery(
                    getLocationProperty.GetGetMethod(),
                    OpCodes.Callvirt),
                new GetInjectedHelperTypeMethod(injectedHelperTypeQuery, getLocationProperty.GetGetMethod().Name)
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


        private static GUIStyle ConfigureTitleBarButtonStyle()
        {
            var style = new GUIStyle(HighLogic.Skin.button) { border = new RectOffset(), padding = new RectOffset() };
            style.fixedHeight = style.fixedWidth = 16f;
            style.margin = new RectOffset();

            return style;
        }


        private void BindTextureToName(IResourceRepository resourceRepo, string url, object name)
        {
            if (resourceRepo == null) throw new ArgumentNullException("resourceRepo");
            if (name == null) throw new ArgumentNullException("name");
            if (string.IsNullOrEmpty(url)) throw new ArgumentException("url is null or empty");

            var tex = resourceRepo.GetTexture(url);

            if (!tex.Any())
                throw new Exception("Couldn't bind texture at \"" + url + "\"; texture not found");

            injectionBinder.Bind<Texture2D>().ToValue(tex.Single()).ToName(name);
        }


        private IFile GetAssemblyFileLocation(Assembly target)
        {
            if (target == null) throw new ArgumentNullException("target");

            var mf = injectionBinder.GetInstance<IGetAssemblyFileLocation>().Get(target);

            if (!mf.Any()) throw new Exception("Failed to find file location of " + target.FullName);
            return mf.Single();
        }



    }
}
