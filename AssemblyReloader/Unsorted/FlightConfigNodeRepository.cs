using System.Collections.Generic;
using AssemblyReloader.DataObjects;
using ReeperCommon.Containers;

namespace AssemblyReloader.Unsorted
{
    public class FlightConfigNodeRepository : IFlightConfigRepository
    {
        private readonly DictionaryQueue<KeyValuePair<uint, ITypeIdentifier>, ConfigNode> _storedNodes = new
            DictionaryQueue<KeyValuePair<uint, ITypeIdentifier>, ConfigNode>();


        public void Store(uint flightid, ITypeIdentifier key, ConfigNode data)
        {
            _storedNodes.Store(new KeyValuePair<uint, ITypeIdentifier>(flightid, key), data);
        }

        public Maybe<ConfigNode> Retrieve(uint flightid, ITypeIdentifier key)
        {
            return _storedNodes.Retrieve(new KeyValuePair<uint, ITypeIdentifier>(flightid, key));
        }

        public Maybe<ConfigNode> Peek(uint flightid, ITypeIdentifier key)
        {
            return _storedNodes.Peek(new KeyValuePair<uint, ITypeIdentifier>(flightid, key));
        }

        public void Clear()
        {
            _storedNodes.Clear();
        }
    }
}
