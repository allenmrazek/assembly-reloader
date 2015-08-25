﻿using System;
using System.Linq;
using System.Reflection;
using ReeperCommon.Containers;

namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations.GameEventInterception
{
    public class GameEventCallback : IEquatable<GameEventCallback>
    {
        public Maybe<MethodBase> CallingMethod { get; private set; }
        private readonly object _del;

        public GameEventCallback(object del, Maybe<MethodBase> callingMethod )
        {
            CallingMethod = callingMethod;
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

        public override string ToString()
        {
            return
                typeof (GameEventCallback).Name + ": " +
                CallingMethod.Return(mi => mi.Name, "Unknown method") + " in " + CallingMethod
                    .SingleOrDefault()
                    .Return(mi =>
                        mi.With(inner => inner.DeclaringType).Return(t => t.FullName, "Unknown declaring type"), "Unknown declaring type");

        }
    }
}
