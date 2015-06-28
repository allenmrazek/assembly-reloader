using AssemblyReloader.DataObjects;

namespace AssemblyReloader.FileSystem
{
    public interface ITemporaryFileFactory
    {
        TemporaryFile Get();
        TemporaryFile Get(string fullPath);
    }
}
