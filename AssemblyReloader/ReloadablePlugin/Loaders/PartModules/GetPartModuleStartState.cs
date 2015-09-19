extern alias KSP;
using System;
using System.Linq;
using AssemblyReloader.Game;
using ReeperCommon.Containers;
using strange.extensions.implicitBind;
using strange.extensions.injector.api;
using PartModule = KSP::PartModule;
using Vessel = KSP::Vessel;
using HighLogic = KSP::HighLogic;

namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules
{
    [Implements(typeof(IGetPartModuleStartState), InjectionBindingScope.CROSS_CONTEXT)]
// ReSharper disable once UnusedMember.Global
    public class GetPartModuleStartState : IGetPartModuleStartState
    {
        public KSP::PartModule.StartState Get(Maybe<IVessel> vessel)
        {
            var state = KSP::PartModule.StartState.None;

            if (KSP::HighLogic.LoadedSceneIsEditor)
            {
                state |= KSP::PartModule.StartState.Editor;
            }
            else
            {
                if (!vessel.Any())
                    throw new ArgumentException("Expected a non-null Vessel");

                var v = vessel.Single();

                if (HighLogic.LoadedSceneIsFlight)
                {
                    if (v.Situation == KSP::Vessel.Situations.PRELAUNCH)
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
