using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
