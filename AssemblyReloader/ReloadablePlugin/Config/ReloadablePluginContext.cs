﻿extern alias KSP;
extern alias Cecil96;
using System;
using System.IO;
using System.Reflection;
using AssemblyReloader.Config;
using AssemblyReloader.Config.Keys;
using AssemblyReloader.Game;
using AssemblyReloader.Gui;
using AssemblyReloader.ReloadablePlugin.Gui;
using AssemblyReloader.ReloadablePlugin.Loaders;
using AssemblyReloader.ReloadablePlugin.Loaders.Addons;
using AssemblyReloader.ReloadablePlugin.Loaders.PartModules;
using AssemblyReloader.ReloadablePlugin.Loaders.ScenarioModules;
using AssemblyReloader.ReloadablePlugin.Loaders.VesselModules;
using AssemblyReloader.ReloadablePlugin.Weaving;
using AssemblyReloader.ReloadablePlugin.Weaving.Operations;
using AssemblyReloader.ReloadablePlugin.Weaving.Operations.GameEventInterception;
using AssemblyReloader.ReloadablePlugin.Weaving.Operations.Keys;
using ReeperAssemblyLibrary;
using ReeperCommon.FileSystem;
using ReeperCommon.Logging;
using strange.extensions.context.api;
using UnityEngine;

namespace AssemblyReloader.ReloadablePlugin.Config
{
    public class ReloadablePluginContext : SignalContext
    {
        private readonly IFile _reloadableFile;

        public IReloadablePlugin Plugin { get; private set; }
        public IPluginInfo Info { get; private set; }


        public ReloadablePluginContext(MonoBehaviour view, IFile reloadableFile) : base(view, ContextStartupFlags.MANUAL_MAPPING | ContextStartupFlags.MANUAL_LAUNCH)
        {
            if (reloadableFile == null) throw new ArgumentNullException("reloadableFile");

            _reloadableFile = reloadableFile;
        }


        protected override void mapBindings()
        {
            base.mapBindings();

            injectionBinder.Bind<IFile>().To(_reloadableFile);
            injectionBinder.Bind<IDirectory>().To(injectionBinder.GetInstance<IFile>().Directory);

            injectionBinder.Bind<ReeperAssembly>()
                .To(injectionBinder.GetInstance<IReeperAssemblyFactory>().Create(_reloadableFile.UrlFile.file));

            SetupNamedLogs();


            injectionBinder.Bind<IReloadablePlugin>().Bind<IPluginInfo>().To<ReloadablePlugin>().ToSingleton();
            injectionBinder.Bind<IMonoBehaviourDestroyer>().To<MonoBehaviourDestroyer>().ToSingleton();

            injectionBinder
                .Bind<IAddonSettings>()
                .Bind<IPartModuleSettings>()
                .Bind<IScenarioModuleSettings>()
                .Bind<IVesselModuleSettings>()
                .Bind<IWeaverSettings>()
                .Bind<PluginConfiguration>()
                .To<PluginConfiguration>().ToSingleton();

            injectionBinder.Bind<IReloadableAddonLoader>().To<ReloadableAddonLoader>().ToSingleton();
            injectionBinder.Bind<IReloadableAddonUnloader>().To<ReloadableAddonUnloader>().ToSingleton();


            injectionBinder.Bind<IPartModuleConfigNodeSnapshotRepository>().To<PartModuleConfigNodeSnapshotRepository>().ToSingleton();
            injectionBinder.Bind<IPartModuleFactory>().To<PartModuleFactory>().ToSingleton();
            injectionBinder.Bind<IPartModuleDescriptorFactory>().To<PartModuleDescriptorFactory>().ToSingleton();
            injectionBinder.Bind<IPartModuleLoader>().To<PartModuleLoader>().ToSingleton();
            injectionBinder.Bind<IPartModuleUnloader>().To<PartModuleUnloader>().ToSingleton();
            injectionBinder.Bind<IPartModuleDestroyer>().To<PartModuleDestroyer>().ToSingleton();

            injectionBinder.Bind<MethodInfo>()
                .To(typeof(Assembly).GetProperty("CodeBase", BindingFlags.Public | BindingFlags.Instance).GetGetMethod())
                .ToName(MethodKeys.AssemblyCodeBase);

            injectionBinder.Bind<IGetInstructionsInMethod>()
                .To(new GetMethodCallsInMethod(injectionBinder.GetInstance<MethodInfo>(MethodKeys.AssemblyCodeBase),
                    Cecil96::Mono.Cecil.Cil.OpCodes.Callvirt))
                .ToName(MethodKeys.AssemblyCodeBase);

            injectionBinder.Bind<MethodInfo>()
                .To(typeof (Assembly).GetProperty("Location", BindingFlags.Public | BindingFlags.Instance).GetGetMethod())
                .ToName(MethodKeys.AssemblyLocation);

            injectionBinder.Bind<IGetInstructionsInMethod>()
                .To(new GetMethodCallsInMethod(injectionBinder.GetInstance<MethodInfo>(MethodKeys.AssemblyLocation),
                    Cecil96::Mono.Cecil.Cil.OpCodes.Callvirt))
                .ToName(MethodKeys.AssemblyLocation);

            injectionBinder.Bind<MethodInfo>()
                .To(typeof(KSP::ScenarioRunner).GetMethod("GetLoadedModules", BindingFlags.Public | BindingFlags.Static))
                .ToName(MethodKeys.ScenarioRunnerGetLoadedModules);

            injectionBinder.Bind<IGetInstructionsInMethod>()
                .To(new GetMethodCallsInMethod(injectionBinder.GetInstance<MethodInfo>(MethodKeys.ScenarioRunnerGetLoadedModules),
                    Cecil96::Mono.Cecil.Cil.OpCodes.Call))
                .ToName(MethodKeys.ScenarioRunnerGetLoadedModules);



            injectionBinder.Bind<IGetTypeDefinitions>().To<GetTypeDefinitionsInAssemblyDefinitionExcludingHelper>().ToSingleton();
            injectionBinder.Bind<IGameEventRegistry>().To<GameEventRegistry>().ToSingleton();

            injectionBinder.Bind<AssemblyLocation>().ToSingleton();
            injectionBinder.Bind<AssemblyCodeBase>().ToSingleton();

            injectionBinder.Bind<SignalWeaveDefinition>().ToSingleton();
            injectionBinder.Bind<SignalPluginWasLoaded>().ToSingleton();
            injectionBinder.Bind<SignalPluginWasUnloaded>().ToSingleton();
            injectionBinder.Bind<SignalAboutToDestroyMonoBehaviour>().ToSingleton();
            injectionBinder.Bind<SignalLoadersFinished>().ToSingleton();
            injectionBinder.Bind<SignalPartModuleCreated>().ToSingleton();
            injectionBinder.Bind<SignalPluginCannotBeLoaded>().ToSingleton();
            injectionBinder.Bind<SignalErrorWhileUnloading>().ToSingleton();

            SetupCommandBindings();
            SetupReeperAssemblyLoader();

            mediationBinder.BindView<PluginConfigurationView>().ToMediator<PluginConfigurationViewMediator>();

            Plugin = injectionBinder.GetInstance<IReloadablePlugin>();
            Info = injectionBinder.GetInstance<IPluginInfo>();
        }


