namespace TestProject
{
    public class TestPartModule : PartModule
    {
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
        }

        public override void OnLoad(ConfigNode node)
        {
            print(string.Format("TestPartModule.OnLoad: {0}", node.ToString()));
        }

        public override void OnSave(ConfigNode node)
        {
            print(string.Format("TestPartModule.OnSave: {0}", node.ToString()));
        }
    }
}