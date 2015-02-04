using System;
using System.Linq;
using AssemblyReloader.Addon.Destruction;
using AssemblyReloader.Queries.ConversionQueries;
using ReeperCommon.Logging;
using UnityEngine;

namespace AssemblyReloader.Addon
{
    class AddonFactory : IAddonFactory
    {
        private readonly IDestructionMediator _mediator;
        private readonly ILog _log;
        private readonly IAddonAttributeFromTypeQuery _addonFromTypeQuery;

        public AddonFactory(
            IDestructionMediator mediator,
            ILog log,
            IAddonAttributeFromTypeQuery addonFromTypeQuery)
        {
            if (mediator == null) throw new ArgumentNullException("mediator");
            if (log == null) throw new ArgumentNullException("log");
            if (addonFromTypeQuery == null) throw new ArgumentNullException("addonFromTypeQuery");

            _mediator = mediator;
            _log = log;
            _addonFromTypeQuery = addonFromTypeQuery;
        }


        public IDisposable CreateAddon(AddonInfo addonInfo)
        {
            _log.Normal("Constructing addon '" + addonInfo.type.FullName + "'");

            var addonHolder = new GameObject(addonInfo.type.FullName);

            var addon = addonHolder.AddComponent(addonInfo.type) as MonoBehaviour;
            addonInfo.created = true;

            return new Addon(addon, _mediator);
        }


        public AddonInfo CreateInfoForAddonType(Type type)
        {
            if (type == null) throw new ArgumentNullException("type");

            var kspAddon = _addonFromTypeQuery.Get(type);

            if (!kspAddon.Any()) throw new InvalidOperationException("type must have KSPAddon attribute");

            return new AddonInfo(type, kspAddon.Single());
        }
    }
}
