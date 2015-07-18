using AssemblyReloader.Config.Keys;
using AssemblyReloader.StrangeIoC.extensions.command.impl;
using AssemblyReloader.StrangeIoC.extensions.injector;
using Mono.Cecil;
using ReeperCommon.Logging;

namespace AssemblyReloader.ReloadablePlugin.Weaving.Commands
{
    public class CommandRewriteAssemblyLocationCalls : Command
    {
        [Inject] public AssemblyDefinition AssemblyDefinition { get; set; }

        [Inject(LogKeys.PluginContext)]
        public ILog Log { get; set; }


        public override void Execute()
        {
            Log.Debug("Rewriting assembly location calls");
        }


        //var methodDefinition = new MethodDefinition(_realCall.Name,
        //        MethodAttributes.HideBySig | MethodAttributes.Public | MethodAttributes.Static,
        //        context.Module.TypeSystem.String);

        //    context.Methods.Add(methodDefinition);

        //    // this comparison method will have this signature:
        //    // string Method(Assembly assembly)
        //    //
        //    // It compares assembly to the result of GetExecutingAssembly(). If the references
        //    // match, the return value specified by the caller will be returned. Otherwise,
        //    // the result of that method is returned
        //    var assemblyType = context.Module.Import(typeof (Assembly));

        //    var getExecutingAssembly = context.Module.Import(typeof (Assembly).GetMethod("GetExecutingAssembly", BindingFlags.Public | BindingFlags.Static));
        //    var refRequals =
        //        context.Module.Import(typeof (System.Object).GetMethod("ReferenceEquals",
        //            BindingFlags.Public | BindingFlags.Static));


        //    if (getExecutingAssembly.IsNull()) throw new Exception("Failed to find Assembly.GetExecutingAssembly");
        //    if (refRequals.IsNull())
        //        throw new Exception("Failed to find System.Object.ReferenceEquals in module type system");

        //    var importedRealCall = context.Module.Import(_realCall);

        //    var processor = methodDefinition.Body.GetILProcessor();
        //    var realCallOpCode = _realCall.IsVirtual ? OpCodes.Callvirt : OpCodes.Call;

        //    methodDefinition.Parameters.Add(new ParameterDefinition("assembly", ParameterAttributes.None, assemblyType));

        //    var jumpLocation = processor.Create(OpCodes.Ldarg_0);

        //    processor.Emit(OpCodes.Call, getExecutingAssembly);
        //    processor.Emit(OpCodes.Ldarg_0);
        //    processor.Emit(OpCodes.Call, refRequals); // conditional jump will be inserted just after this
        //    processor.Emit(OpCodes.Brfalse_S, jumpLocation);
        //    processor.Emit(OpCodes.Ldstr, _retValue);
        //    processor.Emit(OpCodes.Ret);
        //    processor.Append(jumpLocation);
        //    processor.Emit(realCallOpCode, importedRealCall);
        //    processor.Emit(OpCodes.Ret);
    }
}
