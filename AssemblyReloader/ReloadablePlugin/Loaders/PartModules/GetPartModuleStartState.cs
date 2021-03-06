using System;
using System.Linq;
using AssemblyReloader.Game;
using ReeperCommon.Containers;
using strange.extensions.implicitBind;
using strange.extensions.injector.api;

namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules
{
    [Implements(typeof(IGetPartModuleStartState), InjectionBindingScope.CROSS_CONTEXT)]
// ReSharper disable once UnusedMember.Global
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
                    if (v.Situation == Vessel.Situations.PRELAUNCH)
                    {
                        state |= PartModule.StartState.PreLaunch;
                        state |= PartModule.StartState.Landed;
                    }
                    if (v.Situation == Vessel.Situations.DOCKED)
                    {
                        state |= PartModule.StartState.Docked;
                    }
                    if (v.Situation == Vessel.Situations.ORBITING || v.Situation == Vessel.Situations.ESCAPING)
                    {
                        state |= PartModule.StartState.Orbital;
                    }
                    if (v.Situation == Vessel.Situations.SUB_ORBITAL)
                    {
                        state |= PartModule.StartState.SubOrbital;
                    }
                    if (v.Situation == Vessel.Situations.SPLASHED)
                    {
                        state |= PartModule.StartState.Splashed;
                    }
                    if (v.Situation == Vessel.Situations.FLYING)
                    {
                        state |= PartModule.StartState.Flying;
                    }
                    if (v.Situation == Vessel.Situations.LANDED)
                    {
                        state |= PartModule.StartState.Landed;
                    }
                }
            }

            return state;
        }
    }
}
