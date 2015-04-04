using System.Reflection;

namespace TestProject
{
    public class TestPartModule : PartModule
    {
        private ConfigNode local = new ConfigNode();

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

            print("TestPartModule running from " + Assembly.GetExecutingAssembly().CodeBase);
            print("TestPartModule is running from " +
                  (part == null || ReferenceEquals(part, part.partInfo.partPrefab) ? "prefab" : "clone"));

        }

        public override void OnLoad(ConfigNode node)
        {
            print("TestPartModule running from " + Assembly.GetExecutingAssembly().CodeBase);
            print(string.Format("TestPartModule.OnLoad: {0}", node.ToString()));
        }

        public override void OnSave(ConfigNode node)
        {
            print(string.Format("TestPartModule.OnSave: {0}", node.ToString()));

            var current = Assembly.GetExecutingAssembly();

            print("Hello, world!");
            print("Also, I'm running from " + current.CodeBase);

            SomeMethod(local);
        }

        private void SomeMethod(ConfigNode node)
        {
            
        }


        private static string ReturnCodeBaseLocation(Assembly assembly)
        {
            if (ReferenceEquals(assembly, Assembly.GetExecutingAssembly()))
                return "this location";

            return assembly.CodeBase;
        }

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