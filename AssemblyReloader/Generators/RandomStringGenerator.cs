using System;

namespace AssemblyReloader.Generators
{
    [Implements(typeof(IRandomStringGenerator))]
    public class RandomStringGenerator : IRandomStringGenerator
    {
        public string Get()
        {
            return Guid.NewGuid().ToString("n");
        }
    }
}
