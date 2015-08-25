using System.Collections.Generic;

namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations.GameEventInterception
{
    public interface IGameEventRegistry : IEnumerable<GameEventCallback>
    {
        void Add(GameEventReference gameEvent, GameEventCallback callback);
        bool Remove(GameEventReference gameEvent, GameEventCallback callback);
        void ClearCallbacks();

        int Count { get; }
    }
}
