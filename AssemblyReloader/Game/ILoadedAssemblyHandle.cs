extern alias KSP;

namespace AssemblyReloader.Game
{
    public interface ILoadedAssemblyHandle
    {
        KSP::AssemblyLoader.LoadedAssembly LoadedAssembly { get; }
    }
}
