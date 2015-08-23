using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations.GameEventInterception
{
    public class GameEventRegistrar
    {
        public void Add(GameEventReference gameEvent, GameEventCallback callback)
        {
            if (gameEvent == null) throw new ArgumentNullException("gameEvent");
            if (callback == null) throw new ArgumentNullException("callback");
        }
    }
}
