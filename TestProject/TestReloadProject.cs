using System;
using System.Collections.Generic;
using System.Linq;
using ReeperCommon.Extensions;
using ReeperCommon.Logging;
using UnityEngine;

namespace TestProject
{
    [KSPAddon(KSPAddon.Startup.MainMenu, false)]
    public class TestReloadProject : MonoBehaviour
    {
        private Rect _windowRect = new Rect(000f, 000f, 200f, 200f);

        private void OnGUI()
        {
            _windowRect =
                KSPUtil.ClampRectToScreen(GUILayout.Window(GetInstanceID(), _windowRect, DrawWindow, "Test Window"));
        }

        private void DrawWindow(int winid)
        {
            GUILayout.BeginVertical();

            if (GUILayout.Button("Destroy This2"))
                Destroy(this);

            GUILayout.EndVertical();

            GUI.DragWindow();
        }

        void Start()
        {
            //gameObject.GetComponents<Component>().ToList().ForEach(c => print("Have component: " + c.name));
            //WriteString("TestReloadProject: " + typeof(TestReloadProject).FullName);

            //print("Printing components ...");
            //print("Also here's another message");
            //gameObject.PrintComponents(new DebugLog("TestReloadProject"));

        }

        //void OnUpdate()
        //{
        //    WriteString("TestReloadProject: " + typeof(TestReloadProject).FullName + " from " + Assembly.GetExecutingAssembly().Location);
        //    //var r = new SecondClass("secondclass create"); // to verify namespace change hasn't broken something internally
        //}

        //public static void WriteString(string str)
        //{
        //    //print(str);
        //}

        private void OnPluginReloadRequested()
        {
            print("TestReloadProject.OnPluginReloadRequested received!");
        }

        private void OnDestroy()
        {
            print("TestReloadProject.OnDestroy");
        }
    }
}
