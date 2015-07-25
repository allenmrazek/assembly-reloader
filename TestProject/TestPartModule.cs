using System.Linq;
using System.Reflection;
using ReeperCommon.Serialization;

namespace TestProject
{
    public class TestPartModule : PartModule
    {
        //private ConfigNode local = new ConfigNode();

        ////void Awake()
        ////{
        ////    print("TestPartModule awake");
        ////}

        public override void OnAwake()
        {
            base.OnAwake();

#if MODIFIED
            print("TestPartModule awake (**MODIFIED** version)");
#else
            print("TestPartModule awake (unmodified version)");
#endif
            print("hello, testing world!");

            print("TestPartModule running from " + Assembly.GetExecutingAssembly().CodeBase);
            //print("TestPartModule is running from " +
            //      (part == null || ReferenceEquals(part, part.partInfo.partPrefab) ? "prefab" : "clone"));
            print("Done running checks");

            //AssemblyLoader.loadedAssemblies.Where(la => la.dllName.StartsWith("TestProject")).ToList().ForEach(asm =>
            //{
            //    print("LoadedAssembly: " + asm.dllName);
            //    foreach (var kvp in asm.types.ToDictionary(t => t.Key.FullName, items => items.Value).OrderBy(kvp => kvp.Key))
            //    {
            //        print("Type: " + kvp.Key);
            //        foreach (var ty in kvp.Value)
            //            print("       " + ty.FullName);
            //    }
            //});

            //if (AssemblyLoader.loadedAssemblies == null)
            //{
            //    print("AssemblyLoader.list is null!");
            //    return;
            //}

            //if (AssemblyLoader.loadedAssemblies.Any(la => la == null))
            //{
            //    print("An item in AssemblyLoader is null!");
            //    return;
            //}

            //AssemblyLoader.loadedAssemblies.ToList().ForEach(la => print(la.dllName));

            //if (HighLogic.LoadedSceneIsEditor)
            //AssemblyLoader.loadedAssemblies
            //    .Where(la => la.dllName.StartsWith("TestProject"))
            //    .Select(la => la.types)
            //    .ToList().ForEach(loadedTypes =>
            //    {
            //        foreach (var key in loadedTypes.Keys)
            //            foreach (var ty in loadedTypes[key])
            //                print(key.Name + ": " + ty.Name);
            //    });

        }

        public override void OnLoad(ConfigNode node)
        {
           // print("TestPartModule running from " + Assembly.GetExecutingAssembly().CodeBase);
            print(string.Format("TestPartModule.OnLoad: {0}", node.ToString()));
        }

        public override void OnSave(ConfigNode node)
        {
            print(string.Format("TestPartModule.OnSave: {0}", node.ToString()));

            var current = Assembly.GetExecutingAssembly();

            print("Hello, world!");
            print("Also, I'm running from CodeBase: " + current.CodeBase);
            print("Or Location: " + current.Location);

            //SomeMethod(local);
        }

        private void SomeMethod(ConfigNode node)
        {
            
        }

        private void CheckLocationOfOtherAssembly()
        {
            var otherAssembly = typeof (IConfigNodeSerializer).Assembly;

            print("Location of ReeperCommon: " + otherAssembly.Location);
        }

        //private static string ReturnCodeBaseLocation(Assembly assembly)
        //{
        //    if (ReferenceEquals(assembly, Assembly.GetExecutingAssembly()))
        //        return "this location";

        //    return assembly.CodeBase;
        //}

#if MODIFIED
        [KSPEvent(guiActive = true, guiName = "Modded TestEvent", guiActiveEditor = true)]
#else
        [KSPEvent(guiActive = true, guiName = "TestEvent", guiActiveEditor = true)]
#endif
        public void TestMethod()
        {
#if MODIFIED
            print("Modified test method executes");
#else
            print("Test method executes");
#endif
        }
    }
}