using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyReloader.Queries
{
    public interface IStartupSceneFromGameSceneQuery
    {
        KSPAddon.Startup Get(GameScenes gameScene);
    }
}
