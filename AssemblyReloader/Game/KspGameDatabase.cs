using System.Collections.Generic;

namespace AssemblyReloader.Game
{
    public class KspGameDatabase : IGameDatabase
    {
        public IEnumerable<UrlDir.UrlConfig> GetConfigs(string typeName)
        {
            return GameDatabase.Instance.GetConfigs(typeName);
        }
    }
}
