extern alias KSP;
using AssemblyReloader.Game;
using AssemblyReloader.ReloadablePlugin.Loaders.ScenarioModules;

namespace AssemblyReloader.ReloadablePlugin.Loaders
{
    public interface IKspFactory
    {
        IPart Create(KSP::Part part);
        IAvailablePart Create(KSP::AvailablePart part);
        IVessel Create(KSP::Vessel vessel);
        IProtoScenarioModule Create(KSP::ProtoScenarioModule psm);
        IGame Create(KSP::Game game);
    }
}
