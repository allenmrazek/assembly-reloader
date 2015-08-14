extern alias KSP;
using System;
using AssemblyReloader.Game;
using ReeperCommon.Containers;
using strange.extensions.injector.api;

namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules
{
    [Implements(typeof(IGetVesselFromPartModule), InjectionBindingScope.CROSS_CONTEXT)]
    public class GetVesselFromPartModule : IGetVesselFromPartModule
    {
        private readonly IKspFactory _kspFactory;


        public GetVesselFromPartModule(IKspFactory kspFactory)
        {
            if (kspFactory == null) throw new ArgumentNullException("kspFactory");
            _kspFactory = kspFactory;
        }


        public Maybe<IVessel> Get(KSP::PartModule partModule)
        {
            if (partModule == null) throw new ArgumentNullException("partModule");

            if (partModule.part == null || partModule.part.vessel == null)
                return Maybe<IVessel>.None;
            return Maybe<IVessel>.With(_kspFactory.Create(partModule.part.vessel));
        }
    }
}
