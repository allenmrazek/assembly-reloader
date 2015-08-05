namespace AssemblyReloader.Game
{
    public interface IPartLoaderPrefabProvider
    {
        IPart GetPrefab(IPart from);
    }
}
