using System;
using AssemblyReloader.Game;
using ReeperCommon.Containers;

namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules
{
    public class GetVesselFromPartModule : IGetVesselFromPartModule
    {
        private readonly IKspFactory _kspFactory;


        public GetVesselFromPartModule(IKspFactory kspFactory)
        {
            if (kspFactory == null) throw new ArgumentNullException("kspFactory");
            _kspFactory = kspFactory;
        }


        public Maybe<IVessel> Get(PartModule partModule)
        {
            if (partModule == null) throw new ArgumentNullException("partModule");

            if (partModule.part == null || partModule.part.vessel == null)
                return Maybe<IVessel>.None;
            return Maybe<IVessel>.With(_kspFactory.Create(partModule.part.vessel));
        }
    }
}
