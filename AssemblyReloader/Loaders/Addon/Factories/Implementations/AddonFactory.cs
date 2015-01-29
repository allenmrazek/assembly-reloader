using System;
using AssemblyReloader.Mediators;
using AssemblyReloader.TypeTracking;
using ReeperCommon.Logging;
using UnityEngine;

namespace AssemblyReloader.Loaders.Addon.Factories.Implementations
{
    class AddonFactory : IAddonFactory
    {
        private readonly IDestructionMediator _mediator;
        private readonly ILog _log;

        public AddonFactory(
            IDestructionMediator mediator,
            ILog log)
        {
            if (mediator == null) throw new ArgumentNullException("mediator");
            if (log == null) throw new ArgumentNullException("log");

            _mediator = mediator;
            _log = log;
        }


        public IDisposable Create(AddonInfo addonInfo)
        {
            _log.Normal("Constructing addon '" + addonInfo.type.FullName + "'");

            var addonHolder = new GameObject(addonInfo.type.FullName);

            var addon = addonHolder.AddComponent(addonInfo.type) as MonoBehaviour;
            addonInfo.created = true;

            return new Addon(addon, _mediator);
        }
    }
}
