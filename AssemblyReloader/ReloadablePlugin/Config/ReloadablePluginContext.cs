using System;
using AssemblyReloader.Common;
using AssemblyReloader.Gui;
using AssemblyReloader.ReloadablePlugin.Loaders.Addons;
using AssemblyReloader.StrangeIoC.extensions.command.impl;
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

            if (injectionBinder.GetBinding<DefaultAssemblyResolver>() == null)
                injectionBinder.Bind<DefaultAssemblyResolver>().ToSingleton().CrossContext();

            injectionBinder.Bind<IFile>().To(_reloadableFile);

            injectionBinder.Bind<IReloadablePlugin>().Bind<IPluginInfo>().To<ReloadablePlugin>().ToSingleton();


            //commandBinder.Bind<SignalStart>().To<CommandLoadPluginAssembly>().Once();

            commandBinder.Bind<SignalLoadAssembly>().InSequence()
                .To<CommandNull>();
                //.To<CommandLoadAddons>();

            commandBinder.Bind<SignalUnloadAssembly>()
                .To<CommandNull>();

            commandBinder.Bind<SignalAssemblyWasLoaded>()
                .To<CommandNull>();

            commandBinder.Bind<SignalAssemblyWasUnloaded>()
                .To<CommandNull>();

            commandBinder.Bind<SignalStart>()
                .To<CommandTest>().Once();


            Plugin = injectionBinder.GetInstance<IReloadablePlugin>();
            Info = injectionBinder.GetInstance<IPluginInfo>();
        }


        public override void Launch()
        {
            injectionBinder.GetInstance<ILog>().Normal(Plugin.Name + " context launching");
            base.Launch();
        }
    }
}
