extern alias KSP;
using System;
using System.Linq;
using System.Reflection;
using ReeperCommon.Containers;

namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations.GameEventInterception
{
    public class GameEventReference : IEquatable<GameEventReference>
    {
        private readonly object _geRef;

        public GameEventReference(object geRef, string gameEventName)
        {
            if (geRef == null) throw new ArgumentNullException("geRef");

            var ty = geRef.GetType();

            if (!ty.IsGenericType)
            {
                if (ty != typeof (KSP::EventVoid))
                    throw new ArgumentException("Must be a GameEvent type", "geRef");
            } else if (
                new[] {typeof (KSP::EventData<>), typeof (KSP::EventData<,>), typeof (KSP::EventData<,,>), typeof(KSP::EventData<,,,>)}.All(
                    gt => ty.GetGenericTypeDefinition() != gt))
                throw new ArgumentException("Must be a GameEvent type", "geRef");


            _geRef = geRef;
            Name = gameEventName;
        }


        public bool Equals(GameEventReference other)
        {
            return _geRef == other._geRef;
        }


        public override int GetHashCode()
        {
            return _geRef.GetHashCode();
        }


        public override string ToString()
        {
            return typeof (GameEventReference).Name + ": " + Name;
        }

        public string Name { get; private set; }
    }
}
