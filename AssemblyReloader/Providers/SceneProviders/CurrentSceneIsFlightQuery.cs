using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyReloader.Providers.SceneProviders
{
    public class CurrentSceneIsFlightQuery : ICurrentSceneIsFlightQuery
    {
        public bool Get()
        {
            return HighLogic.LoadedSceneIsFlight;
        }
    }
}
