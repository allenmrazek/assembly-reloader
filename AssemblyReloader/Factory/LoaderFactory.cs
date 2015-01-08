using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssemblyReloader.AddonTracking;
using AssemblyReloader.Loaders;
using AssemblyReloader.Providers;
using AssemblyReloader.Queries;
using ReeperCommon.Logging;

namespace AssemblyReloader.Factory
{
    class LoaderFactory
    {
        private readonly Log _log;
        private readonly KspCurrentStartupSceneProvider _currentScene;


        public LoaderFactory(Log log, KspCurrentStartupSceneProvider currentScene)
        {
            if (log == null) throw new ArgumentNullException("log");
            if (currentScene == null) throw new ArgumentNullException("currentScene");

            _log = log;
            _currentScene = currentScene;
        }



        public List<ILoader> CreateLoaders(ReloadableAssembly assembly, AddonInfoFactory factory, KspAddonsFromAssemblyQuery assemblyQuery)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");
            if (factory == null) throw new ArgumentNullException("factory");

            return new List<ILoader>
            {
                new KspAddonLoader(assembly, factory, assemblyQuery, _currentScene, _log)
            };
        }
    }
}
