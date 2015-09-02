using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssemblyReloader.ReloadablePlugin.Weaving;
using ReeperAssemblyLibrary;
using ReeperCommon.Logging;
using strange.extensions.command.impl;

namespace AssemblyReloader.ReloadablePlugin.Config
{
    public class CommandConfigureReeperLoader : Command
    {
        private readonly ILog _log;

        public CommandConfigureReeperLoader(ILog log)
        {
            if (log == null) throw new ArgumentNullException("log");
            _log = log;
        }


        public override void Execute()
        {
            //_log.Verbose("Configuring Reeper loader");


            //// dependency of WovenRawAssemblyFactory
            //var rawFactory = injectionBinder.GetInstance<RawAssemblyDataFactory>();

            //injectionBinder.Bind<IRawAssemblyDataFactory>().To(rawFactory);
            //injectionBinder.Bind<WovenRawAssemblyDataFactory>().ToSingleton();


            //var wovenAssemblyFactory = injectionBinder.GetInstance<WovenRawAssemblyDataFactory>();
            //injectionBinder.Unbind<WovenRawAssemblyDataFactory>();
            //injectionBinder.Bind<IRawAssemblyDataFactory>().To(wovenAssemblyFactory);

            //_log.Verbose("Reeper loader configured");
        }
    }
}
