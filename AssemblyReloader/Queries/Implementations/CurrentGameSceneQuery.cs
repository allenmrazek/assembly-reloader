using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssemblyReloader.Queries;

namespace AssemblyReloader.Providers
{
    public class CurrentGameSceneQuery : ICurrentGameSceneQuery
    {
        public GameScenes Get()
        {
            return HighLogic.LoadedScene;
        }
    }
}
