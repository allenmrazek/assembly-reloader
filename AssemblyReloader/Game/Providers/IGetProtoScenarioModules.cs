using System;
using System.Collections.Generic;

namespace AssemblyReloader.Game.Providers
{
    public interface IGetProtoScenarioModules
    {
        IEnumerable<IProtoScenarioModule> Get(Type type);
    }
}
