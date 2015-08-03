using System.Collections.Generic;
using AssemblyReloader.StrangeIoC.extensions.implicitBind;
using AssemblyReloader.StrangeIoC.extensions.injector.api;

namespace AssemblyReloader.Game
{
    [Implements(typeof(IGameDatabase), InjectionBindingScope.CROSS_CONTEXT)]
    public class KspGameDatabase : IGameDatabase
    {
        public IEnumerable<UrlDir.UrlConfig> GetConfigs(string typeName)
        {
            return GameDatabase.Instance.GetConfigs(typeName);
        }
    }
}
