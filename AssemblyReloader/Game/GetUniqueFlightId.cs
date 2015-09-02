extern alias KSP;
using strange.extensions.implicitBind;
using strange.extensions.injector.api;

namespace AssemblyReloader.Game
{
    [Implements(typeof(IGetUniqueFlightID), InjectionBindingScope.CROSS_CONTEXT)]
    public class GetUniqueFlightId : IGetUniqueFlightID
    {
        private static uint _internalCounter = 1;

        public uint Get()
        {
            return KSP::HighLogic.CurrentGame != null ? KSP::ShipConstruction.GetUniqueFlightID(KSP::HighLogic.CurrentGame.Updated().flightState) : _internalCounter++;
        }
    }
}
