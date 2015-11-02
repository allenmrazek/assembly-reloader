extern alias KSP;
using System;
using System.Collections.Generic;
using System.Linq;
using strange.extensions.implicitBind;
using strange.extensions.injector.api;

namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations.GameEventInterception
{
    [Implements(typeof(IGetGameEventTypes), InjectionBindingScope.CROSS_CONTEXT)]
// ReSharper disable once ClassNeverInstantiated.Global
    public class GetGameEventTypes : IGetGameEventTypes
    {
        private readonly IGetGameEventFields _gameEventFields;

        public GetGameEventTypes(IGetGameEventFields gameEventFields)
        {
            if (gameEventFields == null) throw new ArgumentNullException("gameEventFields");
            _gameEventFields = gameEventFields;
        }

        public IEnumerable<Type> Get(int genericParamCount)
        {
            return _gameEventFields.Get()
                .Where(fi => fi.FieldType.GetGenericArguments().Length == genericParamCount)
                .Select(fi => fi.FieldType)
                .Distinct();
        }
    }
}
