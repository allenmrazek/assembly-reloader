using ReeperCommon.FileSystem;

namespace AssemblyReloader.Config
{
    public interface IPluginInfo
    {
        string Name { get; }
        IFile Location { get; }
    }
}
