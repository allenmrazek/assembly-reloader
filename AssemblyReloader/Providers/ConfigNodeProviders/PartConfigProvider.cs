using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReeperCommon.Containers;

namespace AssemblyReloader.Providers.ConfigNodeProviders
{
    public class PartConfigProvider : IPartConfigProvider
    {
        public Maybe<ConfigNode> Get(AvailablePart availablePart)
        {
            var found = GameDatabase.Instance.GetConfigs("PART")
                .FirstOrDefault(u => u.name.Replace('_', '.') == availablePart.name);

            return found == null ? Maybe<ConfigNode>.None : Maybe<ConfigNode>.With(found.config);
        }
    }
}
