namespace AssemblyReloader.ReloadablePlugin.Loaders.ScenarioModules
{
// ReSharper disable once UnusedMember.Global
    [Implements(typeof(IScenarioModuleConfigNodeRepository))]
    public class ScenarioModuleConfigNodeRepository : DictionaryOfQueues<TypeIdentifier, ConfigNode>, IScenarioModuleConfigNodeRepository
    {
    }
}