        public override void Launch()
        {
            injectionBinder.GetInstance<ILog>().Normal(Plugin.Name + " context launching");
            base.Launch();
            injectionBinder.GetInstance<SignalStart>().Dispatch();
        }


        private void SetupCommandBindings()
        {
            // only happens once: initial load of reloadable plugin
            commandBinder.Bind<SignalStart>()
                .To<CommandConfigurePluginGui>()
                .To<CommandStartReloadablePlugin>()
                .Once();


            // kicks off reload process
            commandBinder.Bind<SignalReloadPlugin>()
                .InSequence()
                .To<CommandUnloadPreviousPlugin>()
                .To<CommandLoadReloadablePlugin>();


            // begin rewriting il
            commandBinder.Bind<SignalWeaveDefinition>()
                .InSequence()
                .To<CommandCheckForUnsupportedTypes>()
                .To<CommandChangeDefinitionIdentity>()
                .To<CommandInsertHelperType>()
                .To<CommandReplaceKSPAddonWithReloadableAddon>()
                .To<CommandRewriteGameEventCalls>();


            // these things need the helper type
            commandBinder.Bind<SignalHelperDefinitionCreated>()
                .To<CommandRewriteAssemblyCodeBaseCalls>()
                .To<CommandRewriteAssemblyLocationCalls>();


            // other signals
            commandBinder.Bind<SignalPluginWasLoaded>()
                .To<CommandSetupGameEventProxyRegistryEntry>()
                .To<CommandInitializeAddonLoader>()
                .To<CommandLoadPartModules>()
                .To<CommandLoadVesselModules>()
                .To<CommandLoadScenarioModules>()
                .To<CommandDispatchLoadersFinished>();


            commandBinder.Bind<SignalUnloadPlugin>()
                .To<CommandDeinitializeAddonLoader>()
                .To<CommandUnloadVesselModules>()
                .To<CommandUnloadPartModules>()
                .To<CommandUnloadScenarioModules>()
                .To<CommandClearGameEventProxyRegistryEntry>(); // waits for SignalPluginWasUnloaded and checks for dangling GameEvent callbacks


            commandBinder.Bind<SignalAboutToDestroyMonoBehaviour>()
                .To<CommandCreatePartModuleConfigNodeSnapshot>()
                .To<CommandCreateScenarioModuleConfigNode>()
                .To<CommandSendReloadRequestedMessageToTarget>();


            commandBinder.Bind<SignalPluginCannotBeLoaded>()
                .To<CommandDisplayFailureMessage>();

            commandBinder.Bind<SignalErrorWhileUnloading>()
                .To<CommandDisplayFailureMessage>();

            // GameEvent signals
            commandBinder.Bind<SignalOnLevelWasLoaded>()
                .To<CommandCreateAddonsForScene>();
        }


