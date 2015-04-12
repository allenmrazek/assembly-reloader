using System;
using System.IO;
using UnityEngine;

namespace TestProject
{
    [KSPAddon(KSPAddon.Startup.SpaceCentre, false)]
    public class AvoidKeyBindCollisions : MonoBehaviour
    {
        private KeyBinding myBinding = new KeyBinding();
        private string text = "";

        private void OnGUI()
        {
            Func<bool> validator = () => true;

            if (GUI.Button(new Rect(200f, 200f, 128f, 32f), "Set key"))
                PopupDialog.SpawnPopupDialog(new MultiOptionDialog("Press new KeyBinding key", DrawCallback,
                    "Set Binding",
                    new DialogOption("Cancel", OnCancel),
                    new DialogOption("Accept", OnAccept, validator, true)));
        }

        private void DrawCallback()
        {
            
        }

        private void OnCancel()
        {
            
        }

        private void OnAccept()
        {
            
        }

        private bool ValidateContents()
        {
            return true;
        }
    }

[KSPAddon(KSPAddon.Startup.SpaceCentre, false)]
public class AddonKeyBindingPersistence : MonoBehaviour
{
    private KeyBinding myBinding = new KeyBinding();


    private void Start()
    {
        if (File.Exists(GetConfigFilePath()))
            LoadSettings();
   
        print("myBinding is currently " + myBinding);
    }

    private void OnDestroy()
    {
        SaveSettings();
    }

    private void LoadSettings()
    {
        var config = ConfigNode.Load(GetConfigFilePath());
        myBinding.Load(config.GetNode("MyBinding") ?? DefaultKeyBindingNode()); // KeyBinding will throw an exception on badly formed ConfigNodes
    }

    private void SaveSettings()
    {
        var config = new ConfigNode("MyAddonSettings");
        myBinding.Save(config.AddNode("MyBinding"));
    }


    private string GetConfigFilePath()
    {
        var dllDir = Path.GetDirectoryName(AssemblyLoader.GetPathByType(typeof(AddonKeyBindingPersistence)));

        if (!Directory.Exists(dllDir))
            Directory.CreateDirectory(dllDir);

        return Path.Combine(dllDir, "settings.cfg");
    }

