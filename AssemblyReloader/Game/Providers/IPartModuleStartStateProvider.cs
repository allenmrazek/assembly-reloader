using ReeperCommon.Containers;

namespace AssemblyReloader.Game.Providers
{
    public interface IPartModuleStartStateProvider
    {
        PartModule.StartState Get(Maybe<IVessel> vessel);
    }
}
