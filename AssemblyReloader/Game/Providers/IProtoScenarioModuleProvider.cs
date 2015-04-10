using System;
using System.Collections.Generic;

namespace AssemblyReloader.Game.Providers
{
    public interface IProtoScenarioModuleProvider
    {
        IEnumerable<IProtoScenarioModule> Get(Type type);
    }
}
