using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyReloader.Messages.Implementation
{
    class SceneChange : IMessage
    {
        public GameScenes Scene { get; private set; }

        public SceneChange(GameScenes scene)
        {
            Scene = scene;
        }
    }
}
