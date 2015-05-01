using System;

namespace AssemblyReloader.Generators
{
    public class RandomStringGenerator : IRandomStringGenerator
    {
        public string Get()
        {
            return Guid.NewGuid().ToString("n");
        }
    }
}
