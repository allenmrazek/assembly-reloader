using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyReloader.Destruction
{
    public interface IPartModuleDestroyer
    {
        void Destroy(PartModule pm);
    }
}
