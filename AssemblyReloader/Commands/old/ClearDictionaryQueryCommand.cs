using System;
using AssemblyReloader.ReloadablePlugin.Loaders;
using AssemblyReloader.ReloadablePlugin.Loaders.PartModules;

namespace AssemblyReloader.Commands.old
{
    public class ClearDictionaryQueryCommand<TKey, TValue> : ICommand
    {
        private readonly DictionaryQueue<TKey, TValue> _queue;

        public ClearDictionaryQueryCommand(DictionaryQueue<TKey, TValue> queue)
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
