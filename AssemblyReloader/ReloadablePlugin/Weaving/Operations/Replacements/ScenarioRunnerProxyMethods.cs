using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations.Replacements
{
    public static class ScenarioRunnerProxyMethods
    {
        public static List<ScenarioModule> Proxy_GetLoadedModules()
        {
            return ScenarioRunner.fetch.gameObject.GetComponents<ScenarioModule>().ToList();
        }
    }
}
