//using System;
//using System.Collections;
//using AssemblyReloader.Game;
//using ReeperCommon.Logging;
//using strange.extensions.command.impl;
//using UnityEngine;

//namespace AssemblyReloader.ReloadablePlugin.Loaders
//{
//    public class CommandRemovePluginFromAssemblyLoader : Command
//    {
//        private readonly ILoadedAssemblyHandle _loadedHandle;
//        private readonly IGameAssemblyLoader _assemblyLoader;
//        private readonly IRoutineRunner _coroutineRunner;
//        private readonly SignalErrorWhileUnloading _unloadError;
//        private readonly ILog _log;

//        private Coroutine _doExecute;

//        public CommandRemovePluginFromAssemblyLoader(
//            ILoadedAssemblyHandle loadedHandle,
//            IGameAssemblyLoader assemblyLoader, 
//            IRoutineRunner coroutineRunner,
//            SignalErrorWhileUnloading unloadError,
//            ILog log)
//        {
//            if (loadedHandle == null) throw new ArgumentNullException("loadedHandle");
//            if (assemblyLoader == null) throw new ArgumentNullException("assemblyLoader");
//            if (coroutineRunner == null) throw new ArgumentNullException("coroutineRunner");
//            if (unloadError == null) throw new ArgumentNullException("unloadError");
//            if (log == null) throw new ArgumentNullException("log");

//            _loadedHandle = loadedHandle;
//            _assemblyLoader = assemblyLoader;
//            _coroutineRunner = coroutineRunner;
//            _unloadError = unloadError;
//            _log = log;
//        }


//        public override void Execute()
//        {
//            Retain();
//            _unloadError.AddListener(OnUnloadError);
//            _doExecute = _coroutineRunner.StartCoroutine(DoExecute());
//        }


//        private IEnumerator DoExecute()
//        {
//            yield return new WaitForEndOfFrame(); // we must wait for OnDestroys of unloaded objects to run, in case they access AssemblyLoader for some reason

//            _log.Debug("Removing plugin from AssemblyLoader");
//            _assemblyLoader.RemoveFromLoadedAssemblies(_loadedHandle);
//            _unloadError.RemoveListener(OnUnloadError);
//            Release();
//        }


//        private void OnUnloadError(string message)
//        {
//            _unloadError.RemoveListener(OnUnloadError);
//            _log.Warning("Did not remove from AssemblyLoader due to unload error");

//            _coroutineRunner.StopCoroutine(_doExecute);
//            Release();
//        }
//    }
//}
