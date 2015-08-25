using System.Reflection;
using ReeperCommon.Containers;

namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations.GameEventInterception
{
    public interface IGameEventReferenceFactory
    {
        GameEventReference Create(object actualRef);
    }
}
