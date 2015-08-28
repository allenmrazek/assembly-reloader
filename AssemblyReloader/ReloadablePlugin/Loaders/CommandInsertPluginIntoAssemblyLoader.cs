//using System;
//using AssemblyReloader.Game;
//using AssemblyReloader.Gui;
//using ReeperCommon.Logging;
//using strange.extensions.command.impl;

//namespace AssemblyReloader.ReloadablePlugin.Loaders
//{
//    public class CommandInsertPluginIntoAssemblyLoader : Command
//    {
//        private readonly IPluginInfo _pluginInfo;
//        private readonly IGameAssemblyLoader _gameAssemblyLoader;
//        private readonly ILoadedAssemblyHandle _assemblyHandle;
//        private readonly SignalPluginCannotBeLoaded _failSignal;
//        private readonly SignalPluginWasLoaded _pluginWasLoadedSignal;
//        private readonly ILog _log;

//        public CommandInsertPluginIntoAssemblyLoader(
//            IPluginInfo pluginInfo,
//            IGameAssemblyLoader gameAssemblyLoader,
//            ILoadedAssemblyHandle assemblyHandle,
//            SignalPluginCannotBeLoaded failSignal,
//            SignalPluginWasLoaded pluginWasLoadedSignal,
//            ILog log)
//        {
//            if (pluginInfo == null) throw new ArgumentNullException("pluginInfo");
//            if (gameAssemblyLoader == null) throw new ArgumentNullException("gameAssemblyLoader");
//            if (assemblyHandle == null) throw new ArgumentNullException("assemblyHandle");
//            if (failSignal == null) throw new ArgumentNullException("failSignal");
//            if (pluginWasLoadedSignal == null) throw new ArgumentNullException("pluginWasLoadedSignal");
//            if (log == null) throw new ArgumentNullException("log");

//            _pluginInfo = pluginInfo;
//            _gameAssemblyLoader = gameAssemblyLoader;
//            _assemblyHandle = assemblyHandle;
//            _failSignal = failSignal;
//            _pluginWasLoadedSignal = pluginWasLoadedSignal;
//            _log = log;
//        }


//        public override void Execute()
//        {
//            try
//            {
//                var handle = _gameAssemblyLoader.AddToLoadedAssemblies(_assemblyHandle.LoadedAssembly.assembly,
//                    _pluginInfo.Location);

//                _pluginWasLoadedSignal.Dispatch(handle);
//            }
//            catch (DuplicateLoadedAssemblyException e)
//            {
//                _log.Error("Exception while inserting " + _pluginInfo.Name + " into AssemblyLoader: " + e);
//                _failSignal.Dispatch("Duplicate loaded assembly already exists in AssemblyLoader!");
//                Fail();
//            }
//            catch (Exception e)
//            {
//                _log.Error("Exception: " + e);
//                _failSignal.Dispatch("Exception occurred while loading plugin: " + e);
//                Fail();
//            }
//        }
//    }
//}