    private static ConfigNode DefaultKeyBindingNode()
    {
        var node = new ConfigNode();
        new KeyBinding(KeyCode.None).Save(node);
        return node;
    }
}

//[KSPScenario(ScenarioCreationOptions.AddToAllGames, new []{ GameScenes.SPACECENTER})]
//public class ScenarioModuleKeyBindingPersistence : ScenarioModule
//{
//    [KSPField(isPersistant = true)] // note: must be marked KSPField, persistent
//    private KeyBinding myBinding = new KeyBinding();


//    // note: none of the persistent fields have been set yet
//    public override void OnAwake()
//    {
//        base.OnAwake();
//    }

//    // persistent fields have been serialized by now
//    private void Start()
//    {
//        print("Start - KeyBindingPersistence - key is set to " + myBinding.primary);
//    }
//}

//    public class MyTestclass
//    {
//        public void Print()
//        {
//            Debug.Log("MyTestClass print");
//        }
//    }

//public class Example : MonoBehaviour
//{
//    public MyTestclass TestClass;
//    public string PublicString;
//    private string PrivateString;

//    private static bool RanOnce = false;

//    private void Awake()
//    {
//        if (!RanOnce)
//        {
//            PublicString = "Public string was set";
//            PrivateString = "Private string was set";
//            TestClass = new MyTestclass();

//            RanOnce = true;
//        }

//        print("PublicString: " + PublicString);
//        print("PrivateString: " + PrivateString);
//        print("TestClass: " + (TestClass == null ? "null" : "exists"));
//    }
//}

//    public class ColorChangingModule : PartModule
//    {


//        public static float scale = 21;


//        [KSPField(guiActiveEditor = true, guiName = "Red", guiFormat = "F2", isPersistant = true)]
//        [UI_FloatRange(minValue = 0, maxValue = 1, stepIncrement = .05f)]
//        public float red = 1f;
//        private float r;


//        [KSPField(guiActiveEditor = true, guiName = "Green", guiFormat = "F2", isPersistant = true)]
//        [UI_FloatRange(minValue = 0, maxValue = 1, stepIncrement = .05f)]
//        public float green = 1f;
//        private float g;


//        [KSPField(guiActiveEditor = true, guiName = "Blue", guiFormat = "F2", isPersistant = true)]
//        [UI_FloatRange(minValue = 0, maxValue = 1, stepIncrement = .05f)]
//        public float blue = 1f;
//        private float b;


//        [KSPField]
//        public string textureMeshName = "Variable";
//        public Mesh textureMesh;

//        [NonSerialized]
//        public Renderer texRend;
//        public Material mat;


//        [KSPField]
//        public bool newUV = true;


//        public float updateDelay = 0;


//        public override void OnAwake()
//        {
//            changeColor(true);
//            setUV(newUV);
//            base.OnAwake();

//            gameObject.AddComponent<Example>();
//        }


//        /// <summary>
//        /// Raises the start event.
//        /// </summary>
//        /// <param name="state">State.</param>
//        public override void OnStart(StartState state)
//        {
//            changeColor(true);
//            setUV(newUV);
//            base.OnStart(state);
//        }
//        /// <summary>
//        /// Standardizes the uv coords of the mesh.
//        /// </summary>
//        private void setUV(bool newUV)
//        {
//            //if (textureMesh == null)
//            //{
//            //    //textureMesh = part.FindModelComponent<MeshFilter>(textureMeshName).mesh;

//            //}
//            //if (newUV)
//            //{
//            //    Vector2[] uvs = new Vector2[textureMesh.vertices.Length];
//            //    for (int i = 0; i < uvs.Length; i++)
//            //    {
//            //        int j = i % 4;
//            //        switch (j)
//            //        {
//            //            case 0:
//            //                uvs[i] = Vector2.zero;
//            //                break;
//            //            case 1:
//            //                uvs[i] = Vector2.up;
//            //                break;
//            //            case 2:
//            //                uvs[i] = Vector2.right;
//            //                break;
//            //            case 3:
//            //                uvs[i] = Vector2.one;
//            //                break;
//            //        }
//            //    }
//            //    textureMesh.uv = uvs;
//            //}
//        }


//        /// <summary>
//        /// Called every frame.
//        /// </summary>
//        public void Update()
//        {
//            if (HighLogic.LoadedSceneIsEditor)
//            {
//                updateDelay -= Time.deltaTime;
//                if (updateDelay <= 0)
//                {
//                    changeColor();
//                    updateDelay = 0.02f;
//                }
//            }
//        }


//        /// <summary>
//        /// Changes the color of a mesh of a part implementing this PartModlue based on it's red, green, and blue values.
//        /// </summary>
//        public void changeColor(Boolean forceChange = false)
//        {
//            if (texRend == null)
//            {
//                //texRend = part.FindModelComponent<Renderer>(textureMeshName);
//                texRend = gameObject.GetComponentsInChildren<Renderer>().FirstOrDefault(rd => rd.sharedMaterial != null && rd.sharedMaterial.mainTexture != null);
//                mat = new Material(texRend.material.shader);
//            }
//            if (red != r | green != g | blue != b | forceChange)
//            {
//                r = red;
//                g = green;
//                b = blue;
//                Color color = new Color(r, g, b);
//                Texture2D tex = new Texture2D(10, 10);
//                Color[] colors = new Color[tex.height * tex.width];
//                for (int i = 0; i < colors.Length; i++)
//                {
//                    colors[i] = color;
//                }
//                tex.SetPixels(colors);
//                tex.Apply();
//                mat.mainTexture = tex;

//                if (ReferenceEquals(texRend, part.partInfo.partPrefab.GetComponent<ColorChangingModule>().texRend))
//                {
//                    print("They're the same renderer");
//                }
//                texRend.material = mat;

//                if (ReferenceEquals(texRend, part.partInfo.partPrefab.GetComponent<ColorChangingModule>().texRend))
//                {
//                    print("They're the same renderer");
//                }
//                if (forceChange)
//                {
//                    print("[VariableTexture]:Changing Color! " + color);
//                }
//            }
//        }
//    }




//    public class ColorChangingModule2 : PartModule
//    {
//        [KSPField(guiActiveEditor = true, guiName = "Red", guiFormat = "F2", isPersistant = true)]
//        [UI_FloatRange(minValue = 0, maxValue = 1, stepIncrement = .05f)]
//        public float Red = 1f;

//        [KSPField(guiActiveEditor = true, guiName = "Green", guiFormat = "F2", isPersistant = true)]
//        [UI_FloatRange(minValue = 0, maxValue = 1, stepIncrement = .05f)]
//        public float Green = 1f;

//        [KSPField(guiActiveEditor = true, guiName = "Blue", guiFormat = "F2", isPersistant = true)]
//        [UI_FloatRange(minValue = 0, maxValue = 1, stepIncrement = .05f)]
//        public float Blue = 1f;

//        [KSPField]
//        public string textureMeshName = "Variable";

//        private Material MaterialRef;

//        public override void OnLoad(ConfigNode node)
//        {
//            var target = part.FindModelComponent<Renderer>(textureMeshName);
//            if (target == null)
//            {
//                Debug.LogWarning(string.Format("Failed to find a renderer called '{0}'!", textureMeshName));
//                return;
//            }

//            UpdateColor();

//            if (HighLogic.LoadedSceneIsEditor)
//            {
//                GameEvents.onPartActionUICreate.Add(PartUIOpened);
//                GameEvents.onPartActionUIDismiss.Add(PartUIClosed);
//            }
//        }

//        private void OnDestroy()
//        {
//            if (HighLogic.LoadedSceneIsEditor)
//            {
//                GameEvents.onPartActionUICreate.Remove(PartUIOpened);
//                GameEvents.onPartActionUIDismiss.Add(PartUIClosed);
//            }
//        }


//        private void PartUIOpened(Part part)
//        {
//            if (!ReferenceEquals(part, this.part)) return;
//            StartCoroutine("UpdateColorWhileGuiOpen");
//        }

//        private void PartUIClosed(Part part)
//        {
//            if (!ReferenceEquals(part, this.part)) return;
//            StopCoroutine("UpdateColorWhileGuiOpen");
//        }


//        private IEnumerator UpdateColorWhileGuiOpen()
//        {
//            for (;;)
//            {
//                UpdateColor();
//                yield return 0;
//            }
//        }


//        private void UpdateColor()
//        {
 

//            target.material.color = new Color(Red, Green, Blue);
//        }
//    }
}