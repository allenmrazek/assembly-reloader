using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyReloader.Game
{
    public class KspPartLoader : IPartLoader
    {
        private readonly IKspFactory _kspFactory;

        public KspPartLoader(IKspFactory kspFactory)
        {
            if (kspFactory == null) throw new ArgumentNullException("kspFactory");
            _kspFactory = kspFactory;
        }


        public List<IAvailablePart> LoadedParts
        {
            get { return PartLoader.LoadedPartsList.Select(ap => _kspFactory.Create(ap)).ToList(); }
        }
    }
}
