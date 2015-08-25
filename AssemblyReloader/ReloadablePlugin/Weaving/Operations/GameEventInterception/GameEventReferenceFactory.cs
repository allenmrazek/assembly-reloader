extern alias KSP;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using ReeperCommon.Containers;
using strange.extensions.injector.api;

namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations.GameEventInterception
{
    [Implements(typeof(IGameEventReferenceFactory), InjectionBindingScope.CROSS_CONTEXT)]
    public class GameEventReferenceFactory : IGameEventReferenceFactory
    {
        private readonly IGetGameEventFields _gameEventFields;

        public GameEventReferenceFactory(IGetGameEventFields gameEventFields)
        {
            if (gameEventFields == null) throw new ArgumentNullException("gameEventFields");

            _gameEventFields = gameEventFields;
        }


        public GameEventReference Create(object actualRef)
        {
            if (actualRef == null) throw new ArgumentNullException("actualRef");

            return new GameEventReference(actualRef, GetGameEventName(actualRef));
        }


        private string GetGameEventName(object actualRef)
        {
            return _gameEventFields
                .Get()
                .FirstOrDefault(fi => ReferenceEquals(fi.GetValue(null), actualRef))
                .ToMaybe()
                .Return(fi => fi.Name, "(Unknown GameEvent)");
        }
    }
}
