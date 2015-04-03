using System.Reflection;
using Mono.Cecil;
using ReeperCommon.Containers;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.Providers
{
    public interface IAssemblyProvider
    {
        Maybe<Assembly> Get();
        IFile Location { get; }
    }
}
