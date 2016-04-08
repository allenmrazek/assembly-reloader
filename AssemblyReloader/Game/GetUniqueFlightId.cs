using strange.extensions.implicitBind;
using strange.extensions.injector.api;

namespace AssemblyReloader.Game
{
    [Implements(typeof(IGetUniqueFlightID), InjectionBindingScope.CROSS_CONTEXT)]
// ReSharper disable once UnusedMember.Global
    public class GetUniqueFlightId : IGetUniqueFlightID
    {
        private static uint _internalCounter = 1;

        public uint Get()
        {
            return HighLogic.CurrentGame != null ? ShipConstruction.GetUniqueFlightID(HighLogic.CurrentGame.Updated().flightState) : _internalCounter++;
        }
    }
}