        private void SetupNamedLogs()
        {
            injectionBinder.Bind<ILog>()
                .To(injectionBinder.GetInstance<ILog>().CreateTag(injectionBinder.GetInstance<IFile>().Name));

            injectionBinder.Bind<ILog>()
                .To(injectionBinder.GetInstance<ILog>().CreateTag("AddonLoader"))
                .ToName(LogKey.AddonLoader);

            injectionBinder.Bind<ILog>()
                .To(injectionBinder.GetInstance<ILog>().CreateTag("AddonUnloader"))
                .ToName(LogKey.AddonUnloader);



            injectionBinder.Bind<ILog>()
                .To(injectionBinder.GetInstance<ILog>().CreateTag("PartModuleLoader"))
                .ToName(LogKey.PartModuleLoader);

            injectionBinder.Bind<ILog>()
                .To(injectionBinder.GetInstance<ILog>().CreateTag("PartModuleFactory"))
                .ToName(LogKey.PartModuleFactory);

            injectionBinder.Bind<ILog>()
                .To(injectionBinder.GetInstance<ILog>().CreateTag("PartModuleUnloader"))
                .ToName(LogKey.PartModuleUnloader);

            injectionBinder.Bind<ILog>()
                .To(injectionBinder.GetInstance<ILog>().CreateTag("PartModuleSnapshotGenerator"))
                .ToName(LogKey.PartModuleSnapshotGenerator);


            injectionBinder.Bind<ILog>()
                .To(injectionBinder.GetInstance<ILog>().CreateTag("ScenarioModuleLoader"))
                .ToName(LogKey.ScenarioModuleLoader);

            injectionBinder.Bind<ILog>()
                .To(injectionBinder.GetInstance<ILog>().CreateTag("ScenarioModuleFactory"))
                .ToName(LogKey.ScenarioModuleFactory);

            injectionBinder.Bind<ILog>()
                .To(injectionBinder.GetInstance<ILog>().CreateTag("ScenarioModuleUnloader"))
                .ToName(LogKey.ScenarioModuleUnloader);

            injectionBinder.Bind<ILog>()
                .To(injectionBinder.GetInstance<ILog>().CreateTag("ScenarioModuleDestroyer"))
                .ToName(LogKey.ScenarioModuleDestroyer);

            injectionBinder.Bind<ILog>()
                .To(injectionBinder.GetInstance<ILog>().CreateTag("ScenarioModuleConfigNodeUpdater"))
                .ToName(LogKey.ScenarioModuleConfigNodeUpdater);
        }

        private void SetupReeperAssemblyLoader()
        {
            injectionBinder.Bind<ITemporaryFileFactory>().To<TemporaryFileFactory>();
            injectionBinder.Bind<IRawAssemblyDataFactory>().To<RawAssemblyDataFactory>().ToSingleton();
            injectionBinder.Bind<IRawAssemblyDataFactory>()
                .To<WovenRawAssemblyDataFactory>()
                .ToName(RawAssemblyDataKey.Woven);

            injectionBinder.Bind<WriteRawAssemblyDataToDisk>().ToSingleton();

            var writePatched =
                new WriteRawAssemblyDataToDisk(
                    injectionBinder.GetInstance<IRawAssemblyDataFactory>(RawAssemblyDataKey.Woven),
                    injectionBinder.GetInstance<IWeaverSettings>(),
                    ra => Path.GetFileName(ra.File.fullPath) + ".patched",
                    injectionBinder.GetInstance<ILog>());
            
            injectionBinder.Unbind<IRawAssemblyDataFactory>(RawAssemblyDataKey.Woven);
            injectionBinder.Unbind<WriteRawAssemblyDataToDisk>();

            injectionBinder.Bind<IRawAssemblyDataFactory>()
                .To(writePatched)
                .ToName(RawAssemblyDataKey.Woven);


            injectionBinder.Bind<ReloadableReeperAssemblyLoader>().ToSingleton();
            var loader = injectionBinder.GetInstance<ReloadableReeperAssemblyLoader>();

            AppDomain.CurrentDomain.AssemblyResolve += loader.Resolve;

            injectionBinder.Unbind<ReloadableReeperAssemblyLoader>();
            injectionBinder
                .Bind<IReeperAssemblyLoader>()
                .Bind<IReeperAssemblyUnloader>()
                .To(loader);
        }
    }
}
