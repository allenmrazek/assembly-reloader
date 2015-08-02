using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace TestProject
{
//    [KSPAddon(KSPAddon.Startup.MainMenu, false)]
//    public class InstructorGreenScreen : MonoBehaviour, IPersistenceLoad, IPersistenceSave
//    {
//// ReSharper disable ConvertToConstant.Local
//// ReSharper disable FieldCanBeMadeReadOnly.Local
//        [Persistent] private int PortraitWidth = 128;
//        [Persistent] private int _portraitHeight = 128;
//        [Persistent] private PersistentRect _windowPosition = new PersistentRect(250f, 250f, 128f, 128f);
//        [Persistent] private int _selectedResponse = 0;
//        [Persistent] private int _selectedInstructor = 0;
//        [Persistent] private PersistentVector2 _scroll = Vector2.zero;

//        private KerbalInstructor _instructor;
//        private RenderTexture _portrait;
//        private Rect _windowRect;
//        private Dictionary<GUIContent, CharacterAnimationState> _responses;
//        private string[] _instructors; 



//        private void Start()
//        {
//            try
//            {
//                Resources.FindObjectsOfTypeAll<KerbalInstructor>()
//                    .Where(ki => AssetBase.GetPrefab(ki.name) != null)
//                    .ToList()
//                    .ForEach(
//                        instructor => print("Instructor: " + instructor.CharacterName + ", prefab: " + instructor.name));

//                if (!File.Exists(GetConfigPath()))
//                    print("No settings found for InstructorGreenScreen");
//                else
//                {
//                    print("Loading settings");
//                    var settings = ConfigNode.Load(GetConfigPath());
//                    print(settings.ToString());
//                    ConfigNode.LoadObjectFromConfig(this, settings);
//                }

//                _instructors = Resources.FindObjectsOfTypeAll<KerbalInstructor>()
//                    .Where(ki => AssetBase.GetPrefab(ki.name) != null)
//                    .Select(ki => ki.name)
//                    .ToArray();

//                _instructor = Create(_instructors[_selectedInstructor]);
//            }
//            catch (Exception e)
//            {
//                Debug.LogError("Failed to start InstructorGreenScreen due to following exception:");
//                Debug.LogException(e);
//                Destroy(this);
//                enabled = false;
//            }
//        }


//        private void OnDestroy()
//        {
//            //File.WriteAllText(
//            //    GetConfigPath(), 
//            //    ConfigNode.CreateConfigFromObject(this, new ConfigNode("InstructorGreenScreenSettings")).ToString(),
//            //    Encoding.UTF8);
//            ConfigNode.CreateConfigFromObject(this, new ConfigNode("InstructorGreenScreenSettings"))
//                .Save(GetConfigPath());

//            if (_portrait != null)
//                _portrait.Release();

//            if (_instructor != null)
//                Destroy(_instructor.gameObject);
//        }


//        private void OnGUI()
//        {
//            _windowRect = KSPUtil.ClampRectToScreen(GUILayout.Window(GetInstanceID(), _windowRect, DrawWindow, "Window Title",
//                HighLogic.Skin.window));
//        }


//        private void DrawWindow(int winid)
//        {
//            GUI.skin = HighLogic.Skin;

//            //_scroll = GUILayout.BeginScrollView(_scroll, GUIStyle.none);
//            {
//                // portrait area
//                GUILayout.BeginHorizontal();
//                {
//                    GUILayout.FlexibleSpace(); // to center portrait in case window stretches beyond portrait width
//                    GUILayoutUtility.GetRect(PortraitWidth, _portraitHeight);

//                    if (Event.current.type == EventType.Repaint)
//                        Graphics.DrawTexture(GUILayoutUtility.GetLastRect(), _portrait);

//                    GUILayout.FlexibleSpace();
//                }
//                GUILayout.EndHorizontal();

//                // response grid
//                _selectedResponse = GUILayout.SelectionGrid(_selectedResponse, _responses.Keys.ToArray(), 4);

//                if (GUI.changed)
//                    _instructor.PlayEmoteRepeating(_responses[_responses.Keys.ToArray()[_selectedResponse]], 1f);

//                GUILayout.Space(10f);
              
//                // instructor list
//                int nextInstructor = GUILayout.SelectionGrid(_selectedInstructor, _instructors, 4);
//                if (nextInstructor != _selectedInstructor)
//                {
//                    _selectedInstructor = nextInstructor;
//                    if (_instructor != null)
//                        Destroy(_instructor.gameObject);

//                    _instructor = Create(_instructors[_selectedInstructor]);
//                }
//            }
//            //GUILayout.EndScrollView();

//            GUI.DragWindow();
//        }


//        private KerbalInstructor Create(string instructorName)
//        {
//            var prefab = AssetBase.GetPrefab(instructorName);
//            if (prefab == null)
//                throw new ArgumentException("Could not find instructor named '" + instructorName + "'");

//            var prefabInstance = (GameObject)Instantiate(prefab);
//            var instructor = prefabInstance.GetComponent<KerbalInstructor>();

//            _portrait = _portrait ?? new RenderTexture(PortraitWidth, PortraitWidth, 8);
//            instructor.instructorCamera.targetTexture = _portrait;

//            _responses = instructor.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance)
//                .Where(fi => fi.FieldType == typeof(CharacterAnimationState))
//                .Where(fi => fi.GetValue(instructor) != null)
//                .ToDictionary(fi => new GUIContent(fi.Name), fi => fi.GetValue(instructor) as CharacterAnimationState);

//            return instructor;
//        }


//        public void PersistenceLoad()
//        {
//            _windowRect = _windowPosition;
//            print("PersistenceLoad: windowRect is now " + _windowRect + " while _windowPosition is " + _windowPosition);
//        }


//        public void PersistenceSave()
//        {
//            _windowPosition = _windowRect;
//        }


//        private string GetConfigPath()
//        {
//            return 
//                Path.Combine(
//                    Path.GetDirectoryName(
//                        AssemblyLoader.loadedAssemblies.GetByAssembly(Assembly.GetExecutingAssembly()).path) ?? string.Empty,
//                    "settings.cfg");
//        }
//    }


//    [Serializable]
//    public struct PersistentRect
//    {
//        [Persistent] public float X;
//        [Persistent] public float Y;
//        [Persistent] public float Width;
//        [Persistent] public float Height;

//        public PersistentRect(float x, float y, float width, float height)
//        {
//            X = x;
//            Y = y;
//            Width = width;
//            Height = height;
//        }


//        public override string ToString()
//        {
//            return string.Format("PersistentRect: X = {0}, Y = {1}, W = {2}, H = {3}", X, Y, Width, Height);
//        }

//        public static implicit operator Rect(PersistentRect pRect)
//        {
//            return new Rect {x = pRect.X, y = pRect.Y, width = pRect.Width, height = pRect.Height};
//        }

//        public static implicit operator PersistentRect(Rect rect)
//        {
//            return new PersistentRect {X = rect.x, Y = rect.y, Width = rect.width, Height = rect.height};
//        }
//    }


//    [Serializable]
//    public struct PersistentVector2
//    {
//        [Persistent] public float X;
//        [Persistent] public float Y;

//        public PersistentVector2(float x, float y)
//        {
//            X = x;
//            Y = y;
//        }

//        public static implicit operator Vector2(PersistentVector2 vector)
//        {
//            return new Vector2(vector.X, vector.Y);
//        }

//        public static implicit operator PersistentVector2(Vector2 from)
//        {
//            return new PersistentVector2(from.x, from.y);
//        }
//    }
}
