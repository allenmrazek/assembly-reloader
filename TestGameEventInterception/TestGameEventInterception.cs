using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace TestGameEventInterception
{
    public class TestGameEventInterception
    {
        private EventVoid EventVoidShouldNotBeIntercepted = new EventVoid("DoNotIntercept");
        private EventData<string> EventDataTShouldNotBeIntercepted = new EventData<string>("DoNotInterceptT");

        private EventData<string, int> EventDataT1T2ShouldNotBeIntercepted =
            new EventData<string, int>("DoNotInterceptT1T2");
        
        TestGameEventInterception()
        {
            
        }


        private void AddCallbacks()
        {
            GameEvents.onGamePause.Add(OnGamePaused); // EventVoid
            GameEvents.onVesselChange.Add(OnVesselChange); // EventData<T>
            GameEvents.onEditorLoad.Add(OnEditorLoad); // EventData<T1, T2>

            EventVoidShouldNotBeIntercepted.Add(Callback);
            EventDataTShouldNotBeIntercepted.Add(Callback);
            EventDataT1T2ShouldNotBeIntercepted.Add(Callback);
        }


        private void RemoveCallbacks()
        {
            GameEvents.onGamePause.Remove(OnGamePaused); // EventVoid
            GameEvents.onVesselChange.Remove(OnVesselChange); // EventData<T>
            GameEvents.onEditorLoad.Remove(OnEditorLoad); // EventData<T1, T2>

            EventVoidShouldNotBeIntercepted.Remove(Callback);
            EventDataTShouldNotBeIntercepted.Remove(Callback);
            EventDataT1T2ShouldNotBeIntercepted.Remove(Callback);
        }


        private void OnGamePaused()
        {
            
        }

        private void OnVesselChange(Vessel v)
        {
            
        }


        private void OnEditorLoad(ShipConstruct construct, CraftBrowser.LoadType ty)
        {
            
        }



        private void Callback()
        {
            
        }

        private void Callback(string s)
        {
            
        }

        private void Callback(string s, int i)
        {
            
        }

    }


    /// <summary>
    /// Make sure a hanging GameEvent is removed upon reload
    /// </summary>
    [KSPAddon(KSPAddon.Startup.Instantly, true)]
    public class TestGameEventProxyRemoval : MonoBehaviour
    {
        private void Start()
        {
            GameEvents.onGamePause.Add(OnGamePause); // do not unregister; ART should catch this
        }

        private void OnGamePause()
        {
            
        }
    }
}
