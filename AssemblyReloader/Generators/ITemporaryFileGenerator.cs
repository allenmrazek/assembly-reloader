using AssemblyReloader.DataObjects;

namespace AssemblyReloader.Generators
{
    public interface ITemporaryFileGenerator
    {
        TemporaryFile Get();
        TemporaryFile Get(string fullPath);
    }
}
