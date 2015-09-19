using ReeperAssemblyLibrary;

namespace AssemblyReloader.Config
{
    public interface IReeperAssemblyUnloader
    {
        void Unload(ILoadedAssemblyHandle handle);
    }
}
