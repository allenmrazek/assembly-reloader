using AssemblyReloader.Game;

namespace AssemblyReloader.Queries
{
    public interface IPartIsPrefabQuery
    {
        bool Get(IPart part);
    }
}
