using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Contracts.Agents.Mentalities;
using ReeperCommon.Extensions;
using ReeperCommon.Logging.Implementations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TestProject
{
    public class BiomeDustColorInfo
    {
        [Persistent]
        public string Name;

        [Persistent]
        public Color Color = Color.clear;
    }

    public class BodyDustColorInfo : IPersistenceLoad
    {
        [Persistent]
        public string Name;

        [Persistent]
        public Color DefaultColor = Color.clear;

        [Persistent]
        public List<BiomeDustColorInfo> BiomeColors = new List<BiomeDustColorInfo>();

        private Dictionary<string, Color> _colorDictionary = new Dictionary<string, Color>();
 
        public void PersistenceLoad()
        {
            _colorDictionary = BiomeColors.ToDictionary(ci => ci.Name, ci => ci.Color);
        }

        public Color GetDustColor(string name)
        {
            Color color;

            return _colorDictionary.TryGetValue(name, out color) ? color : DefaultColor;
        }
    }

    public class DustColorDefinitions : IPersistenceLoad, IPersistenceSave
    {
        [Persistent] private List<BodyDustColorInfo> BodyDustColors = new List<BodyDustColorInfo>();
        [Persistent] private Color DefaultColor = Color.clear;

        private Dictionary<CelestialBody,
            BodyDustColorInfo>
            _dustColors = new Dictionary<CelestialBody, BodyDustColorInfo>();


        public Color GetDustColor(CelestialBody body, double lat, double lon)
        {
            BodyDustColorInfo biomeColors;
            BiomeDustColorInfo dustColor;

            if (!_dustColors.TryGetValue(body, out biomeColors))
                return DefaultColor;

            var biome = FlightGlobals.ActiveVessel == null
                ? string.Empty
                : (string.IsNullOrEmpty(FlightGlobals.ActiveVessel.landedAt)
                    ? ScienceUtil.GetExperimentBiome(body, lat, lon)
                    : Vessel.GetLandedAtString(FlightGlobals.ActiveVessel.landedAt));

            return biomeColors.GetDustColor(biome);
        }


        public void PersistenceLoad()
        {
            _dustColors = BodyDustColors
                .Where(info => FlightGlobals.Bodies.Any(b => b.bodyName == info.Name))
                .ToDictionary(
                info => FlightGlobals.Bodies.Single(b => b.bodyName == info.Name), c => c);
        }


        public void PersistenceSave()
        {
            if (!BodyDustColors.Any())
            {
                // generate default config
                BodyDustColors = FlightGlobals.Bodies.Where(cb => cb.BiomeMap != null).Select(b => new BodyDustColorInfo
                {
                    DefaultColor = this.DefaultColor,
                    Name = b.bodyName,
                    BiomeColors = b.BiomeMap.Attributes
                        .GroupBy(attr => attr.name)
                        .Select(group => group.First())
                        .Select(attr => new BiomeDustColorInfo {Color = attr.mapColor, Name = attr.name}).ToList()
                }).ToList();
            }
        }
    }


    //[KSPAddon(KSPAddon.Startup.Flight, false)]
    public class SomeAddonOrScenarioModuleOrWhatever : MonoBehaviour
    {
        private DustColorDefinitions dustDefinitions = new DustColorDefinitions();

        private void Awake()
        {

            // save default config
            // ConfigNode.Save sometimes strips out the root node which could be a problem if it's empty,
            // leading to a blank cfg = crashes KSP loader
            var defaultConfig = ConfigNode.CreateConfigFromObject(dustDefinitions);
            defaultConfig.name = "DustColorDefinitions";

            System.IO.File.WriteAllText(KSPUtil.ApplicationRootPath + "GameData/DustColorDefinitions.cfg", defaultConfig.ToString());


            var definition = GameDatabase.Instance.GetConfigNodes("DustColorDefinitions").Single();

            if (!ConfigNode.LoadObjectFromConfig(dustDefinitions, definition))
                print("ERROR: failed to load dust definitions");
        }


        private void Update()
        {
            print("Current dust color: " +
                    dustDefinitions.GetDustColor(FlightGlobals.ActiveVessel.mainBody,
                        FlightGlobals.ActiveVessel.latitude, FlightGlobals.ActiveVessel.longitude));

            print("LandedAt: " + Vessel.GetLandedAtString(FlightGlobals.ActiveVessel.landedAt));
        }
    }


    //[KSPAddon(KSPAddon.Startup.EditorAny, false)]
    public class CheckRootModules : MonoBehaviour
    {
        

        private void Awake()
        {
            //if (EditorLogic.RootPart != null)
            //EditorLogic.RootPart.FindModulesImplementing<PartModule>()
            //    .ToList()
            //    .ForEach(m => print("Module: " + m.GetType().FullName));

 
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Keypad8))
            {
                print("Dumping loaded parts now ...");

                var loadedParts = Object.FindObjectsOfType(typeof(Part)).Cast<Part>().ToList();

                print(loadedParts.Count + " FindObjectsOfType returned");

                // Bit tricky here: loadedParts is looking for loose parts so if the parts are actually attached to each
                // other via parenting instead of joints (such as when building a ship in the editor), we'll only find
                // the top-level ones! Better look for children too
                loadedParts
                    .SelectMany(p => p.gameObject.GetComponentsInChildren<Part>(true))
                    .Where(p => !ReferenceEquals(p.gameObject, p.partInfo.partPrefab.gameObject))
                    .ToList()
                    .ForEach(p => print("Loaded part: " + p.name + ", " + p.flightID));
            }
        }
    }

}
