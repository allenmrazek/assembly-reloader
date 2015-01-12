using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using AssemblyReloader.AddonTracking;
using AssemblyReloader.Loaders;
using AssemblyReloader.Messages;
using AssemblyReloader.Providers;
using AssemblyReloader.Queries;
using ReeperCommon.Logging;

namespace AssemblyReloader.Factory
{
    class LoaderFactory
    {
        private readonly Log _log;
        private readonly CurrentStartupSceneProvider _currentScene;


        public LoaderFactory(
            Log log,
            CurrentStartupSceneProvider currentScene)
        {
            if (log == null) throw new ArgumentNullException("log");
            if (currentScene == null) throw new ArgumentNullException("currentScene");

            _log = log;
            _currentScene = currentScene;
        }



        public List<ILoader> CreateLoaders(
            ReloadableAssembly assembly,
            AddonsFromAssemblyQuery assemblyQuery)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");

            _log.Verbose("Creating loaders for assembly '{0}'", assembly.File.FileName);

            var addonLoader = new Loaders.Addon.AddonLoader(
                assembly,
                assemblyQuery,
                _currentScene,
                _log);


            addonLoader.Initialize();


            return new List<ILoader>
            {
                addonLoader
            };
        }
    }
}
