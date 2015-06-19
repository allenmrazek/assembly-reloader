using ReeperCommon.Gui.Window;
using strange.extensions.mediation.impl;

namespace AssemblyReloader.Gui
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class MainWindowMediator : EventMediator
    {
        [Inject]
        public IWindowComponent ViewLogic { get; set; }

        override public void OnRegister()
        {
            //view.init();
            //dispatcher.AddListener
            //    (ServiceEvent.FULFILL_ONLINE_PLAYERS, onPlayers);
            //dispatcher.Dispatch
            //    (ServiceEvent.REQUEST_ONLINE_PLAYERS);
        }

        override public void OnRemove()
        {
            //dispatcher.RemoveListener
            //    (ServiceEvent.FULFILL_ONLINE_PLAYERS, onPlayers);
        }
    }
}
