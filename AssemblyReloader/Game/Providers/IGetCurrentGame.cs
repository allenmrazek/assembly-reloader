using ReeperCommon.Containers;

namespace AssemblyReloader.Game.Providers
{
    public interface IGetCurrentGame
    {
        Maybe<IGame> Get();
    }
}
