using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssemblyReloader.DataObjects;
using ReeperCommon.Containers;

namespace AssemblyReloader.Repositories
{
    public class FlightConfigRepository : IFlightConfigRepository
    {
        private readonly Dictionary<uint,
                            Dictionary<string,
                                Queue<ConfigNode>
                            >
                         > _entries = new Dictionary<uint, Dictionary<string, Queue<ConfigNode>>>();

        public void Store(uint flightid, ITypeIdentifier key, ConfigNode data)
        {
            if (key == null) throw new ArgumentNullException("key");
            if (data == null) throw new ArgumentNullException("data");

            Dictionary<string, Queue<ConfigNode>> configsForKey;

            if (!_entries.TryGetValue(flightid, out configsForKey))
                configsForKey = _entries[flightid] = new Dictionary<string, Queue<ConfigNode>>();

            Queue<ConfigNode> configData;

            if (!configsForKey.TryGetValue(key.Identifier, out configData))
                configData = _entries[flightid][key.Identifier] = new Queue<ConfigNode>();

            configData.Enqueue(data);
        }


        public Maybe<ConfigNode> Retrieve(uint flightid, ITypeIdentifier key)
        {
            if (key == null) throw new ArgumentNullException("key");

            var result = Peek(flightid, key);

            if (result.Any())
                _entries[flightid][key.Identifier].Dequeue();

            return result;
        }


        public Maybe<ConfigNode> Peek(uint flightid, ITypeIdentifier key)
        {
            Dictionary<string, Queue<ConfigNode>> configsForKey;
            Queue<ConfigNode> configData;


            if (!_entries.TryGetValue(flightid, out configsForKey)) return Maybe<ConfigNode>.None;
            if (!configsForKey.TryGetValue(key.Identifier, out configData)) return Maybe<ConfigNode>.None;


            return configData.Any()
                        ? Maybe<ConfigNode>.With(_entries[flightid][key.Identifier].Peek())
                        : Maybe<ConfigNode>.None;
        }


        public void Clear()
        {
            _entries.Clear();
        }
    }
}
