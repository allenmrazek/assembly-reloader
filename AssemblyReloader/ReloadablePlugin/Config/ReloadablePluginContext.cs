using System;
using System.Reflection;
using AssemblyReloader.Config.Keys;
using AssemblyReloader.FileSystem;
using AssemblyReloader.Game;
using AssemblyReloader.Gui;
using AssemblyReloader.ReloadablePlugin.Gui;
using AssemblyReloader.ReloadablePlugin.Loaders;
using AssemblyReloader.ReloadablePlugin.Loaders.Addons;
using AssemblyReloader.ReloadablePlugin.Loaders.PartModules;
using AssemblyReloader.ReloadablePlugin.Weaving;
using AssemblyReloader.ReloadablePlugin.Weaving.Operations;
using AssemblyReloader.ReloadablePlugin.Weaving.Operations.Keys;
using AssemblyReloader.StrangeIoC.extensions.context.api;
using Mono.Cecil.Cil;
using ReeperCommon.FileSystem;
using ReeperCommon.Logging;
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

            injectionBinder.Bind<ILog>()
                .To(injectionBinder.GetInstance<ILog>().CreateTag(injectionBinder.GetInstance<IFile>().Name));

            injectionBinder.Bind<ILog>()
                .To(injectionBinder.GetInstance<ILog>().CreateTag("AddonLoader"))
                .ToName(LogKeys.AddonLoader);

            injectionBinder.Bind<ILog>()
                .To(injectionBinder.GetInstance<ILog>().CreateTag("AddonUnloader"))
                .ToName(LogKeys.AddonUnloader);

            injectionBinder.Bind<ILog>()
                .To(injectionBinder.GetInstance<ILog>().CreateTag("PartModuleLoader"))
                .ToName(LogKeys.PartModuleLoader);

            injectionBinder.Bind<ILog>()
                .To(injectionBinder.GetInstance<ILog>().CreateTag("PartModuleFactory"))
                .ToName(LogKeys.PartModuleFactory);

            injectionBinder.Bind<ILog>()
                .To(injectionBinder.GetInstance<ILog>().CreateTag("PartModuleUnloader"))
                .ToName(LogKeys.PartModuleUnloader);


            injectionBinder.Bind<IGetTemporaryFile>().To<GetTemporaryFile>().ToSingleton();
            injectionBinder.Bind<IReloadablePlugin>().Bind<IPluginInfo>().To<ReloadablePlugin>().ToSingleton();
            injectionBinder.Bind<IGetDebugSymbolsExistForDefinition>()
                .To<GetDebugSymbolsExistForDefinition>()
                .ToSingleton();

            injectionBinder.Bind<AssemblyDefinitionLoader>().ToSingleton();
            injectionBinder.Bind<SignalWeaveDefinition>().ToSingleton();

            injectionBinder.Bind<IAssemblyDefinitionReader>().To(
                new AssemblyDefinitionWeaver(
                    injectionBinder.GetInstance<AssemblyDefinitionReader>(),
                    injectionBinder.GetInstance<SignalWeaveDefinition>(),
                    injectionBinder.GetInstance<ILog>()));


            injectionBinder.Bind<IAssemblyDefinitionLoader>()
                .To(new WriteModifiedAssemblyDefinitionToDisk(
                    injectionBinder.GetInstance<AssemblyDefinitionLoader>(), injectionBinder.GetInstance<IDirectory>(),
                    () => true, injectionBinder.GetInstance<ILog>()));

            injectionBinder.Bind<IMonoBehaviourDestroyer>().To<MonoBehaviourDestroyer>().ToSingleton();

            injectionBinder
                .Bind<IAddonSettings>()
                .Bind<IPartModuleSettings>()
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
                .ToName(InterceptedMethods.CodeBase);

            injectionBinder.Bind<MethodInfo>()
                .To(typeof (Assembly).GetProperty("Location", BindingFlags.Public | BindingFlags.Instance).GetGetMethod())
                .ToName(InterceptedMethods.Location);

            injectionBinder.Bind<IGetInstructionsInMethod>()
                .To(new GetMethodCallsInMethod(injectionBinder.GetInstance<MethodInfo>(InterceptedMethods.CodeBase),
                    OpCodes.Callvirt))
                .ToName(InterceptedMethods.CodeBase);

            injectionBinder.Bind<IGetInstructionsInMethod>()
                .To(new GetMethodCallsInMethod(injectionBinder.GetInstance<MethodInfo>(InterceptedMethods.Location),
                    OpCodes.Callvirt))
                .ToName(InterceptedMethods.Location);

            injectionBinder.Bind<IGetTypeDefinitions>().To(new GetTypeDefinitionsInAssemblyDefinitionExcludingHelper()).ToSingleton();

            injectionBinder.Bind<AssemblyLocation>().ToSingleton();
            injectionBinder.Bind<AssemblyCodeBase>().ToSingleton();


            injectionBinder.Bind<SignalPluginWasLoaded>().ToSingleton();
            injectionBinder.Bind<SignalPluginWasUnloaded>().ToSingleton();
            injectionBinder.Bind<SignalAboutToDestroyMonoBehaviour>().ToSingleton();
            injectionBinder.Bind<SignalLoadersFinished>().ToSingleton();
            injectionBinder.Bind<SignalPartModuleCreated>().ToSingleton();

            SetupCommandBindings();


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
                .To<CommandConfigurePluginGUI>()
                .To<CommandStartReloadablePlugin>()
                .Once();

            // kicks off reload process
            commandBinder.Bind<SignalReloadPlugin>()
                .To<CommandReloadPlugin>();

            // begin rewriting il
            commandBinder.Bind<SignalWeaveDefinition>()
                .To<CommandChangeDefinitionIdentity>()
                .To<CommandInsertHelperType>()
                .To<CommandReplaceKSPAddonWithReloadableAddon>();

            // these things need the helper type
            commandBinder.Bind<SignalHelperDefinitionCreated>()
                .To<CommandRewriteAssemblyCodeBaseCalls>()
                .To<CommandRewriteAssemblyLocationCalls>();


            // other signals
            commandBinder.Bind<SignalPluginWasLoaded>()
                .To<CommandInitializeAddonLoader>()
                .To<CommandCreatePartModules>()
                .To<CommandDispatchLoadersFinished>();


            commandBinder.Bind<SignalUnloadPlugin>()
                .InSequence()
                .To<CommandDeinitializeAddonLoader>()
                .To<CommandUnloadPartModules>();


            commandBinder.Bind<SignalAboutToDestroyMonoBehaviour>()
                .To<CommandCreatePartModuleConfigNodeSnapshot>()
                .To<CommandSendReloadRequestedMessageToTarget>();


            // GameEvent signals
            commandBinder.Bind<SignalOnLevelWasLoaded>()
                .To<CommandCreateAddonsForScene>();
        }
    }
}
