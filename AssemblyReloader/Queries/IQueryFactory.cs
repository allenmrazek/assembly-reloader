using System.Reflection;

namespace AssemblyReloader.Queries
{
    public interface IQueryFactory
    {
        IAddonsFromAssemblyQuery GetAddonsFromAssemblyQuery(Assembly assembly);
        IAddonAttributeFromTypeQuery GetAddonAttributeQuery();
        IStartupSceneFromGameSceneQuery GetStartupSceneFromGameSceneQuery();
        ICurrentGameSceneQuery GetCurrentGameSceneProvider();
    }
}
