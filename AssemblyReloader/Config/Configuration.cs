using System;

namespace AssemblyReloader.Config
{
    public class Configuration : IConfiguration
    {
        [Persistent]
        private bool _startAddonsForCurrentScene = true;

        [Persistent]
        private bool _reloadPartModulesImmediately = true;

        [Persistent]
        private bool _ignoreCurrentSceneForInstantAddons = true;

        [Persistent]
        private bool _rewriteAssemblyLocationCalls = true;


        public Configuration(ConfigNode config)
        {
            if (config == null) throw new ArgumentNullException("config");
        }


        public bool StartAddonsForCurrentScene
        {
            get { return _startAddonsForCurrentScene; }
        }

        public bool ReloadPartModulesImmediately
        {
            get { return _reloadPartModulesImmediately; }
        }

        public bool IgnoreCurrentSceneForInstantAddons
        {
            get { return _ignoreCurrentSceneForInstantAddons; }
        }

        public bool RewriteAssemblyLocationCalls
        {
            get { return _rewriteAssemblyLocationCalls; }
        }
    }
}
