using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TestProject
{
//[KSPAddon(KSPAddon.Startup.EditorAny, true)]
//public class InitPartActionWindowTracker : MonoBehaviour
//{
//    private void Awake() { UIPartActionWindowTracker.Init(); }
//}

//[KSPAddon(KSPAddon.Startup.Flight, true)]
//public class FlightBootstrapper : InitPartActionWindowTracker
//{
//}

 
//public class UIPartActionWindowTracker
//{
//    private class WindowTrackerComponent : MonoBehaviour
//    {
//        private void Awake() { Add(GetComponent<UIPartActionWindow>()); }
//        private void OnDestroy() { Remove(GetComponent<UIPartActionWindow>()); }
//    }


//    public static readonly List<UIPartActionWindow> Windows = new List<UIPartActionWindow>();

//    public static void Init()
//    {
//        if (UIPartActionController.Instance.windowPrefab.gameObject.GetComponent<WindowTrackerComponent>() == null)
//            UIPartActionController.Instance.windowPrefab.gameObject.AddComponent<WindowTrackerComponent>();
//    }

//    private static void Add(UIPartActionWindow window)
//    {
//        if (window != null && !Windows.Contains(window))
//            Windows.Add(window);
//    }

//    private static void Remove(UIPartActionWindow window)
//    {
//        if (window != null && Windows.Contains(window))
//            Windows.Remove(window);
//    }
//}

//    [KSPAddon(KSPAddon.Startup.EditorAny, false)]
//    public class TestTrackedWindows : MonoBehaviour
//    {
//        private void Update()
//        {
//            if (Input.GetKeyDown(KeyCode.Keypad1))
//                foreach (var item in UIPartActionWindowTracker.Windows)
//                    print("item: " + item.name);
//        }
//    }

//    public class DustFX : PartModule
//    {
//        // Class-wide disabled warnings in SharpDevelop
//        // disable AccessToStaticMemberViaDerivedType
//        // disable RedundantDefaultFieldInitializer

//        [KSPField]
//        public bool dustEffects = true;
//        // Mostly unnecessary, since there is no other purpose to having the module active.
//        // Default: true

//        [KSPField]
//        public bool wheelImpact;
//        // Used to enable the impact effects.
//        // Default: false

//        [KSPField]
//        public float minScrapeSpeed = 1f;
//        // Minimum scrape speed.
//        // Default: 1
//        [KSPField]
//        public float minDustEnergy = 0f;
//        // Minimum dust energy value.
//        // Default: 0

//        [KSPField]
//        public float minDustEmission = 0f;
//        // Minimum emission value of the dust particles.
//        // Default: 0

//        [KSPField]
//        public float maxDustEnergyDiv = 10f;
//        // Maximum emission energy divisor.
//        // Default: 10

//        [KSPField]
//        public float maxDustEmissionMult = 2f;
//        // Maximum emission multiplier.
//        // Default: 2

//        [KSPField]
//        public float maxDustEmission = 35f;
//        // Maximum emission value of the dust particles.
//        // Default: 35

//        [KSPField]
//        public string wheelImpactSound = String.Empty;
//        // Path to the sound file that is to be used for impacts.
//        // Default: Empty

//        [KSPField]
//        public float minVelocityMag = 3f;
//        // Used in the OnCollisionEnter function to define the minimum velocity magnitude to check against.
//        // Default: 3

//        [KSPField]
//        public float pitchRange = 0.3f;
//        // Pitch-range that is used to vary the sound pitch.
//        // Default: 0.3

//        public const string effectsfxsmokeTraillight = "Effects/fx_smokeTrail_light";
//        FXGroup WheelImpactSound;

//        public string logprefix = "[DustFX]: ";
//        // Prefix the logs with this to identify it.

//        bool paused;
//        GameObject dustFx;
//        ParticleAnimator dustAnimator;
//        Color dustColor;


//        List<GameObject> emitterDiscardPile = new List<GameObject>();

//        /// <summary>
//        /// CollisionInfo class for the DustFX module.
//        /// </summary>
//        public class CollisionInfo
//        {
//            public DustFX DustFX;
//            public CollisionInfo(DustFX dustFX)
//            {
//                DustFX = dustFX;
//            }
//        }


//        public override void OnStart(StartState state)
//        {
//            if (Equals(state, StartState.Editor) || Equals(state, StartState.None))
//                return;

//            if (dustEffects)
//                SetupParticles();
//            if (wheelImpact && !String.IsNullOrEmpty(wheelImpactSound))
//                DustAudio();
//            GameEvents.onGamePause.Add(OnPause);
//            GameEvents.onGameUnpause.Add(OnUnpause);
//        }

//        /// <summary>
//        /// Defines the particle effects used in this module.
//        /// </summary>
//        void SetupParticles()
//        {
//            if (!dustEffects)
//                return;
//            dustFx = (GameObject)GameObject.Instantiate(Resources.Load(effectsfxsmokeTraillight));
//            dustFx.transform.parent = part.transform;
//            dustFx.transform.position = part.transform.position;
//            dustFx.particleEmitter.localVelocity = Vector3.zero;
//            dustFx.particleEmitter.useWorldSpace = true;
//            dustFx.particleEmitter.emit = false;
//            dustFx.particleEmitter.minEnergy = minDustEnergy;
//            dustFx.particleEmitter.minEmission = minDustEmission;
//            dustAnimator = dustFx.particleEmitter.GetComponent<ParticleAnimator>();
//        }

//        public void OnCollisionEnter(Collision c)
//        {
//            if (c.relativeVelocity.magnitude > minVelocityMag)
//            {
//                if (Equals(c.contacts.Length, 0))
//                    return;
//                var cInfo = GetClosestChild(part, c.contacts[0].point + (part.rigidbody.velocity * Time.deltaTime));
//                if (!Equals(cInfo.DustFX, null))
//                    cInfo.DustFX.DustImpact();
//                return;
//            }
//        }

//        /// <summary>
//        /// Searches child parts for the nearest instance of this class to the given point.
//        /// </summary>
//        /// <remarks>Parts with "physicsSignificance = 1" have their collisions detected by the parent part.
//        /// To identify which part is the source of a collision, check which part the collision is closest to.</remarks>
//        /// <param name="parent">The parent part whose children should be tested.</param>
//        /// <param name="p">The point to test the distance from.</param>
//        /// <returns>The nearest child part with a DustFX module, or null if the parent part is nearest.</returns>
//        static CollisionInfo GetClosestChild(Part parent, Vector3 p)
//        {
//            float parentDistance = Vector3.Distance(parent.transform.position, p);
//            float minDistance = parentDistance;
//            DustFX closestChild = null;
//            foreach (Part child in parent.children)
//            {
//                if (!Equals(child, null) && !Equals(child.collider, null) && (Equals(child.physicalSignificance, Part.PhysicalSignificance.NONE)))
//                {
//                    float childDistance = Vector3.Distance(child.transform.position, p);
//                    var cfx = child.GetComponent<DustFX>();
//                    if (!Equals(cfx, null) && childDistance < minDistance)
//                    {
//                        minDistance = childDistance;
//                        closestChild = cfx;
//                    }
//                }
//            }
//            return new CollisionInfo(closestChild);
//        }

//        public void Scrape(Collision c)
//        {
//            if ((paused || Equals(part, null)) || Equals(part.rigidbody, null))
//                return;
//            float m = c.relativeVelocity.magnitude;
//            DustParticles(m, c.contacts[0].point + (part.rigidbody.velocity * Time.deltaTime), c.collider);
//        }

//        void DustParticles(float speed, Vector3 contactPoint, Collider col)
//        {
//            if (!dustEffects)
//                return;
//            if (speed > minScrapeSpeed)
//            {
//                // Set dust biome colour.
//                if (!Equals(dustAnimator, null))
//                {
//                    //[s]Color BiomeColor = GetBiomeColour(col);[/s]
//                    Color BiomeColor = DustFXController.DustColors.GetDustColor(vessel.mainBody, vessel.latitude, vessel.longitude);
//                    if (!Equals(BiomeColor, dustColor))
//                    {
//                        Color[] colors = dustAnimator.colorAnimation;
//                        colors[0] = BiomeColor;
//                        colors[1] = BiomeColor;
//                        colors[2] = BiomeColor;
//                        colors[3] = BiomeColor;
//                        colors[4] = BiomeColor;
//                        dustAnimator.colorAnimation = colors;
//                        dustColor = BiomeColor;
//                    }
//                }
//                else
//                    return;

//                dustFx.transform.position = contactPoint;
//                dustFx.particleEmitter.maxEnergy = speed / maxDustEnergyDiv; 														// Values determined
//                dustFx.particleEmitter.maxEmission = Mathf.Clamp((speed * maxDustEmissionMult), minDustEmission, maxDustEmission); 	// via experimentation.
//                dustFx.particleEmitter.Emit();
//            }
//        }

//        //public string GetCurrentBiomeName ()
//        //{
//        //    CBAttributeMapSO biomeMap = FlightGlobals.currentMainBody.BiomeMap;
//        //    CBAttributeMapSO.MapAttribute mapAttribute = biomeMap.GetAtt(vessel.latitude * Mathf.Deg2Rad, vessel.longitude * Mathf.Deg2Rad);
//        //    return mapAttribute.name;
//        //}

//        // Color format: RGBA (0-1, decimal percentage)
//        //public Color genericDustColour = new Color (0.75f, 0.75f, 0.75f, 0.007f);
//        //// Grey 210 210 210
//        //public Color dirtColour = new Color (0.65f, 0.48f, 0.34f, 0.0125f);
//        //// Brown 165, 122, 88
//        //public Color lightDirtColour = new Color (0.65f, 0.52f, 0.34f, 0.0125f);
//        //// Brown 165, 132, 88
//        //public Color sandColour = new Color (0.80f, 0.68f, 0.47f, 0.0125f);
//        //// Light brown 203, 173, 119
//        //public Color snowColour = new Color (0.90f, 0.94f, 1f, 0.0125f);
//        //// Blue-white 230, 250, 255

//        //public Color GetBiomeColour ( Collider c )
//        //{
//        //    switch (FlightGlobals.ActiveVessel.mainBody.name)
//        //    {
//        //        case "Kerbin":
//        //            if (IsPQS(c))
//        //            {
//        //                string biome = GetCurrentBiomeName();
//        //                switch (biome)
//        //                {
//        //                    case "Water": //Anything here would be the sea-bottom.
//        //                        return lightDirtColour;
//        //                    case "Grasslands":
//        //                        return dirtColour;
//        //                    case "Highlands":
//        //                        return dirtColour;
//        //                    case "Shores":
//        //                        return lightDirtColour;
//        //                    case "Mountains":
//        //                        return dirtColour;
//        //                    case "Deserts":
//        //                        return sandColour;
//        //                    case "Badlands":
//        //                        return dirtColour;
//        //                    case "Tundra":
//        //                        return dirtColour;
//        //                    case "Ice Caps":
//        //                        return snowColour;
//        //                    default:
//        //                        return dirtColour;
//        //                }
//        //            }
//        //            return genericDustColour;
//        //        case "Duna":
//        //            return sandColour;
//        //        default:
//        //            return genericDustColour;
//        //    }
//        //}

//        public void OnCollisionStay(Collision c)
//        {
//            if (paused)
//                return;
//            var cInfo = DustFX.GetClosestChild(part, c.contacts[0].point + part.rigidbody.velocity * Time.deltaTime);
//            if (!Equals(cInfo.DustFX, null))
//                cInfo.DustFX.Scrape(c);
//            Scrape(c);
//        }

//        public bool IsPQS(Collider c)
//        {
//            if (Equals(c, null))
//                return false;
//            // Test for PQS: Name in the form "Ab0123456789".
//            Int64 n;
//            return Equals(c.name.Length, 12) && Int64.TryParse(c.name.Substring(2, 10), out n);
//        }

//        void OnPause()
//        {
//            paused = true;
//            dustFx.particleEmitter.enabled = false;
//            if (!Equals(WheelImpactSound, null) && !Equals(WheelImpactSound.audio, null))
//                WheelImpactSound.audio.Stop();
//        }

//        void OnUnpause()
//        {
//            paused = false;
//            dustFx.particleEmitter.enabled = true;
//        }

//        void OnDestroy()
//        {
//            if (!Equals(WheelImpactSound, null) && !Equals(WheelImpactSound.audio, null))
//                WheelImpactSound.audio.Stop();
//            GameEvents.onGamePause.Remove(OnPause);
//            GameEvents.onGameUnpause.Remove(OnUnpause);
//        }

//        void DustAudio()
//        {
//            WheelImpactSound = new FXGroup("WheelImpactSound");
//            part.fxGroups.Add(WheelImpactSound);
//            WheelImpactSound.audio = gameObject.AddComponent<AudioSource>();
//            WheelImpactSound.audio.clip = GameDatabase.Instance.GetAudioClip(wheelImpactSound);
//            WheelImpactSound.audio.dopplerLevel = 0f;
//            WheelImpactSound.audio.rolloffMode = AudioRolloffMode.Logarithmic;
//            WheelImpactSound.audio.Stop();
//            WheelImpactSound.audio.loop = false;
//            WheelImpactSound.audio.volume = GameSettings.SHIP_VOLUME;
//        }

//        public void DustImpact()
//        {
//            if (Equals(WheelImpactSound, null))
//            {
//                WheelImpactSound.audio.Stop();
//                return;
//            }
//            WheelImpactSound.audio.pitch = UnityEngine.Random.Range(1 - pitchRange, 1 + pitchRange);
//            WheelImpactSound.audio.Play();
//        }
//    }

//    public class DustColorDefinitions : IPersistenceLoad, IPersistenceSave
//    {
//        [Persistent]
//        List<BiomeDustColorInfo> BodyDustColors = new List<BiomeDustColorInfo>();

//        [Persistent]
//        public Color DefaultColor = Color.clear;

//        Dictionary<CelestialBody, BiomeDustColorInfo> _dustColors = new Dictionary<CelestialBody, BiomeDustColorInfo>();

//        public Color GetDustColor(CelestialBody body, double lat, double lon)
//        {
//            BiomeDustColorInfo biomeColors;

//            if (!_dustColors.TryGetValue(body, out biomeColors))
//                return DefaultColor;
//            var biome = Equals(FlightGlobals.ActiveVessel, null) ? string.Empty : (string.IsNullOrEmpty(FlightGlobals.ActiveVessel.landedAt) ? ScienceUtil.GetExperimentBiome(body, lat, lon) : Vessel.GetLandedAtString(FlightGlobals.ActiveVessel.landedAt));
//            return biomeColors.GetDustColor(biome);
//        }

//        public void PersistenceLoad()
//        {
//            _dustColors = BodyDustColors.Where(info => FlightGlobals.Bodies.Any(b => Equals(b.bodyName, info.Name))).ToDictionary(info => FlightGlobals.Bodies.Single(b => Equals(b.bodyName, info.Name)), c => c);
//        }

//        public void PersistenceSave()
//        {
//            if (!BodyDustColors.Any())
//            {
//                // generate default config
//                BodyDustColors = FlightGlobals.Bodies.Where(cb => !Equals(cb.BiomeMap, null)).Select(b => new BiomeDustColorInfo { DefaultColor = DefaultColor, Name = b.bodyName, BiomeColors = b.BiomeMap.Attributes.GroupBy(attr => attr.name).Select(group => group.First()).Select(attr => new BiomeDustColorInfo { Color = attr.mapColor, Name = attr.name }).ToList() }).ToList();
//            }
//        }
//    }

//    public class BiomeDustColorInfo
//    {
//        [Persistent]
//        public string Name;

//        [Persistent]
//        public Color Color = Color.clear;

//        [Persistent]
//        public Color DefaultColor = Color.clear;

//        [Persistent]
//        public List<BiomeDustColorInfo> BiomeColors = new List<BiomeDustColorInfo>();

//        Dictionary<string, Color> _colorDictionary = new Dictionary<string, Color>();

//        public void PersistenceLoad()
//        {
//            _colorDictionary = BiomeColors.ToDictionary(ci => ci.Name, ci => ci.Color);
//        }

//        public Color GetDustColor(string name)
//        {
//            Color color;
//            return _colorDictionary.TryGetValue(name, out color) ? color : DefaultColor;
//        }
//    }

//    public class DustFXController : MonoBehaviour
//    {
//        private static DustFXController _instance;
//        private readonly DustColorDefinitions _dustDefinitions = new DustColorDefinitions();

//        private void Awake()
//        {
//            _instance = this;

//            var definition = GameDatabase.Instance.GetConfigNodes("DustColorDefinitions");

//            if (!definition.Any() || !ConfigNode.LoadObjectFromConfig(_dustDefinitions, definition.Single()))
//                Debug.LogError("Failed to load dust color definitions!");
//        }

//        private void OnDestroy()
//        {
//            _instance = null;
//        }

//        public static DustColorDefinitions DustColors
//        {
//            get
//            {
//                _instance = _instance ?? new GameObject("DustFXController").AddComponent<DustFXController>();
//                return _instance._dustDefinitions;
//            }
//        }
//    }
}
