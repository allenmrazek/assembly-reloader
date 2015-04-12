using AssemblyReloader.DataObjects;

namespace AssemblyReloader.Generators
{
    public interface ITemporaryFileFactory
    {
        TemporaryFile Get();
        TemporaryFile Get(string fullPath);
    }
}
