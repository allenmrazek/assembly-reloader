extern alias KSP;
using UnityEngine;

namespace TestGameEventInterception
{
    // Can we trick the interception logic?
    public class EventVoid
    {
        public delegate void OnEvent();

        public void Add(OnEvent evt)
        {
            
        }

        public void Remove(OnEvent evt)
        {
            
        }
    }

    public class TestGameEventInterception
    {
        private readonly EventVoid NotAGameEvent = new EventVoid();

        TestGameEventInterception()
        {
            AddCallbacks();
            RemoveCallbacks();
        }


        private void AddCallbacks()
        {
            KSP::GameEvents.onGamePause.Add(OnGamePaused); // EventVoid
            KSP::GameEvents.onVesselChange.Add(OnVesselChange); // EventData<T>
            KSP::GameEvents.onEditorLoad.Add(OnEditorLoad); // EventData<T1, T2>

            NotAGameEvent.Add(() => { });
        }


        private void RemoveCallbacks()
        {
            KSP::GameEvents.onGamePause.Remove(OnGamePaused); // EventVoid
            KSP::GameEvents.onVesselChange.Remove(OnVesselChange); // EventData<T>
            KSP::GameEvents.onEditorLoad.Remove(OnEditorLoad); // EventData<T1, T2>

            NotAGameEvent.Remove(() => { });
        }


        private void OnGamePaused()
        {
            
        }

        private void OnVesselChange(KSP::Vessel v)
        {
            
        }


        private void OnEditorLoad(KSP::ShipConstruct construct, KSP::CraftBrowser.LoadType ty)
        {
            
        }
    }


    /// <summary>
    /// Make sure a hanging GameEvent is removed upon reload
    /// </summary>
    [KSP::KSPAddon(KSP::KSPAddon.Startup.Instantly, true)]
    public class TestGameEventProxyRemoval : MonoBehaviour
    {
        private readonly EventVoid _customEvent = new EventVoid();

        private void Start()
        {
            print("TestGameEventProxyRemoval - Start");
            KSP::GameEvents.onGamePause.Add(OnGamePause); // do not unregister; ART should catch this
            //_customEvent.Add(OnGamePause); // and also this
        }

        private void OnGamePause()
        {
            
        }
    }
}
