using System.Collections.ObjectModel;
using AssemblyReloader.ReloadablePlugin.Loaders;

namespace AssemblyReloader.Game
{
    public interface IPartLoader
    {
        ReadOnlyCollection<IAvailablePart> LoadedParts { get; }
    }
}
