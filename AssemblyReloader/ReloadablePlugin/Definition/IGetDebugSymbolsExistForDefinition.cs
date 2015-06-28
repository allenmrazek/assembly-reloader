using ReeperCommon.FileSystem;

namespace AssemblyReloader.ReloadablePlugin.Definition
{
    public interface IGetDebugSymbolsExistForDefinition
    {
        bool Get(IFile location);
    }
}
