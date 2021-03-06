using System;
using AssemblyReloader.Game;
using ReeperCommon.Containers;
using UnityEngine;

namespace AssemblyReloader.ReloadablePlugin.Loaders
{
    public class KspPart : IPart, IEquatable<KspPart>
    {
        private readonly Part _target;
        private readonly IKspFactory _kspFactory;

        public KspPart(Part target, IKspFactory kspFactory)
        {
            if (target == null) throw new ArgumentNullException("target");
            if (kspFactory == null) throw new ArgumentNullException("kspFactory");

            _target = target;
            _kspFactory = kspFactory;
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
            get { return _target.partInfo.name; }
        }

        public string PartName
        {
            get { return _target.partName; }
        }

        public IAvailablePart PartInfo {
            get { return _kspFactory.Create(_target.partInfo); }
        }

        public uint FlightID {
            get { return _target.flightID; }
            set { _target.flightID = value; }
        }


        public Maybe<IVessel> Vessel
        {
            get { return _target.vessel != null ? Maybe<IVessel>.With(_kspFactory.Create(_target.vessel)) : Maybe<IVessel>.None; }
        }

        public bool Equals(KspPart other)
        {
            return other != null && ReferenceEquals(_target, other._target);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as KspPart);
        }

        public override int GetHashCode()
        {
            return _target.GetHashCode();
        }

        public override string ToString()
        {
            return "KspPart: " + PartInfo.Name + ", " + FlightID;
        }
    }
}
