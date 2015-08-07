using AssemblyReloader.ReloadablePlugin.Loaders.ScenarioModules;

namespace AssemblyReloader.Game
{
    public interface IKspFactory
    {
        IPart Create(Part part);
        IAvailablePart Create(AvailablePart part);
        IVessel Create(Vessel vessel);
        IProtoScenarioModule Create(ProtoScenarioModule psm);
        IScenarioRunner Create(ScenarioRunner runner);
    }
}
