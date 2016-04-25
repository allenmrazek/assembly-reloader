extern alias KSP;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using ReeperAssemblyLibrary;
using ReeperCommon.Containers;
using ReeperCommon.Logging;
using ReeperKSP.Serialization.Exceptions;

namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations.GameEventInterception
{
    // EventVoid and EventData<,,,> Add and Remove calls will be redirected to the appropriate
    // Register and Unregister method
    public class GameEventProxy : IGameEventProxy
    {
        private readonly IGameEventReferenceFactory _referenceFactory;
        private readonly ILog _log;

        private readonly IDictionary<Assembly, IGameEventRegistry> _registries =
            new Dictionary<Assembly, IGameEventRegistry>();


        private static GameEventProxy _instance;

        private static GameEventProxy Instance
        {
            get
            {
                if (_instance == null)
                    throw new NoDefaultValueException(typeof (GameEventProxy));

                return _instance;
            }
        }

        public static IGameEventProxy Create(IGameEventReferenceFactory refFactory, ILog log)
        {
            if (refFactory == null) throw new ArgumentNullException("refFactory");
            if (log == null) throw new ArgumentNullException("log");

            _instance = new GameEventProxy(refFactory, log);

            return _instance;
        }


        private GameEventProxy(IGameEventReferenceFactory referenceFactory, ILog log)
        {
            if (referenceFactory == null) throw new ArgumentNullException("referenceFactory");
            if (log == null) throw new ArgumentNullException("log");

            _referenceFactory = referenceFactory;
            _log = log;
        }


        public void AddRegistry(ILoadedAssemblyHandle handle, IGameEventRegistry registry)
        {
            if (handle == null) throw new ArgumentNullException("handle");
            if (registry == null) throw new ArgumentNullException("registry");
            if (_registries.ContainsKey(handle.LoadedAssembly.assembly))
                throw new DuplicateGameEventRegistryException("Already have a registry for " + handle.LoadedAssembly.assembly.FullName);

            _registries.Add(handle.LoadedAssembly.assembly, registry);
            _log.Verbose("Added registry for " + handle.LoadedAssembly.dllName);
        }


        public void RemoveRegistry(ILoadedAssemblyHandle handle)
        {
            if (handle == null) throw new ArgumentNullException("handle");

            if (_registries.Remove(handle.LoadedAssembly.assembly))
                _log.Verbose("Removed registry for " + handle.LoadedAssembly.dllName);
            else _log.Warning("Did not find registry for " + handle.LoadedAssembly.dllName);
        }


        private IGameEventRegistry GetRegistry(Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");

            IGameEventRegistry registry;

            if (_registries.TryGetValue(assembly, out registry))
                return registry;

            throw new GameEventRegistryNotFoundException("Did not find a registry for " + assembly.FullName);
        }


        private void DoRegister(IGameEventRegistry registry, GameEventReference geRef, GameEventCallback geCallback)
        {
            registry.Add(geRef, geCallback);
            _log.Verbose("Registered callback for " + geRef.Name + " from " +
                         geCallback);
        }


        private void DoUnregister(IGameEventRegistry registry, GameEventReference geRef, GameEventCallback geCallback)
        {
            if (registry == null) throw new ArgumentNullException("registry");
            if (geRef == null) throw new ArgumentNullException("geRef");
            if (geCallback == null) throw new ArgumentNullException("geCallback");

            if (!registry.Remove(geRef, geCallback))
                _log.Warning("Did not unregister " + geCallback + "from " + geCallback + " successfully");
            else _log.Verbose("Unregistered callback for " + geRef + " for " + geCallback);
        }


        private void DoAction(Assembly @from, object gameEvent, object callback, StackFrame stack, Action<IGameEventRegistry, GameEventReference, GameEventCallback> action)
        {
            try
            {
                // note to self: we must avoid throwing exceptions here, since they'll end up unwinding back to
                // the calling method (and potentially past it), unexpectedly
                //
                // this is why these checks are within a try block
                if (@from == null) throw new ArgumentNullException("from");
                if (gameEvent == null) throw new ArgumentNullException("gameEvent");
                if (callback == null) throw new ArgumentNullException("callback");
                if (stack == null) throw new ArgumentNullException("stack");
                if (action == null) throw new ArgumentNullException("action");

                var geRef = _referenceFactory.Create(gameEvent);
                var geCallback = new GameEventCallback(callback, stack.GetMethod().ToMaybe());
                var registry = GetRegistry(@from);

                action(registry, geRef, geCallback);
            }
            catch (Exception e)
            {
                // we absolutely cannot let an unexpected exception from us unwind the stack
                // past the caller since we're supposed to be invisible!
                _log.Error("Exception! Details: " + e);
            }
        }


        private static void RegisterCallback(Assembly callingAssembly, object evt, object callback, StackFrame stack)
        {
            Instance.DoAction(callingAssembly, evt, callback, stack, Instance.DoRegister);
        }

        private static void UnregisterCallback(Assembly callingAssembly, object evt, object callback, StackFrame stack)
        {
            Instance.DoAction(callingAssembly, evt, callback, stack, _instance.DoUnregister);
        }


        //---------------------------------------------------------------------
        // EventData and EventVoid Add/Remove will be redirected to these methods
        //---------------------------------------------------------------------

// ReSharper disable once UnusedMember.Global
        public static void Register(EventVoid evt, EventVoid.OnEvent callback)
        {
            RegisterCallback(Assembly.GetCallingAssembly(), evt, callback, new StackFrame(1));
        }



// ReSharper disable once UnusedMember.Global
        public static void Unregister(EventVoid evt, EventVoid.OnEvent callback)
        {
            UnregisterCallback(Assembly.GetCallingAssembly(), evt, callback, new StackFrame(1));
        }


// ReSharper disable once UnusedMember.Global
        public static void Register<T>(EventData<T> evt, EventData<T>.OnEvent callback)
        {
            RegisterCallback(Assembly.GetCallingAssembly(), evt, callback, new StackFrame(1));
        }


// ReSharper disable once UnusedMember.Global
        public static void Unregister<T>(EventData<T> evt, EventData<T>.OnEvent callback)
        {
            UnregisterCallback(Assembly.GetCallingAssembly(), evt, callback, new StackFrame(1));
        }



// ReSharper disable once UnusedMember.Global
        public static void Register<T1, T2>(EventData<T1, T2> evt, EventData<T1, T2>.OnEvent callback)
        {
            RegisterCallback(Assembly.GetCallingAssembly(), evt, callback, new StackFrame(1));
        }


// ReSharper disable once UnusedMember.Global
        public static void Unregister<T1, T2>(EventData<T1, T2> evt, EventData<T1, T2>.OnEvent callback)
        {
            UnregisterCallback(Assembly.GetCallingAssembly(), evt, callback, new StackFrame(1));
        }



// ReSharper disable once UnusedMember.Global
        public static void Register<T1, T2, T3>(EventData<T1, T2, T3> evt, EventData<T1, T2, T3>.OnEvent callback)
        {
            RegisterCallback(Assembly.GetCallingAssembly(), evt, callback, new StackFrame(1));
        }


// ReSharper disable once UnusedMember.Global
        public static void Unregister<T1, T2, T3>(EventData<T1, T2, T3> evt, EventData<T1, T2, T3>.OnEvent callback)
        {
            UnregisterCallback(Assembly.GetCallingAssembly(), evt, callback, new StackFrame(1));
        }
    }
}
