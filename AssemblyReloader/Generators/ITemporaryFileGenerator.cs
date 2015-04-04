using AssemblyReloader.DataObjects;

namespace AssemblyReloader.Generators
{
    public interface ITemporaryFileGenerator
    {
        TemporaryFile Get();
    }
}
