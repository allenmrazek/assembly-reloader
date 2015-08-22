using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeavingTestData.GameEventsReal
{
    public class AddGameEvent
    {
        public void Execute()
        {
            GameEvents.onCrash.Add(OnCrashCallback);
            GameEvents.onCrewOnEva.Add(OnCrewOnEVA);

            var evt = GameEvents.onVesselChange;

            evt.Add(OnVesselChange);
        }


        public void OnCrashCallback(EventReport report)
        {
            
        }

        public void OnCrewOnEVA(GameEvents.FromToAction<Part, Part> between)
        {
            
        }

        public void OnVesselChange(Vessel vessel)
        {
            
        }
    }
}
