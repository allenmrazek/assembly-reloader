using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyReloader.Providers
{
    class CurrentGameSceneProvider
    {
        public GameScenes Get()
        {
            return HighLogic.LoadedScene;
        }
    }
}
