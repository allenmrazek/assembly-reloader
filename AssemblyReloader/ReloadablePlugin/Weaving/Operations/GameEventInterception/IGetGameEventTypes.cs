using System;
using System.Collections.Generic;

namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations.GameEventInterception
{
    public interface IGetGameEventTypes
    {
        IEnumerable<Type> Get(int genericParamCount);
    }
}
