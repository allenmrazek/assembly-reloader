using AssemblyReloader.DataObjects;

namespace AssemblyReloader.FileSystem
{
    public interface IGetTemporaryFile
    {
        TemporaryFile Get();
        TemporaryFile Get(string fullPath);
    }
}
