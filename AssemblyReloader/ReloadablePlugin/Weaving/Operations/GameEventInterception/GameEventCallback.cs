using System;

namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations.GameEventInterception
{
    public class GameEventCallback : IEquatable<GameEventCallback>
    {
        private readonly object _del;

        public GameEventCallback(object del)
        {
            if (del == null) throw new ArgumentNullException("del");

            var ty = del.GetType();
            if (ty.BaseType == null || !typeof (MulticastDelegate).IsAssignableFrom(ty.BaseType))
                throw new ArgumentException("Must be a delegate", "del");

            _del = del;
        }

        public bool Equals(GameEventCallback other)
        {
            return _del == other._del;
        }

        public override int GetHashCode()
        {
            return _del.GetHashCode();
        }
    }
}
