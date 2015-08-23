extern alias KSP;
using System;
using System.Linq;

namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations.GameEventInterception
{
    public class GameEventReference : IEquatable<GameEventReference>
    {
        private readonly object _geRef;

        public GameEventReference(object geRef)
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
        }


        public bool Equals(GameEventReference other)
        {
            return _geRef == other._geRef;
        }


        public override int GetHashCode()
        {
            return _geRef.GetHashCode();
        }
    }
}
