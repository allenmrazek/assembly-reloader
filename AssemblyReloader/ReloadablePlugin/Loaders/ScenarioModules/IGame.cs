using System.Collections.ObjectModel;

namespace AssemblyReloader.ReloadablePlugin.Loaders.ScenarioModules
{
    public interface IGame
    {
        ReadOnlyCollection<IProtoScenarioModule> Scenarios { get; }
    }
}
