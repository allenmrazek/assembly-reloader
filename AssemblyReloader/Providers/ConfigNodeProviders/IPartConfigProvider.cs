using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReeperCommon.Containers;

namespace AssemblyReloader.Providers.ConfigNodeProviders
{
    public interface IPartConfigProvider
    {
        Maybe<ConfigNode> Get(AvailablePart availablePart);
    }
}
