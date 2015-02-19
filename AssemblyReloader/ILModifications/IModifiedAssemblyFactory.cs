using ReeperCommon.FileSystem;

namespace AssemblyReloader.ILModifications
{
    public interface IModifiedAssemblyFactory
    {
        IModifiedAssembly Create(IFile location);
    }
}
