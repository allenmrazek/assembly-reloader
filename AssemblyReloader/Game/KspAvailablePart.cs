using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyReloader.Game
{
    public class KspAvailablePart : IAvailablePart
    {
        private readonly AvailablePart _ap;

        public KspAvailablePart(AvailablePart ap)
        {
            _ap = ap;
            if (ap == null) throw new ArgumentNullException("ap");
        }

        public string Name
        {
            get { return _ap.name; }
        }

        public IPart partPrefab
        {
            get { return new KspPart(_ap.partPrefab); }
        }
    }
}
