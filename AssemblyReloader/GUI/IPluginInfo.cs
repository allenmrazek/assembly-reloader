using ReeperKSP.FileSystem;

namespace AssemblyReloader.Gui
{
    public interface IPluginInfo
    {
        string Name { get; }
        IFile Location { get; }
    }
}
