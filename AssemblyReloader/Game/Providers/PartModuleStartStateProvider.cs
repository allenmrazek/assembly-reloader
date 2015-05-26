using System;
using AssemblyReloader.Annotations;

namespace AssemblyReloader.Game.Providers
{
    public class PartModuleStartStateProvider : IPartModuleStartStateProvider
    {
        public PartModule.StartState Get([NotNull] IVessel vessel)
        {
            if (vessel == null) throw new ArgumentNullException("vessel");

            var state = PartModule.StartState.None;

            if (HighLogic.LoadedSceneIsEditor)
            {
                state |= PartModule.StartState.Editor;
            }
            else if (HighLogic.LoadedSceneIsFlight)
            {
                if (vessel.situation == Vessel.Situations.PRELAUNCH)
                {
                    state |= PartModule.StartState.PreLaunch;
                    state |= PartModule.StartState.Landed;
                }
                if (vessel.situation == Vessel.Situations.DOCKED)
                {
                    state |= PartModule.StartState.Docked;
                }
                if (vessel.situation == Vessel.Situations.ORBITING || vessel.situation == Vessel.Situations.ESCAPING)
                {
                    state |= PartModule.StartState.Orbital;
                }
                if (vessel.situation == Vessel.Situations.SUB_ORBITAL)
                {
                    state |= PartModule.StartState.SubOrbital;
                }
                if (vessel.situation == Vessel.Situations.SPLASHED)
                {
                    state |= PartModule.StartState.Splashed;
                }
                if (vessel.situation == Vessel.Situations.FLYING)
                {
                    state |= PartModule.StartState.Flying;
                }
                if (vessel.situation == Vessel.Situations.LANDED)
                {
                    state |= PartModule.StartState.Landed;
                }
            }

            return state;
        }
    }
}
