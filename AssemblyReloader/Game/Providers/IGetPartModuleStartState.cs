using ReeperCommon.Containers;

namespace AssemblyReloader.Game.Providers
{
    public interface IGetPartModuleStartState
    {
        PartModule.StartState Get(Maybe<IVessel> vessel);
    }
}
