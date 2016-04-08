using System;
using System.Collections.Generic;
using System.Linq;
using ReeperCommon.Containers;

namespace AssemblyReloader.ReloadablePlugin.Loaders
{
    public class DictionaryOfQueues<TKey, TValue>
    {
        private readonly Dictionary<TKey, Queue<TValue>> _items;
       


        public DictionaryOfQueues(IEqualityComparer<TKey> comparer)
        {
            if (comparer == null) throw new ArgumentNullException("comparer");

            _items = new Dictionary<TKey, Queue<TValue>>(comparer);
        }


        public DictionaryOfQueues()
        {
            _items = new Dictionary<TKey, Queue<TValue>>();
        }


        public Maybe<TValue> Peek(TKey key)
        {
            Queue<TValue> q;

            if (_items.TryGetValue(key, out q) && q.Count > 0)
                return Maybe<TValue>.With(q.Peek());

            return Maybe<TValue>.None;
        }


        public Maybe<TValue> Retrieve(TKey key)
        {
            var result = Peek(key);

            if (result.Any()) _items[key].Dequeue();

            return result;
        }


        public void Store(TKey key, TValue item)
        {
            Queue<TValue> q;

            if (_items.TryGetValue(key, out q))
            {
                q.Enqueue(item);
            }
            else
            {
                q = new Queue<TValue>();
                q.Enqueue(item);

                _items.Add(key, q);
            }
        }


        public void Clear()
        {
            _items.Clear();
        }


        public int Count()
        {
            return _items.Sum(kvp => kvp.Value.Count);
        }
    }
}
