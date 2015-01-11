using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using AssemblyReloader.AddonTracking;
using AssemblyReloader.Loaders;
using AssemblyReloader.Loaders.Implementations;
using AssemblyReloader.Messages;
using AssemblyReloader.Providers;
using AssemblyReloader.Queries;
using ReeperCommon.Logging;

namespace AssemblyReloader.Factory
{
    class LoaderFactory
    {
        private readonly IChannel _channel;
        private readonly CommandFactory _commandFactory;
        private readonly Log _log;
        private readonly CurrentStartupSceneProvider _currentScene;


        public LoaderFactory(
            IChannel channel,
            CommandFactory commandFactory,
            Log log,
            CurrentStartupSceneProvider currentScene)
        {
            if (channel == null) throw new ArgumentNullException("channel");
            if (commandFactory == null) throw new ArgumentNullException("commandFactory");
            if (log == null) throw new ArgumentNullException("log");
            if (currentScene == null) throw new ArgumentNullException("currentScene");

            _channel = channel;
            _commandFactory = commandFactory;
            _log = log;
            _currentScene = currentScene;
        }



        public List<ILoader> CreateLoaders(ReloadableAssembly assembly, AddonInfoFactory infoFactory,
            AddonsFromAssemblyQuery assemblyQuery)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");
            if (infoFactory == null) throw new ArgumentNullException("infoFactory");

            var addonLoader = new Loaders.Implementations.AddonLoader(
                _channel,
                assembly,
                infoFactory,
                _commandFactory,
                assemblyQuery,
                _currentScene,
                _log);


            addonLoader.Initialize();



            //loaders.ForEach(l => l.Initialize());

            return new List<ILoader>
            {
                addonLoader
            };
        }
    }
}
