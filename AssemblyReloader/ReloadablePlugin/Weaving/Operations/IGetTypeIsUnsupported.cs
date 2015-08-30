extern alias Cecil96;
using TypeDefinition = Cecil96::Mono.Cecil.TypeDefinition;

namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations
{
    public interface IGetTypeIsUnsupported
    {
        bool Get(TypeDefinition typeDefinition);
    }
}
