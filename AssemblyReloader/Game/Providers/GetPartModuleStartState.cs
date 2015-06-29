using System;
using System.Linq;
using ReeperCommon.Containers;

namespace AssemblyReloader.Game.Providers
{
    public class GetPartModuleStartState : IGetPartModuleStartState
    {
        public PartModule.StartState Get(Maybe<IVessel> vessel)
        {
            var state = PartModule.StartState.None;

            if (HighLogic.LoadedSceneIsEditor)
            {
                state |= PartModule.StartState.Editor;
            }
            else
            {
                if (!vessel.Any())
                    throw new ArgumentException("Expected a non-null Vessel");

                var v = vessel.Single();

                if (HighLogic.LoadedSceneIsFlight)
                {
                    if (v.situation == Vessel.Situations.PRELAUNCH)
                    {
                        state |= PartModule.StartState.PreLaunch;
                        state |= PartModule.StartState.Landed;
                    }
                    if (v.situation == Vessel.Situations.DOCKED)
                    {
                        state |= PartModule.StartState.Docked;
                    }
                    if (v.situation == Vessel.Situations.ORBITING || v.situation == Vessel.Situations.ESCAPING)
                    {
                        state |= PartModule.StartState.Orbital;
                    }
                    if (v.situation == Vessel.Situations.SUB_ORBITAL)
                    {
                        state |= PartModule.StartState.SubOrbital;
                    }
                    if (v.situation == Vessel.Situations.SPLASHED)
                    {
                        state |= PartModule.StartState.Splashed;
                    }
                    if (v.situation == Vessel.Situations.FLYING)
                    {
                        state |= PartModule.StartState.Flying;
                    }
                    if (v.situation == Vessel.Situations.LANDED)
                    {
                        state |= PartModule.StartState.Landed;
                    }
                }
            }

            return state;
        }
    }
}
