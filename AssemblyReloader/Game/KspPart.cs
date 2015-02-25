using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AssemblyReloader.Game
{
    public class KspPart : IPart
    {
        private readonly Part _target;

        public KspPart(Part target)
        {
            _target = target;
            if (target == null) throw new ArgumentNullException("target");
        }

        public void RemoveModule(PartModule pm)
        {
            if (pm == null) throw new ArgumentNullException("pm");
            _target.RemoveModule(pm);
        }

        public GameObject GameObject
        {
            get { return _target.gameObject; }
        }

        public PartModuleList Modules
        {
            get { return _target.Modules; }
        }

        public string Name
        {
            get { return _target.name; }
        }

        public string PartName
        {
            get { return _target.partName; }
        }

        public IAvailablePart PartInfo
        {
            get { return new KspAvailablePart(_target.partInfo); }
        }
    }
}
