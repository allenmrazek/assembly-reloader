using AssemblyReloader.Game;
using AssemblyReloader.ReloadablePlugin.Loaders.ScenarioModules;

namespace AssemblyReloader.ReloadablePlugin.Loaders
{
    public interface IKspFactory
    {
        IPart Create(Part part);
        IAvailablePart Create(AvailablePart part);
        IVessel Create(Vessel vessel);
        IProtoScenarioModule Create(ProtoScenarioModule psm);
        IGame Create(global::Game game);
    }
}
