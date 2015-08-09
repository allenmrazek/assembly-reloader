using AssemblyReloader.DataObjects;
using AssemblyReloader.StrangeIoC.extensions.implicitBind;

namespace AssemblyReloader.ReloadablePlugin.Loaders.ScenarioModules
{
// ReSharper disable once UnusedMember.Global
    [Implements(typeof(IScenarioModuleConfigNodeRepository))]
    public class ScenarioModuleConfigNodeRepository : DictionaryQueue<TypeIdentifier, ConfigNode>, IScenarioModuleConfigNodeRepository
    {
        public ScenarioModuleConfigNodeRepository() : base(new TypeIdentifierComparer())
        {
            
        }
    }
}
