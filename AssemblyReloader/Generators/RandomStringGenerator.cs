using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
