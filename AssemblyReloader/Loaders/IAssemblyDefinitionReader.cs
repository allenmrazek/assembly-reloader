using System.IO;
using System.Reflection;
using Mono.Cecil;
using ReeperCommon.Containers;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.Loaders
{
    public interface IAssemblyDefinitionReader
    {
        Maybe<AssemblyDefinition> GetDefinition();
        void WriteToStream(AssemblyDefinition definition, Stream stream);
        Maybe<Assembly> Load(MemoryStream stream);
 
        string Name { get; }
        IFile Location { get; }
    }
}
