using AssemblyReloader.StrangeIoC.extensions.implicitBind;
using AssemblyReloader.StrangeIoC.extensions.injector.api;

namespace AssemblyReloader.Game
{
    [Implements(typeof(IGetUniqueFlightID), InjectionBindingScope.CROSS_CONTEXT)]
    public class GetUniqueFlightId : IGetUniqueFlightID
    {
        private static uint _internalCounter = 1;

        static GetUniqueFlightId()
        {
            _internalCounter = 1;
        }

        public uint Get()
        {
            return HighLogic.CurrentGame != null ? ShipConstruction.GetUniqueFlightID(HighLogic.CurrentGame.Updated().flightState) : _internalCounter++;
        }
    }
}
