using UnityEngine;

namespace AssemblyReloader.ReloadablePlugin.Loaders
{
    public interface IMonoBehaviourDestroyer 
    {
        void DestroyMonoBehaviour(MonoBehaviour target);

    }
}
