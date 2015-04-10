using ReeperCommon.Containers;

namespace AssemblyReloader.Game.Providers
{
    public interface ICurrentGameProvider
    {
        Maybe<IGame> Get();
    }
}
