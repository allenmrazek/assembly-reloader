using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssemblyReloader.AddonTracking;
using AssemblyReloader.Loaders;
using AssemblyReloader.Loaders.Implementations;
using AssemblyReloader.Providers;
using AssemblyReloader.Queries;
using ReeperCommon.Logging;

namespace AssemblyReloader.Factory
{
    class LoaderProvider
    {
        private readonly CommandFactory _commandFactory;
        private readonly Log _log;
        private readonly CurrentStartupSceneProvider _currentScene;


        public LoaderProvider(
            CommandFactory commandFactory,
            Log log,
            CurrentStartupSceneProvider currentScene)
        {
            if (commandFactory == null) throw new ArgumentNullException("commandFactory");
            if (log == null) throw new ArgumentNullException("log");
            if (currentScene == null) throw new ArgumentNullException("currentScene");

            _commandFactory = commandFactory;
            _log = log;
            _currentScene = currentScene;
        }



        public List<ILoader> CreateLoaders(ReloadableAssembly assembly, AddonInfoFactory infoFactory, AddonsFromAssemblyQuery assemblyQuery)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");
            if (infoFactory == null) throw new ArgumentNullException("infoFactory");

            var loaders = new List<ILoader>
            {
                new Loaders.Implementations.AddonLoader(assembly, infoFactory, _commandFactory, assemblyQuery, _currentScene, _log)
            };

            loaders.ForEach(l => l.Initialize());

            return loaders;
        }
    }
}
