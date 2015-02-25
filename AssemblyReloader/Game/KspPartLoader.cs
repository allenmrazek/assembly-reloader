using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyReloader.Game
{
    public class KspPartLoader : IPartLoader
    {
        public List<IAvailablePart> LoadedParts
        {
            get { return PartLoader.LoadedPartsList.Select(ap => new KspAvailablePart(ap)).Cast<IAvailablePart>().ToList(); }
        }
    }
}
