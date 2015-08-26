extern alias KSP;
using System;
using System.Linq;
using System.Reflection;
using ReeperCommon.Containers;

namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations.GameEventInterception
{
    public class GameEventReference : IEquatable<GameEventReference>
    {
        public readonly object GameEventRef;

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


            GameEventRef = geRef;
            Name = gameEventName;
        }


        public bool Equals(GameEventReference other)
        {
            return GameEventRef == other.GameEventRef;
        }


        public override int GetHashCode()
        {
            return GameEventRef.GetHashCode();
        }


        public override string ToString()
        {
            return typeof (GameEventReference).Name + ": " + Name;
        }

        public string Name { get; private set; }

        bool IEquatable<GameEventReference>.Equals(GameEventReference other)
        {
            return GameEventRef == other.GameEventRef;
        }

        public override bool Equals(object obj)
        {
            var other = obj as GameEventReference;
            if (other == null) return false;

            return GameEventRef == other.GameEventRef;
        }
    }
}
