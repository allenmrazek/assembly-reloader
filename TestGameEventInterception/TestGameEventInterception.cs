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
            GameEvents.onGamePause.Add(OnGamePaused); // EventVoid
            GameEvents.onVesselChange.Add(OnVesselChange); // EventData<T>
            GameEvents.onEditorLoad.Add(OnEditorLoad); // EventData<T1, T2>

            NotAGameEvent.Add(() => { });
        }


        private void RemoveCallbacks()
        {
            GameEvents.onGamePause.Remove(OnGamePaused); // EventVoid
            GameEvents.onVesselChange.Remove(OnVesselChange); // EventData<T>
            GameEvents.onEditorLoad.Remove(OnEditorLoad); // EventData<T1, T2>

            NotAGameEvent.Remove(() => { });
        }


        private void OnGamePaused()
        {
            
        }

        private void OnVesselChange(Vessel v)
        {
            
        }


        private void OnEditorLoad(ShipConstruct construct, KSP.UI.Screens.CraftBrowserDialog.LoadType ty)
        {
            
        }
    }


    /// <summary>
    /// Make sure a hanging GameEvent is removed upon reload
    /// </summary>
    [KSPAddon(KSPAddon.Startup.Instantly, true)]
    public class TestGameEventProxyRemoval : MonoBehaviour
    {
        private readonly EventVoid _customEvent = new EventVoid();

        private void Start()
        {
            print("TestGameEventProxyRemoval - Start");
            GameEvents.onGamePause.Add(OnGamePause); // do not unregister; ART should catch this
            //_customEvent.Add(OnGamePause); // and also this
        }

        private void OnGamePause()
        {
            
        }
    }
}
