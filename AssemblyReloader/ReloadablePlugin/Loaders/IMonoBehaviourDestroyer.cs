using UnityEngine;

namespace AssemblyReloader.ReloadablePlugin.Loaders
{
    public interface IMonoBehaviourDestroyer 
    {
        void Destroy(MonoBehaviour target);
    }
}
