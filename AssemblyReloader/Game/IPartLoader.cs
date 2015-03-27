using System.Collections.Generic;

namespace AssemblyReloader.Game
{
    public interface IPartLoader
    {
        List<IAvailablePart> LoadedParts { get; }
    }
}
