using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReeperCommon.Containers;

namespace AssemblyReloader.Repositories
{
    public class PartModuleFlightConfigRepository : IPartModuleFlightConfigRepository
    {
        private readonly Dictionary<uint,
                            Dictionary<string,
                                Queue<ConfigNode>
                            >
                         > _entries = new Dictionary<uint, Dictionary<string, Queue<ConfigNode>>>();
  
        public void Store(uint flightid, string key, ConfigNode data)
        {
            if (key == null) throw new ArgumentNullException("key");
            if (data == null) throw new ArgumentNullException("data");

            Dictionary<string, Queue<ConfigNode>> configsForKey;

            if (!_entries.TryGetValue(flightid, out configsForKey))
                configsForKey = _entries[flightid] = new Dictionary<string, Queue<ConfigNode>>();

            Queue<ConfigNode> configData;

            if (!configsForKey.TryGetValue(key, out configData))
                configData = _entries[flightid][key] = new Queue<ConfigNode>();

            configData.Enqueue(data);
        }


        public Maybe<ConfigNode> Retrieve(uint flightid, string key)
        {
            if (key == null) throw new ArgumentNullException("key");

            Dictionary<string, Queue<ConfigNode>> configsForKey;
            Queue<ConfigNode> configData;


            if (!_entries.TryGetValue(flightid, out configsForKey)) return Maybe<ConfigNode>.None;
            if (!configsForKey.TryGetValue(key, out configData)) return Maybe<ConfigNode>.None;


            return configData.Any()
                        ? Maybe<ConfigNode>.With(_entries[flightid][key].Dequeue())
                        : Maybe<ConfigNode>.None;
        }


        public void Clear()
        {
            _entries.Clear();
        }
    }
}
