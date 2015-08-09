using System.Collections.ObjectModel;

namespace AssemblyReloader.Game
{
    public interface IPartLoader
    {
        ReadOnlyCollection<IAvailablePart> LoadedParts { get; }
    }
}
