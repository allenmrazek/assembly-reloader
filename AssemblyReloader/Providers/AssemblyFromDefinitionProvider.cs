//using System;
//using System.IO;
//using System.Linq;
//using System.Reflection;
//using Mono.Cecil;
//using ReeperCommon.Containers;
//using ReeperCommon.Logging.Implementations;

//namespace AssemblyReloader.Providers
//{
//    public class AssemblyFromDefinitionProvider : IAssemblyProvider
//    {
//        public Maybe<Assembly> Get(AssemblyDefinition definition)
//        {
//            if (definition == null) throw new ArgumentNullException("definition");

//            using (var symbolStream = new MemoryStream(1024 * 1024))
//            using (var stream =new MemoryStream(1024 * 1024))
//            {
//                definition.Write(stream, new WriterParameters
//                {
//                    WriteSymbols = true,
//                    SymbolStream = symbolStream
//                });

//                Assembly result = null;

//                //var result = Assembly.Load(stream.GetBuffer());
//                if (symbolStream.Length > 0)
//                    result = Assembly.Load(stream.GetBuffer(), symbolStream.GetBuffer());
//                else result = Assembly.Load(stream.GetBuffer());

//                if (result != null)
//                {
//                    var log = new DebugLog("AssemblyFromDefinitionProvider");

//                    result.GetReferencedAssemblies().ToList().ForEach(an => log.Normal(an.FullName + ", " + an.Version));

//                }
//                return result != null ? Maybe<Assembly>.With(result) : Maybe<Assembly>.None;
//            }
//        }
//    }
//}
