using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using strange.extensions.injector.api;

namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations.GameEventInterception
{
    [Implements(typeof(IGetGameEventFields), InjectionBindingScope.CROSS_CONTEXT)]
    public class GetGameEventFields : IGetGameEventFields
    {
        private readonly Type[] _validTypes =
        {
            typeof (EventVoid),
            typeof (EventData<>),
            typeof (EventData<,>),
            typeof (EventData<,,>),
            typeof (EventData<,,,>)
        };


        private readonly Type[] _gameEventContainers =
        {
            typeof (GameEvents),
            typeof (GameEvents.Contract),
            typeof (GameEvents.GameModifiers),
            typeof (GameEvents.VesselSituation)
        };


        public IEnumerable<FieldInfo> Get()
        {
            return _gameEventContainers
                .SelectMany(owner => owner.GetFields(BindingFlags.Public | BindingFlags.Static))
                .Where(
                    fi =>
                        _validTypes.Any(
                            vt =>
                                vt ==
                                (fi.FieldType.IsGenericType ? fi.FieldType.GetGenericTypeDefinition() : fi.FieldType)));
        }
    }
}
