using System.Collections.Generic;
using ReeperCommon.Containers;

namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules
{
    [Implements(typeof(IPartModuleConfigNodeSnapshotRepository))]
// ReSharper disable once ClassNeverInstantiated.Global
    public class PartModuleConfigNodeSnapshotRepository : IPartModuleConfigNodeSnapshotRepository
    {
        private readonly DictionaryOfQueues<KeyValuePair<uint, TypeIdentifier>, ConfigNode> _storedNodes = new
            DictionaryOfQueues<KeyValuePair<uint, TypeIdentifier>, ConfigNode>(new FlightConfigNodeKeyValuePairComparer());


        public void Store(uint flightid, TypeIdentifier key, ConfigNode data)
        {
            _storedNodes.Store(new KeyValuePair<uint, TypeIdentifier>(flightid, key), data);
        }

        public Maybe<ConfigNode> Retrieve(uint flightid, TypeIdentifier key)
        {
            return _storedNodes.Retrieve(new KeyValuePair<uint, TypeIdentifier>(flightid, key));
        }

        public Maybe<ConfigNode> Peek(uint flightid, TypeIdentifier key)
        {
            return _storedNodes.Peek(new KeyValuePair<uint, TypeIdentifier>(flightid, key));
        }

        public void Clear()
        {
            _storedNodes.Clear();
        }

        public int Count()
        {
            return _storedNodes.Count();
        }
    }
}
