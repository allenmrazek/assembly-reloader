using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Mono.Cecil.Cil;
using Mono.Cecil.Metadata;
using ReeperCommon.Extensions;
using ReeperCommon.FileSystem;
using ReeperCommon.FileSystem.Implementations;
using ReeperCommon.Logging;
using UnityEngine;
using Mono.Cecil;

namespace AssemblyReloader
{

    //[KSPAddon(KSPAddon.Startup.Instantly, true)]
    public class AssemblyReloaderOld : MonoBehaviour
    {
        private static Assembly MyResolveEventHandler(object sender, ResolveEventArgs args)
        {
            print("MyResolveeventHandler called");

            return typeof(AssemblyReloaderOld).Assembly;
        }

        System.Collections.IEnumerator Start()
        {
            var log = LogFactory.Create(LogLevel.Verbose);

            log.Normal("AssemblyReloader.awake");

            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(MyResolveEventHandler);
            var gamedata = new KSPGameDataDirectoryProvider();

            var testproj = new KSPDirectory(new KSPFileFactory(), gamedata.Directory()).Directory("TestProject");
            if (testproj.IsNull())
                log.Error("failed to get test project dir");

            var file = testproj.File("TestProject.dll");

            if (file.IsNull())
            {
                log.Error("failed to get TestProject.dll.reloadable");
                testproj.Files().ToList().ForEach(f => log.Normal("file: " + f.FileName));
            }

            log.Normal("file is at " + file.FullPath);

            var asm = Assembly.Load(System.IO.File.ReadAllBytes(file.FullPath));
            //var asm = Assembly.LoadWithPartialName(file.FullPath);

            asm.GetTypes()
                .Where(ty => ty.GetCustomAttributes(true).Any(attr => attr is KSPAddon))
                .ToList()
                .ForEach(ty =>
                {
                    log.Warning("Instantiating " + ty.FullName);
                    gameObject.AddComponent(ty);
                });

            //TestAssembly(log, file);
            RenameNamespaces(log, file);

            yield return new WaitForSeconds(3f);

            //if (AssemblyLoader.LoadPlugin(file.Info, file.UrlFile.url, null))
            //{
            //    log.Warning("AssemblyLoader:: success");

            //    AssemblyLoader.loadedAssemblies.ToList().ForEach(la => log.Normal("dllName = " + la.dllName));

            //    var found = AssemblyLoader.loadedAssemblies.First(la => la.dllName == "TestProject.dll");
            //}
            //else log.Error("AssemblyLoader:: fail");

            var aname = new AssemblyName("Modded.TestProject");
            aname.CodeBase = file.FullPath + ".edit";

            var modasm = Assembly.Load(System.IO.File.ReadAllBytes(file.FullPath + ".edit"));
            //var modasm = Assembly.Load(aname);

            if (ReferenceEquals(asm, modasm))
                log.Error("Did not load modified assembly into memory!");
            else log.Warning("Looks different!");
            AppDomain.CurrentDomain.GetAssemblies()
                .ToList()
                .ForEach(a => log.Normal("Loaded assembly: {0}", a.GetName().Name));

            modasm.GetTypes()
                .Where(ty => ty.GetCustomAttributes(true).Any(attr => attr is KSPAddon))
                .ToList()
                .ForEach(ty =>
                {
                    log.Warning("Instantiating " + ty.FullName);
                    gameObject.AddComponent(ty);
                });
        }

        private void TestAssembly(Log log, IFile file)
        {
            //Gets the AssemblyDefinition of "MyLibrary"
            AssemblyDefinition assembly = AssemblyDefinition.ReadAssembly(file.FullPath);

           
            MethodInfo writeLineMethod =
                typeof (UnityEngine.Debug).GetMethod("Log", new Type[] {typeof (object), typeof (UnityEngine.Object)});

            if (writeLineMethod.IsNull())
                log.Error("didn't find log method");

            var debugType = assembly.MainModule.Import(typeof(Debug)).Resolve();
            debugType.Methods.ToList().ForEach(md => log.Normal("Method: " + md.Name));

            assembly.MainModule.Import(debugType.Methods.Single(x => x.Name == "Log" && x.Parameters.Count == 2));

            //Gets all types of the MainModule of the assembly
            foreach (TypeDefinition type in assembly.MainModule.Types)
            {
                //if (type.Name != "<Module>")
                {
                    var writeMethod = type.Methods.SingleOrDefault(mi => mi.Name == "WriteString");
                    var targetMethod = type.Methods.SingleOrDefault(mi => mi.Name == "Update");

                    var processor = targetMethod.Body.GetILProcessor();

                    string sentence = String.Concat("Code added in ", targetMethod.Name);

                    var loadStr = processor.Create(OpCodes.Ldstr, sentence);
                    var callMethod = processor.Create(OpCodes.Call, writeMethod.Resolve());

                    processor.InsertBefore(targetMethod.Body.Instructions.First(), loadStr);
                    processor.InsertAfter(loadStr, callMethod);
                    break;
                }
            }

            assembly.Name.Version = new Version("1.2.3");

            assembly.Write(file.FullPath + ".modified.dll");
        }

        private void RenameNamespaces(Log log, IFile file)
        {
            var source = AssemblyDefinition.ReadAssembly(file.FullPath);

            //var nameDef = new AssemblyNameDefinition("Constructed", new Version("0.1.2"));


            //foreach (TypeDefinition srcType in source.MainModule.Types)
            //{
            //    if (srcType.Name == "<Module>") continue;

            //    srcType.Namespace = "Modded." + srcType.Namespace;
            //}


            ////Required to update the constructor arguments
            //foreach (var item in source.MainModule.Types)
            //{
            //    foreach (var attr in item.CustomAttributes)
            //    {
            //        if (attr.HasConstructorArguments)
            //        {
            //            if (attr.ConstructorArguments.Any(a => a.Type.Name == "Type"))
            //            {

            //            }

            //        }
            //    }
            //}


            source.Name.Name = "Modded." + source.Name.Name;

            var id = new AddonTypeIdentifier();
            
            //id.IdentifyAssembly(log, source);

            source.Write(file.FullPath + ".edit");

        }
    }
}
