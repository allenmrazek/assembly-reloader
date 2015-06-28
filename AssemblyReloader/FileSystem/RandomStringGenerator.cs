using System;
using AssemblyReloader.StrangeIoC.extensions.implicitBind;

namespace AssemblyReloader.FileSystem
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
