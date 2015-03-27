using System.Reflection;

namespace AssemblyReloader.Destruction
{
    public interface IAddonDestroyer
    {
        void DestroyAddonsFrom(Assembly assembly);
    }
}
