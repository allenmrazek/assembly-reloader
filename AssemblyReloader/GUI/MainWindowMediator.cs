using AssemblyReloader.StrangeIoC.extensions.injector;
using AssemblyReloader.StrangeIoC.extensions.mediation.impl;
using ReeperCommon.Gui.Window;

namespace AssemblyReloader.Gui
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class MainWindowMediator : Mediator
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
