//extern alias Cecil96;
//using System;
//using System.Linq;
//using ReeperCommon.Containers;
//using ReeperCommon.Logging;

//namespace AssemblyReloader.ReloadablePlugin.Weaving.old
//{
//    public class AssemblyDefinitionWeaver : IAssemblyDefinitionReader
//    {
//        private readonly ILog _log;
//        private readonly IAssemblyDefinitionReader _definitionReader;
//        private readonly SignalWeaveDefinition _weaveDefinitionSignal;


//        public AssemblyDefinitionWeaver(
//            IAssemblyDefinitionReader definitionReader, 
//            SignalWeaveDefinition weaveDefinitionSignal,
//            ILog log)
//        {
//            if (definitionReader == null) throw new ArgumentNullException("definitionReader");
//            if (weaveDefinitionSignal == null) throw new ArgumentNullException("weaveDefinitionSignal");
//            if (log == null) throw new ArgumentNullException("log");

//            _log = log;
//            _definitionReader = definitionReader;
//            _weaveDefinitionSignal = weaveDefinitionSignal;
//        }


//        public Maybe<Cecil96::Mono.Cecil.AssemblyDefinition> Read()
//        {
//            var unmodifiedDefinition = _definitionReader.Read();

//            if (!unmodifiedDefinition.Any())
//                return Maybe<Cecil96::Mono.Cecil.AssemblyDefinition>.None;

//            var weavedDefinition = unmodifiedDefinition.Single();

//            _log.Debug("Weaving definition of {0}", weavedDefinition.FullName);
//            _weaveDefinitionSignal.Dispatch(weavedDefinition);

//            return Maybe<Cecil96::Mono.Cecil.AssemblyDefinition>.With(weavedDefinition);
//        }
//    }
//}
