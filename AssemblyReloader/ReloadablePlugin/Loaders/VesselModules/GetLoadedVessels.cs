extern alias KSP;
using System;
using System.Collections.Generic;
using System.Linq;
using AssemblyReloader.Game;
using strange.extensions.implicitBind;
using strange.extensions.injector.api;

namespace AssemblyReloader.ReloadablePlugin.Loaders.VesselModules
{
    [Implements(typeof(IGetLoadedVessels), InjectionBindingScope.CROSS_CONTEXT)]
// ReSharper disable once UnusedMember.Global
    public class GetLoadedVessels : IGetLoadedVessels
    {
        private readonly IGetCurrentSceneIsFlight _inFlightScene;
        private readonly IKspFactory _kspFactory;

        public GetLoadedVessels(IGetCurrentSceneIsFlight inFlightScene, IKspFactory kspFactory)
        {
            if (inFlightScene == null) throw new ArgumentNullException("inFlightScene");
            if (kspFactory == null) throw new ArgumentNullException("kspFactory");

            _inFlightScene = inFlightScene;
            _kspFactory = kspFactory;
        }


        public IEnumerable<IVessel> Get()
        {
            return _inFlightScene.Get() ? KSP::FlightGlobals.Vessels.Where(v => v.loaded).Select(v => _kspFactory.Create(v))
                : Enumerable.Empty<IVessel>();
        }
    }
}
