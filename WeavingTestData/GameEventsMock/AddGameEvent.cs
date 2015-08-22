using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeavingTestData.GameEventsMock
{
    public class AddGameEvent
    {
        public void Execute()
        {
            MockedGameEvents.TestEvent.Add(EventCallback);
        }

        public void EventCallback(EventReport param)
        {
            
        }
    }
}
