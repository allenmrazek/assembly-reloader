extern alias KSP;
using strange.extensions.injector;
using strange.extensions.mediation.impl;

namespace AssemblyReloader.Game
{
    public class GameEventMediator : Mediator
    {
        [Inject] public GameEventView View { get; set; }
        [Inject] public SignalOnLevelWasLoaded LevelWasLoadedSignal { get; set; }
        [Inject] public SignalApplicationQuitting QuittingSignal { get; set; }

        public override void OnRegister()
        {
            base.OnRegister();

            View.LevelWasLoaded.AddListener(LevelWasLoaded);
            View.ApplicationQuit.AddListener(ApplicationQuitting);
        }

        public override void OnRemove()
        {
            base.OnRemove();

            View.LevelWasLoaded.RemoveListener(LevelWasLoaded);
            View.ApplicationQuit.RemoveListener(ApplicationQuitting);
        }


        private void LevelWasLoaded(KSP::KSPAddon.Startup scene)
        {
            LevelWasLoadedSignal.Dispatch(scene);
        }


        private void ApplicationQuitting()
        {
            QuittingSignal.Dispatch();
        }
    }
}
