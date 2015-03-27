using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyReloader.Providers.Game
{
    public interface IProtoScenarioModuleProvider
    {
        IEnumerable<ProtoScenarioModule> Get(Type type);
    }
}
