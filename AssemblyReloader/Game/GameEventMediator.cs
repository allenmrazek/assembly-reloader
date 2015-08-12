extern alias KSP;
using AssemblyReloader.StrangeIoC.extensions.injector;
using AssemblyReloader.StrangeIoC.extensions.mediation.impl;

namespace AssemblyReloader.Game
{
    public class GameEventMediator : Mediator
    {
        [Inject] public GameEventView View { get; set; }
        [Inject] public SignalOnLevelWasLoaded LevelWasLoadedSignal { get; set; }


        public override void OnRegister()
        {
            base.OnRegister();

            View.LevelWasLoaded.AddListener(LevelWasLoaded);
        }

        public override void OnRemove()
        {
            base.OnRemove();

            View.LevelWasLoaded.RemoveListener(LevelWasLoaded);
        }


        private void LevelWasLoaded(KSP::KSPAddon.Startup scene)
        {
            LevelWasLoadedSignal.Dispatch(scene);
        }
    }
}
