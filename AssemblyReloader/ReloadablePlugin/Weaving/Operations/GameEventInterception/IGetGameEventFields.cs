using System.Collections.Generic;
using System.Reflection;

namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations.GameEventInterception
{
    public interface IGetGameEventFields
    {
        IEnumerable<FieldInfo> Get();
    }
}
