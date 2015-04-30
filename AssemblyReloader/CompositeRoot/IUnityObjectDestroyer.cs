namespace AssemblyReloader.CompositeRoot
{
    public interface IUnityObjectDestroyer
    {
        void Destroy<T>(T target) where T : UnityEngine.Object;
    }
}
