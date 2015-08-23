extern alias KSP;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ReeperCommon.Containers;
using ReeperCommon.Logging;
using GameEvents = KSP::GameEvents;

namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations.GameEventInterception
{
    // EventVoid and EventData<,,,> Add and Remove calls will be redirected to the appropriate
    // Register and Unregister method
    public static class GameEventProxy
    {
        private static readonly ILog Log = new DebugLog("GameEventProxy");
        private static readonly IDictionary<object, object> Cache = new Dictionary<object, object>();

        public static int Registrations
        {
            get { return -1; }
        }

        public static void Register(KSP::EventVoid evt, KSP::EventVoid.OnEvent callback)
        {
            var whichEvent =
                typeof(GameEvents).GetFields(BindingFlags.Public | BindingFlags.Static)
                    .FirstOrDefault(fi => fi.GetValue(null) == evt)
                    .ToMaybe();

            Log.Verbose("Received Register<EventVoid> with " + (whichEvent.Any() ? whichEvent.Single().Name : "<unknown>"));
        }


        public static void Unregister(KSP::EventVoid evt, KSP::EventVoid.OnEvent callback)
        {
            var whichEvent =
                typeof(GameEvents).GetFields(BindingFlags.Public | BindingFlags.Static)
                    .FirstOrDefault(fi => fi.GetValue(null) == evt)
                    .ToMaybe();

            Log.Verbose("Received Unregister<EventVoid> with " + (whichEvent.Any() ? whichEvent.Single().Name : "<unknown>"));
        }

        public static void Register<T>(KSP::EventData<T> evt, KSP::EventData<T>.OnEvent callback)
        {
            var whichEvent =
                typeof (GameEvents).GetFields(BindingFlags.Public | BindingFlags.Static)
                    .FirstOrDefault(fi => fi.GetValue(null) == evt)
                    .ToMaybe();

            Log.Verbose("Received Register<T> with " + (whichEvent.Any() ? whichEvent.Single().Name : "<unknown>"));
            //evt.Add(callback);
        }



        public static void Unregister<T>(KSP::EventData<T> evt, KSP::EventData<T>.OnEvent callback)
        {
            var whichEvent =
    typeof(GameEvents).GetFields(BindingFlags.Public | BindingFlags.Static)
        .FirstOrDefault(fi => fi.GetValue(null) == evt)
        .ToMaybe();

            Log.Verbose("Received Unregister<T> with " + (whichEvent.Any() ? whichEvent.Single().Name : "<unknown>"));
        }


        public static void Register<T1, T2>(KSP::EventData<T1, T2> evt, KSP::EventData<T1, T2>.OnEvent callback)
        {
            var whichEvent =
                typeof(GameEvents).GetFields(BindingFlags.Public | BindingFlags.Static)
                    .FirstOrDefault(fi => fi.GetValue(null) == evt)
                    .ToMaybe();

            Log.Verbose("Received Register<T1, T2> with " + (whichEvent.Any() ? whichEvent.Single().Name : "<unknown>"));
            //evt.Add(callback);
        }



        public static void Unregister<T1, T2>(KSP::EventData<T1, T2> evt, KSP::EventData<T1, T2>.OnEvent callback)
        {
            var whichEvent =
    typeof(GameEvents).GetFields(BindingFlags.Public | BindingFlags.Static)
        .FirstOrDefault(fi => fi.GetValue(null) == evt)
        .ToMaybe();

            Log.Verbose("Received Unregister<T1, T2> with " + (whichEvent.Any() ? whichEvent.Single().Name : "<unknown>"));
        }

        public static void Register<T1, T2, T3>(KSP::EventData<T1, T2, T3> evt, KSP::EventData<T1, T2, T3>.OnEvent callback)
        {
            var whichEvent =
                typeof(GameEvents).GetFields(BindingFlags.Public | BindingFlags.Static)
                    .FirstOrDefault(fi => fi.GetValue(null) == evt)
                    .ToMaybe();

            Log.Verbose("Received Register<T1, T2, T3> with " + (whichEvent.Any() ? whichEvent.Single().Name : "<unknown>"));
            //evt.Add(callback);
        }



        public static void Unregister<T1, T2, T3>(KSP::EventData<T1, T2, T3> evt, KSP::EventData<T1, T2, T3>.OnEvent callback)
        {
            var whichEvent =
                typeof(GameEvents).GetFields(BindingFlags.Public | BindingFlags.Static)
                    .FirstOrDefault(fi => fi.GetValue(null) == evt)
                    .ToMaybe();

            Log.Verbose("Received Unregister<T1, T2, T3> with " + (whichEvent.Any() ? whichEvent.Single().Name : "<unknown>"));
        }
    }
}
