using System;
using strange.extensions.injector.api;

namespace AssemblyReloader
{
    [Implements(typeof(IGetRandomString), InjectionBindingScope.CROSS_CONTEXT)]
// ReSharper disable once ClassNeverInstantiated.Global
    public class GetRandomString : IGetRandomString
    {
        public string Get()
        {
            return Guid.NewGuid().ToString("n");
        }
    }
}
