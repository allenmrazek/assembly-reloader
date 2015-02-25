using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace AssemblyReloader.Game
{
    public interface IPartLoader
    {
        List<IAvailablePart> LoadedParts { get; }
    }
}
