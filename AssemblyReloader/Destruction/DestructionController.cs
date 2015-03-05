using System;
using System.Collections.Generic;
using System.Linq;
using ReeperCommon.Containers;

namespace AssemblyReloader.Destruction
{
    public class DestructionController : IDestructionController
    {
        private readonly Dictionary<Type, Delegate> _actions = new Dictionary<Type, Delegate>(); 

        
        public void Destroy<T>(T target)
        {
            var del = GetDestructionMethodFor(typeof (T));

            if (!del.Any())
                throw new Exception(typeof (T).FullName + " handler not registered");

            del.Single().DynamicInvoke(target);
        }


        public void Register<T>(Action<T> f)
        {
            Delegate del;

            if (_actions.TryGetValue(typeof (T), out del))
                _actions[typeof(T)] = Delegate.Combine(del, f);
            else _actions[typeof (T)] = f;
        }


        private Maybe<Delegate> GetDestructionMethodForExact(Type type)
        {
            Delegate del;

            return _actions.TryGetValue(type, out del) ? Maybe<Delegate>.With(del) : Maybe<Delegate>.None;
        }


        private Maybe<Delegate> GetDestructionMethodFor(Type type)
        {
            var current = type;

            do
            {
                var result = GetDestructionMethodForExact(current);
                if (result.Any()) return result;

                current = current.BaseType;
            } while (current != null);


            return Maybe<Delegate>.None;
        }
    }
}
