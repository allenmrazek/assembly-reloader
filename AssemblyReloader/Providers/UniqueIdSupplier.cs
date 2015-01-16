using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyReloader.Providers
{
    class UniqueIdSupplier
    {
        private static int _id = 0;

        public int Generate()
        {
            return _id++;
        }
    }
}
