extern alias KSP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using strange.extensions.injector.api;
using EventVoid = KSP::EventVoid;
using GameEvents = KSP::GameEvents;

namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations.GameEventInterception
{
    [Implements(typeof(IGetGameEventTypes), InjectionBindingScope.CROSS_CONTEXT)]
// ReSharper disable once ClassNeverInstantiated.Global
    public class GetGameEventTypes : IGetGameEventTypes
    {
        private readonly Type[] _validTypes =
        {
            typeof (EventVoid),
            typeof (KSP::EventData<>),
            typeof (KSP::EventData<,>),
            typeof (KSP::EventData<,,>),
            typeof (KSP::EventData<,,,>)
        };

        public IEnumerable<Type> Get(int genericParamCount)
        {
            return new[] { typeof(GameEvents), typeof(GameEvents.Contract), typeof(GameEvents.GameModifiers), typeof(GameEvents.VesselSituation) }
                .SelectMany(owner => owner.GetFields(BindingFlags.Public | BindingFlags.Static))
                .Where(fi => fi.FieldType.GetGenericArguments().Length == genericParamCount &&
                    _validTypes.Any(vt => vt == (fi.FieldType.IsGenericType ? fi.FieldType.GetGenericTypeDefinition() : fi.FieldType)))
                .Select(fi => fi.FieldType)
                .Distinct(); // there can be many GameEvents sharing the same generic type instance
        }
    }
}
