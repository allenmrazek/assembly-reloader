using System;
using System.Reflection;
using ReeperCommon.Containers;
using ReeperCommon.Serialization;
using UnityEngine;

namespace TestProject
{
    [Serializable]
    public class TestSerialization : ISerializationCallbackReceiver
    {
        public string Value1;
        public string Value2;
        public ConfigNode node;
        public void OnBeforeSerialize()
        {
            UnityEngine.Debug.Log("TestSerialization.OnBeforeSerialize!");
        }

        public void OnAfterDeserialize()
        {
            UnityEngine.Debug.Log("TestSerialization/OnAfterDeserialize!");
        }
    }

//    [Serializable]
//    public class TestPartModule : PartModule, ISerializationCallbackReceiver
//    {

//        public ConfigNode TestConfig;
//        [SerializeField]
//        public TestSerialization TestTwo;
//        public string BasicTest;


//        public void OnBeforeSerialize()
//        {
//            UnityEngine.Debug.Log("TestPartModule.OnBeforeSerialize!");
//        }

//        public void OnAfterDeserialize()
//        {
//            UnityEngine.Debug.Log("TestPartModule.OnAfterDeserialize!");
//        }

//        //private ConfigNode local = new ConfigNode();

//        ////void Awake()
//        ////{
//        ////    print("TestPartModule awake");
//        ////}
//        public override void OnStart(StartState state)
//        {
//            base.OnStart(state);
//            print("TestPartModule (modified) OnStart: " + state);
//        }

//        public override void OnAwake()
//        {
//            base.OnAwake();

//            print("Loading level: " + Application.loadedLevelName);

//            if (Application.loadedLevelName == "ksploading")
//            {
//                TestConfig = new ConfigNode("PrefabConfigNode");
//                TestTwo = new TestSerialization {node = TestConfig, Value1 = "value 1", Value2 = "value 2"};
//                BasicTest = "BasicTestString";
//            }

//            print("Current ConfigNode: " + (TestConfig.Return(cfg => cfg.ToString(), "null")) + TestTwo.Return(ts => ts.Value1 + ", " + ts.node, " (testtwo is null)"));
//            print("Basic test: " + BasicTest);
//#if MODIFIED
//            print("TestPartModule awake (**MODIFIED** version)");
//#else
//            print("TestPartModule awake (unmodified version)");
//#endif
//            print("hello, testing world!");

//            print("TestPartModule running from " + Assembly.GetExecutingAssembly().CodeBase);
//            //print("TestPartModule is running from " +
//            //      (part == null || ReferenceEquals(part, part.partInfo.partPrefab) ? "prefab" : "clone"));
//            print("Done running checks");

//            //print("Our InstanceID: " + gameObject.GetInstanceID());
//            //print("Prefab InstanceID: " + part.partInfo.partPrefab.gameObject.GetInstanceID());

//            //Action<Part, int> printDetails = (p, i) => { };
//            //Action<Part, int> details = printDetails;

//            //printDetails = (p, i) => 
//            //{
//            //    print("Level " + i + " instanceID: " + p.gameObject.GetInstanceID());
//            //    print("Level " + i + " prefab ID: " + p.partInfo.partPrefab.gameObject.GetInstanceID());

//            //    if (p.parent != null)
//            //        details(p.parent, i + 1);
//            //};

//            //printDetails(part, 0);

//            //AssemblyLoader.loadedAssemblies.Where(la => la.dllName.StartsWith("TestProject")).ToList().ForEach(asm =>
//            //{
//            //    print("LoadedAssembly: " + asm.dllName);
//            //    foreach (var kvp in asm.types.ToDictionary(t => t.Key.FullName, items => items.Value).OrderBy(kvp => kvp.Key))
//            //    {
//            //        print("Type: " + kvp.Key);
//            //        foreach (var ty in kvp.Value)
//            //            print("       " + ty.FullName);
//            //    }
//            //});

//            //if (AssemblyLoader.loadedAssemblies == null)
//            //{
//            //    print("AssemblyLoader.list is null!");
//            //    return;
//            //}

//            //if (AssemblyLoader.loadedAssemblies.Any(la => la == null))
//            //{
//            //    print("An item in AssemblyLoader is null!");
//            //    return;
//            //}

//            //AssemblyLoader.loadedAssemblies.ToList().ForEach(la => print(la.dllName));

//            //if (HighLogic.LoadedSceneIsEditor)
//            //AssemblyLoader.loadedAssemblies
//            //    .Where(la => la.dllName.StartsWith("TestProject"))
//            //    .Select(la => la.types)
//            //    .ToList().ForEach(loadedTypes =>
//            //    {
//            //        foreach (var key in loadedTypes.Keys)
//            //            foreach (var ty in loadedTypes[key])
//            //                print(key.Name + ": " + ty.Name);
//            //    });

//        }

//        public override void OnLoad(ConfigNode node)
//        {
//           // print("TestPartModule running from " + Assembly.GetExecutingAssembly().CodeBase);
//            //print(string.Format("TestPartModule.OnLoad: {0}", node.ToString()));
//        }

//        public override void OnSave(ConfigNode node)
//        {
//            //print(string.Format("TestPartModule.OnSave: {0}", node.ToString()));

//            //var current = Assembly.GetExecutingAssembly();

//            //print("Hello, world!");
//            //print("Also, I'm running from CodeBase: " + current.CodeBase);
//            //print("Or Location: " + current.Location);

//            ////SomeMethod(local);
//        }

//        private void SomeMethod(ConfigNode node)
//        {
            
//        }

//        private void CheckLocationOfOtherAssembly()
//        {
//            var otherAssembly = typeof (IConfigNodeSerializer).Assembly;

//            print("Location of ReeperCommon: " + otherAssembly.Location);
//        }

//        //private static string ReturnCodeBaseLocation(Assembly assembly)
//        //{
//        //    if (ReferenceEquals(assembly, Assembly.GetExecutingAssembly()))
//        //        return "this location";

//        //    return assembly.CodeBase;
//        //}

//#if MODIFIED
//        [KSPEvent(guiActive = true, guiName = "Modded TestEvent", guiActiveEditor = true)]
//#else
//        [KSPEvent(guiActive = true, guiName = "TestEvent", guiActiveEditor = true)]
//#endif
//        public void TestMethod()
//        {
//#if MODIFIED
//            print("Modified test method executes");
//#else
//            print("Test method executes");
//#endif
//        }
    //}
}