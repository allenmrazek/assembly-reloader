using System;
using AssemblyReloader.CompositeRoot;
using AssemblyReloader.Properties;

namespace AssemblyReloader.Commands
{
    public class ClearDictionaryQueryCommand<TKey, TValue> : ICommand
    {
        private readonly DictionaryQueue<TKey, TValue> _queue;

        public ClearDictionaryQueryCommand([NotNull] DictionaryQueue<TKey, TValue> queue)
        {
            if (queue == null) throw new ArgumentNullException("queue");
            _queue = queue;
        }

        public void Execute()
        {
            _queue.Clear();
        }
    }
}
