using System.Collections.Generic;
using System.Linq;

namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations.Proxies
{
    public static class ScenarioRunnerProxyMethods
    {
// ReSharper disable once UnusedMember.Global
        public static List<ScenarioModule> Proxy_GetLoadedModules()
        {
            if (ScenarioRunner.fetch == null)
                return new List<ScenarioModule>();

            return ScenarioRunner.fetch.gameObject.GetComponents<ScenarioModule>()
                .Where(sm => sm != null) // might be insanely defensive coding here but just in case the 
                                         // ScenarioModules happened to be destroyed this frame, use Unity's 
                                         // overload to avoid any
                .ToList();
        }
    }
}
