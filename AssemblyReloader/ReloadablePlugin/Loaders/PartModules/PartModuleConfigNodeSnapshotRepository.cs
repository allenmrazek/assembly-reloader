extern alias KSP;
using System.Collections.Generic;
using ReeperCommon.Containers;
using strange.extensions.implicitBind;

namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules
{
    [Implements(typeof(IPartModuleConfigNodeSnapshotRepository))]
// ReSharper disable once ClassNeverInstantiated.Global
    public class PartModuleConfigNodeSnapshotRepository : IPartModuleConfigNodeSnapshotRepository
    {
        private readonly DictionaryOfQueues<KeyValuePair<uint, TypeIdentifier>, KSP::ConfigNode> _storedNodes = new
            DictionaryOfQueues<KeyValuePair<uint, TypeIdentifier>, KSP::ConfigNode>(new FlightConfigNodeKeyValuePairComparer());


        public void Store(uint flightid, TypeIdentifier key, KSP::ConfigNode data)
        {
            _storedNodes.Store(new KeyValuePair<uint, TypeIdentifier>(flightid, key), data);
        }

        public Maybe<KSP::ConfigNode> Retrieve(uint flightid, TypeIdentifier key)
        {
            return _storedNodes.Retrieve(new KeyValuePair<uint, TypeIdentifier>(flightid, key));
        }

        public Maybe<KSP::ConfigNode> Peek(uint flightid, TypeIdentifier key)
        {
            return _storedNodes.Peek(new KeyValuePair<uint, TypeIdentifier>(flightid, key));
        }

        public void Clear()
        {
            _storedNodes.Clear();
        }
    }
}
