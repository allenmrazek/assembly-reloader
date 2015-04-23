using ReeperCommon.Serialization;

namespace AssemblyReloader.Providers
{
    public interface IConfigNodeFormatterProvider
    {
        IConfigNodeFormatter Get();
    }
}
