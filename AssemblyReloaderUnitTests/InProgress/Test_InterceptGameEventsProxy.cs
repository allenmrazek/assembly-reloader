extern alias Cecil96;
using System.Linq;
using AssemblyReloaderTests.Fixtures;
using WeavingTestData.GameEventsMock;
using Xunit;
using Xunit.Extensions;
using OpCodes = Cecil96::Mono.Cecil.Cil.OpCodes;
using Cecil96::Mono.Cecil;
using Cecil96::Mono.Cecil.Rocks;

namespace AssemblyReloaderTests.InProgress
{
    public class Test_InterceptGameEventsProxy
    {
        [Theory, MockGameEventsDomainData]
        void Check(AssemblyDefinition definition, TypeDefinition type)
        {
            var genericType = type.Module.Import(typeof (EventDataMock<>));
            var genericParam = type.Module.Import(typeof(EventReport));
            var methodWithGameEventsAdd =
                definition.MainModule.GetType("WeavingTestData.GameEventsMock.AddGameEvent").Methods.Single(m => m.Name == "Execute");

            var definedOwningType = genericType.MakeGenericInstanceType(genericParam);
            var definedTypeAddMethod = definedOwningType.Resolve().Methods.Single(m => m.Name == "Add");
            var addMethod = definedTypeAddMethod.MakeHostInstanceGeneric(genericParam); // because resolving defined type lost its generic parameters


            // look for add calls
            foreach (var i in methodWithGameEventsAdd.Body.Instructions)
            {
                if (i.OpCode == OpCodes.Callvirt)
                {
                    var method = i.Operand as MethodReference;

                    if (method != null)
                    {



                    }
                }
            }


            Assert.NotNull(definedTypeAddMethod);
            Assert.NotNull(methodWithGameEventsAdd);
            Assert.NotNull(genericType);
            Assert.NotNull(genericParam);
            Assert.NotNull(definedOwningType);
            Assert.True(
    methodWithGameEventsAdd.Body.Instructions.Any(
        i => i.OpCode == OpCodes.Callvirt && ((MethodReference)i.Operand).FullName == addMethod.FullName));
        }
    }

    public static class CecilExtensions
    {
        public static MethodReference MakeHostInstanceGeneric(
                                  this MethodReference self,
                                  params TypeReference[] args)
        {
            var reference = new MethodReference(
                self.Name,
                self.ReturnType,
                self.DeclaringType.MakeGenericInstanceType(args))
            {
                HasThis = self.HasThis,
                ExplicitThis = self.ExplicitThis,
                CallingConvention = self.CallingConvention
            };

            foreach (var parameter in self.Parameters)
            {
                reference.Parameters.Add(new ParameterDefinition(parameter.ParameterType));
            }

            foreach (var genericParam in self.GenericParameters)
            {
                reference.GenericParameters.Add(new GenericParameter(genericParam.Name, reference));
            }

            return reference;
        }
    }
}
