using UnityEngine;

namespace AssemblyReloader.ReloadablePlugin.Loaders
{
    public interface IUnityObjectDestroyer 
    {
        void Destroy(MonoBehaviour target);
    }
}
