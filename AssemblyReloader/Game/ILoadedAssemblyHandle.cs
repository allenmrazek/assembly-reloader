namespace AssemblyReloader.Game
{
    public interface ILoadedAssemblyHandle
    {
        AssemblyLoader.LoadedAssembly LoadedAssembly { get; }
    }
}
