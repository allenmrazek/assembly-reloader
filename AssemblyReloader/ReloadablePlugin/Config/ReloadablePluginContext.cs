﻿using System;
using AssemblyReloader.Common;
using AssemblyReloader.Config.Keys;
using AssemblyReloader.FileSystem;
using AssemblyReloader.Gui;
using AssemblyReloader.ReloadablePlugin.Weaving;
using AssemblyReloader.StrangeIoC.extensions.context.api;
using Mono.Cecil;
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
                .To(injectionBinder.GetInstance<ILog>().CreateTag(injectionBinder.GetInstance<IFile>().Name))
                .ToName(LogKeys.PluginContext);


            injectionBinder.Bind<IGetTemporaryFile>().To<GetTemporaryFile>().ToSingleton();
            injectionBinder.Bind<IReloadablePlugin>().Bind<IPluginInfo>().To<ReloadablePlugin>().ToSingleton();
            injectionBinder.Bind<IGetDebugSymbolsExistForDefinition>()
                .To<GetDebugSymbolsExistForDefinition>()
                .ToSingleton();

            injectionBinder.Bind<AssemblyDefinitionLoader>().ToSingleton();
            injectionBinder.Bind<SignalDefinitionReady>().ToSingleton();

            injectionBinder.Bind<IAssemblyDefinitionReader>().To(
                new AssemblyDefinitionWeaver(
                    injectionBinder.GetInstance<AssemblyDefinitionReader>(),
                    injectionBinder.GetInstance<SignalDefinitionReady>(),
                    injectionBinder.GetInstance<ILog>(LogKeys.PluginContext)));


            injectionBinder.Bind<IAssemblyDefinitionLoader>()
                .To(new WriteModifiedAssemblyDefinitionToDisk(
                    injectionBinder.GetInstance<AssemblyDefinitionLoader>(), injectionBinder.GetInstance<IDirectory>(),
                    () => true));

            injectionBinder.Bind<SignalPluginWasLoaded>().ToSingleton();
            injectionBinder.Bind<SignalPluginWasUnloaded>().ToSingleton();

            // only happens once: initial load of reloadable plugin
            commandBinder.Bind<SignalStart>()
                .To<CommandStartReloadablePlugin>().Once();


            commandBinder.Bind<SignalReloadPlugin>().To<CommandReloadPlugin>();
            commandBinder.Bind<SignalInstallPluginTypes>().To<CommandNull>();
            commandBinder.Bind<SignalUninstallPluginTypes>().To<CommandNull>();

            commandBinder.Bind<SignalDefinitionReady>()
                .InSequence()
                .To<CommandChangeDefinitionIdentity>();
                //to intercept Assembly.Location
                //to intercept Assembly.CodeBase
                //to intercept calls to LoadedAssembly

           

            Plugin = injectionBinder.GetInstance<IReloadablePlugin>();
            Info = injectionBinder.GetInstance<IPluginInfo>();
        }


        public override void Launch()
        {
            injectionBinder.GetInstance<ILog>().Normal(Plugin.Name + " context launching");
            base.Launch();
            injectionBinder.GetInstance<SignalStart>().Dispatch();
        }
    }
}
