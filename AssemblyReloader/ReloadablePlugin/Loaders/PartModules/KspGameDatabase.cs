using System.Collections.Generic;
using strange.extensions.injector.api;

namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules
{
    [Implements(typeof(IGameDatabase), InjectionBindingScope.CROSS_CONTEXT)]
// ReSharper disable once UnusedMember.Global
    public class KspGameDatabase : IGameDatabase
    {
        public IEnumerable<UrlDir.UrlConfig> GetConfigs(string typeName)
        {
            return GameDatabase.Instance.GetConfigs(typeName);
        }
    }
}
