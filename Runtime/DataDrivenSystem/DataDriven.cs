/*
ModdableDataDrivenPackage.cs

A single-file example implementation of a generic, data-driven, moddable core
that can be split into a Unity package.  This file contains:
 - README / usage notes
 - Sample JSON schemas (in comments)
 - Generic loader (uses Newtonsoft.Json for partial overrides/inheritance)
 - Definition registry
 - Generic factory that instantiates prefabs and applies definitions
 - Sprite loader (StreamingAssets based)
 - Example Tile and Unit definitions + appliers
 - Map loader example
 - Bootstrapper example

Requirements:
 - Unity 2019.4+ (coroutines & UnityWebRequest)
 - Newtonsoft.Json (install package com.unity.nuget.newtonsoft-json or include Json.NET)

Recommended folder layout inside a project or package:
Packages/com.yourname.moddable/
  Runtime/
    ModdableDataDrivenPackage.cs  <-- split into files later
  Samples~/StreamingAssets/Definitions/Tiles/*.json
  Samples~/StreamingAssets/Definitions/Units/*.json
  Samples~/StreamingAssets/Art/Tiles/*.png
  Samples~/StreamingAssets/Maps/*.json

How it works (high level):
 1. DataLoader<T> reads all JSON files from StreamingAssets/<relativeFolder>.
 2. JSON files may include "basedOn" referencing another definition; the loader
    merges JSON objects so mods may override only the fields they want.
 3. Definitions are registered into a DefinitionRegistry<T>.
 4. GenericFactory<T> creates instances (prefab or new GameObject) and finds
    components that implement IApplyDefinition<T> to populate runtime objects.
 5. SpriteLoader loads PNGs at runtime from StreamingAssets path (or cache).

Note on JSON merging / inheritance:
 - This implementation uses Newtonsoft.Json.Linq (JObject) to detect which
   properties are present in the override JSON. That allows "basedOn" behavior
   where the override only needs to contain changed fields.

SAMPLE JSON (tile) - put in StreamingAssets/Definitions/Tiles/grass.json:
{
  "id": "grass",
  "movementSpeed": 0.8,
  "sprite": "Art/Tiles/grass.png"
}

SAMPLE JSON (road) - put in StreamingAssets/Definitions/Tiles/road.json:
{
  "id": "road",
  "movementSpeed": 1.2,
  "sprite": "Art/Tiles/road.png"
}

SAMPLE JSON (swamp inherits grass) - StreamingAssets/Definitions/Tiles/swamp.json:
{
  "id": "swamp",
  "basedOn": "grass",
  "movementSpeed": 0.5,
  "sprite": "Art/Tiles/swamp.png"
}

SAMPLE MAP JSON - StreamingAssets/Maps/map1.json:
{
  "width": 4,
  "height": 3,
  "tiles": [
    {"x":0,"y":0,"id":"grass"},
    {"x":1,"y":0,"id":"road"},
    {"x":2,"y":0,"id":"road"},
    {"x":3,"y":0,"id":"grass"}
    ... 
  ]
}


================== CODE START ==================
*/

#define REQUIRE_NEWTONSOFT  // Remove this if you will rewrite loader to use JsonUtility

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

namespace Moddable.DataDriven {

    #region Base Definition / Interfaces

    [Serializable]
    public class BaseDefinition {
        public string id;
        public string basedOn; // optional
    }

    public interface IApplyDefinition<TDef> {
        void Apply(TDef def);
    }

    #endregion

    #region Registry

    public class DefinitionRegistry<TDef> where TDef : BaseDefinition {
        private readonly Dictionary<string, TDef> _dict = new();

        public void AddOrReplace(TDef def) {
            if (def == null || string.IsNullOrEmpty(def.id)) throw new ArgumentException("Definition or id is null");
            _dict[def.id] = def;
        }

        public bool TryGet(string id, out TDef def) => _dict.TryGetValue(id, out def);

        public IEnumerable<TDef> All => _dict.Values;

        public void Clear() => _dict.Clear();
    }

    #endregion

    #region DataLoader (uses JObject to support partial overrides)

    public class DataLoader<TDef> where TDef : BaseDefinition {
        private readonly string _rootFolder = Application.streamingAssetsPath;
        private readonly string _relativeFolder; // relative to StreamingAssets
        private readonly Dictionary<string, JObject> _rawJson = new Dictionary<string, JObject>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, JObject> _mergedJsonCache = new Dictionary<string, JObject>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, TDef> _resolved = new Dictionary<string, TDef>(StringComparer.OrdinalIgnoreCase);

        public DataLoader(string relativeFolder) {
            _relativeFolder = relativeFolder;
        }

        public Dictionary<string, TDef> LoadAll() {
            _rawJson.Clear();
            _mergedJsonCache.Clear();
            _resolved.Clear();

            string fullPath = Path.Combine(_rootFolder, _relativeFolder);
            if (!Directory.Exists(fullPath)) {
                Debug.LogWarning($"DataLoader: folder not found: {fullPath}");
                return new Dictionary<string, TDef>();
            }

            foreach (var file in Directory.GetFiles(fullPath, "*.json", SearchOption.AllDirectories)) {
                try {
                    var text = File.ReadAllText(file);
                    var j = JObject.Parse(text);
                    string id = j.Value<string>("id");
                    if (string.IsNullOrEmpty(id)) id = Path.GetFileNameWithoutExtension(file);
                    j["id"] = id; // ensure id is present
                    _rawJson[id] = j;
                } catch (Exception ex) {
                    Debug.LogError($"DataLoader: failed to parse {file}: {ex}");
                }
            }

            // build merged objects and resolved typed defs
            foreach (var id in new List<string>(_rawJson.Keys)) {
                try {
                    var merged = GetMergedJObject(id);
                    var obj = merged.ToObject<TDef>();
                    if (obj != null) _resolved[id] = obj;
                } catch (Exception ex) {
                    Debug.LogError($"DataLoader: failed to resolve {id}: {ex}");
                }
            }

            return new Dictionary<string, TDef>(_resolved);
        }

        private JObject GetMergedJObject(string id) {
            if (_mergedJsonCache.TryGetValue(id, out var cached)) return cached;
            if (!_rawJson.TryGetValue(id, out var j)) throw new Exception($"Definition '{id}' not found in folder {_relativeFolder}");

            var basedOn = j.Value<string>("basedOn");
            JObject merged;
            if (!string.IsNullOrEmpty(basedOn)) {
                // ensure base exists
                if (!_rawJson.ContainsKey(basedOn)) throw new Exception($"Base definition '{basedOn}' (used by '{id}') not found");
                var baseMerged = GetMergedJObject(basedOn);
                merged = (JObject)baseMerged.DeepClone();
                // overrides from j replace arrays and properties
                merged.Merge(j, new JsonMergeSettings { MergeArrayHandling = MergeArrayHandling.Replace });
            } else {
                merged = (JObject)j.DeepClone();
            }

            _mergedJsonCache[id] = merged;
            return merged;
        }
    }

    #endregion

    #region SpriteLoader (StreamingAssets)

    public static class SpriteLoader {
        private static readonly Dictionary<string, Sprite> Cache = new(StringComparer.OrdinalIgnoreCase);
        public static int PixelsPerUnit { get; set; } = 100;

        // Coroutine style loader - recommended for runtime
        public static IEnumerator LoadSpriteCoroutine(string relativePath, Action<Sprite> onComplete) {
            if (string.IsNullOrEmpty(relativePath)) { onComplete?.Invoke(null); yield break; }
            if (Cache.TryGetValue(relativePath, out var cached)) { onComplete?.Invoke(cached); yield break; }

            string fullPath = Path.Combine(Application.streamingAssetsPath, relativePath);
            string uri = new Uri(fullPath).AbsoluteUri; // works on all platforms

            using UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(uri);
#if UNITY_2020_1_OR_NEWER
            yield return uwr.SendWebRequest();
            if (uwr.result != UnityWebRequest.Result.Success) {
                Debug.LogError($"SpriteLoader: failed to load {relativePath}: {uwr.error}");
                onComplete?.Invoke(null);
            } else {
                Texture2D tex = DownloadHandlerTexture.GetContent(uwr);
                var spr = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), PixelsPerUnit);
                Cache[relativePath] = spr;
                onComplete?.Invoke(spr);
            }
#else
            yield return uwr.Send();
            if (!string.IsNullOrEmpty(uwr.error)) {
                Debug.LogError($"SpriteLoader: failed to load {relativePath}: {uwr.error}");
                onComplete?.Invoke(null);
            } else {
                var tex = DownloadHandlerTexture.GetContent(uwr);
                var spr = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100f);
                Cache[relativePath] = spr;
                onComplete?.Invoke(spr);
            }
#endif
        }

        // Synchronous loader (useful in editor or if you need immediate access)
        public static Sprite LoadSpriteFromFile(string relativePath) {
            if (string.IsNullOrEmpty(relativePath)) return null;
            if (Cache.TryGetValue(relativePath, out Sprite cached)) return cached;
            string fullPath = Path.Combine(Application.streamingAssetsPath, relativePath);
            if (!File.Exists(fullPath)) {
                Debug.LogWarning($"SpriteLoader: file not found: {fullPath}");
                return null;
            }
            try {
                byte[] bytes = File.ReadAllBytes(fullPath);
                var tex = new Texture2D(2, 2);
                tex.LoadImage(bytes);
                var spr = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), PixelsPerUnit);
                Cache[relativePath] = spr;
                return spr;
            } catch (Exception ex) {
                Debug.LogError($"SpriteLoader: failed to load file {fullPath}: {ex}");
                return null;
            }
        }
    }

    #endregion

    #region Generic Factory

    public class GenericFactory<TDef> where TDef : BaseDefinition {
        private readonly DefinitionRegistry<TDef> _registry;
        private readonly GameObject _defaultPrefab;

        public GenericFactory(DefinitionRegistry<TDef> registry, GameObject defaultPrefab = null) {
            _registry = registry;
            _defaultPrefab = defaultPrefab;
        }

        public GameObject Create(string id, Vector3 position, GameObject prefabOverride = null, Transform parent = null) {
            if (!_registry.TryGet(id, out TDef def)) {
                Debug.LogError($"GenericFactory: unknown id '{id}'");
                return null;
            }

            GameObject go;
            GameObject prefab = prefabOverride ?? _defaultPrefab;
            if (prefab != null) {
                go = Object.Instantiate(prefab, position, Quaternion.identity, parent);
                go.name = $"{prefab.name}_{id}";
            } else {
                go = new GameObject($"Instance_{id}") {
                    transform = {
                        position = position,
                        parent = parent
                    }
                };
                // if (parent != null) go.transform.SetParent(parent, false);
            }

            // Find components that implement IApplyDefinition<TDef>
            MonoBehaviour[] monos = go.GetComponentsInChildren<MonoBehaviour>(true);
            foreach (var m in monos) {
                if (m is IApplyDefinition<TDef> app) {
                    try { app.Apply(def); } catch (Exception ex) { Debug.LogError($"Apply failed on {m.GetType().Name}: {ex}"); }
                }
            }

            return go;
        }
    }

    #endregion

    #region Example Tile / Unit definitions + appliers

    [Serializable]
    public class TileDefinition : BaseDefinition {
        public float movementSpeed = 1f;
        public string sprite; // relative path inside StreamingAssets, e.g. "Art/Tiles/grass.png"
    }

    public class TileApplier : MonoBehaviour, IApplyDefinition<TileDefinition> {
        [Tooltip("Used at runtime to store current movement speed")]
        public float movementSpeed = 1f;

        [SerializeField]
        private SpriteRenderer sr;

        public void Apply(TileDefinition def) {
            if (def == null) {
                Debug.LogError($"TileApplier: cannot apply null definition");
                return;
            }

            movementSpeed = def.movementSpeed;
            if (sr == null) sr = GetComponent<SpriteRenderer>() ?? gameObject.AddComponent<SpriteRenderer>();

            if (!string.IsNullOrEmpty(def.sprite)) {
                // Start a coroutine to load sprite
                StartCoroutine(SpriteLoader.LoadSpriteCoroutine(def.sprite, sprite => {
                    if (sprite != null) sr.sprite = sprite;
                }));
            }
        }
    }

    [Serializable]
    public class UnitDefinition : BaseDefinition {
        public float moveSpeed = 1f;
        public int health = 10;
        public string sprite;
    }

    public class UnitApplier : MonoBehaviour, IApplyDefinition<UnitDefinition> {
        public float MoveSpeed = 1f;
        public int Health = 10;
        [SerializeField]
        private SpriteRenderer _sr;

        public void Apply(UnitDefinition def) {
            if (def == null) return;
            MoveSpeed = def.moveSpeed;
            Health = def.health;
            if (_sr == null) _sr = GetComponent<SpriteRenderer>() ?? gameObject.AddComponent<SpriteRenderer>();
            if (!string.IsNullOrEmpty(def.sprite)) {
                StartCoroutine(SpriteLoader.LoadSpriteCoroutine(def.sprite, sprite => {
                    if (sprite != null) _sr.sprite = sprite;
                }));
            }
        }
    }

    #endregion

    #region Map Loader example

    [Serializable]
    public class MapTileEntry {
        public int x;
        public int y;
        public string id;
    }

    [Serializable]
    public class MapDefinition {
        public int width;
        public int height;
        public List<MapTileEntry> tiles;
    }

    public class MapLoader : MonoBehaviour {
        [Tooltip("Relative path to maps folder inside StreamingAssets e.g. Maps")] public string mapsFolder = "Maps";
        [Tooltip("Map file name, e.g. map1.json")] public string mapFile = "map1.json";

        [Tooltip("Tile factory to use - assign at runtime or after bootstrap")]
        public GenericFactory<TileDefinition> tileFactory;

        [Tooltip("Parent transform for instantiated tiles")]
        public Transform tileParent;

        public IEnumerator LoadAndInstantiate() {
            string fullPath = Path.Combine(Application.streamingAssetsPath, mapsFolder, mapFile);
            if (!File.Exists(fullPath)) {
                Debug.LogError($"MapLoader: map file not found: {fullPath}");
                yield break;
            }

            string json = File.ReadAllText(fullPath);
            MapDefinition map;
            try { map = JsonConvert.DeserializeObject<MapDefinition>(json); } catch (Exception ex) { Debug.LogError($"MapLoader: failed to parse map: {ex}"); yield break; }
            if (map.tiles == null) yield break;

            foreach (var t in map.tiles) {
                Vector3 pos = new Vector3(t.x, t.y, 0);
                // Create tile via factory
                if (tileFactory != null) tileFactory.Create(t.id, pos, null, tileParent);
                else Debug.LogWarning("MapLoader: tileFactory not assigned");
                // optional: yield return null; to spread instantiation over frames
            }
        }
    }

    #endregion

    #region Bootstrapper example (editor or runtime attach to an object)

    public class DataBootstrapper : MonoBehaviour {
        [Header("Tile data")]
        public string tilesFolder = "Definitions/Tiles"; // relative to StreamingAssets
        public GameObject tileDefaultPrefab; // a prefab that has TileApplier attached

        [Header("Unit data")]
        public string unitsFolder = "Definitions/Units";
        public GameObject unitDefaultPrefab; // prefab that has UnitApplier attached

        // registries + factories (public for debug/inspection)
        public DefinitionRegistry<TileDefinition> TileRegistry { get; private set; } = new();
        public DefinitionRegistry<UnitDefinition> UnitRegistry { get; private set; } = new();

        public GenericFactory<TileDefinition> TileFactory { get; private set; }
        public GenericFactory<UnitDefinition> UnitFactory { get; private set; }

        IEnumerator Start() {
            // Load tile definitions
            var tileLoader = new DataLoader<TileDefinition>(tilesFolder);
            var tileDefs = tileLoader.LoadAll();
            TileRegistry.Clear();
            foreach (var kv in tileDefs) TileRegistry.AddOrReplace(kv.Value);

            // Load unit definitions
            var unitLoader = new DataLoader<UnitDefinition>(unitsFolder);
            var unitDefs = unitLoader.LoadAll();
            UnitRegistry.Clear();
            foreach (var kv in unitDefs) UnitRegistry.AddOrReplace(kv.Value);

            // create factories
            TileFactory = new GenericFactory<TileDefinition>(TileRegistry, tileDefaultPrefab);
            UnitFactory = new GenericFactory<UnitDefinition>(UnitRegistry, unitDefaultPrefab);

            // done
            Debug.Log($"Bootstrapper: loaded {tileDefs.Count} tile defs, {unitDefs.Count} unit defs");
            yield return null; // allow first frame to continue
        }

        // Example convenience method you can call from other code
        public GameObject SpawnTile(string id, Vector3 position, Transform parent = null) => TileFactory?.Create(id, position, null, parent);
        public GameObject SpawnUnit(string id, Vector3 position, Transform parent = null) => UnitFactory?.Create(id, position, null, parent);
    }

    #endregion

}

/*
================== CODE END ==================

Next steps & suggestions:
 - Split this single file into separate files per class (Runtime/Definitions, Runtime/Loading, Runtime/Factories, Runtime/Components, etc.).
 - Add editor tooling: a small window that shows which definitions are loaded, previews sprites, and validates mods.
 - Add hot-reload support for mod folders (watch for file changes and re-load definitions). When reloading, consider how to update live instances.
 - Consider using Addressables or AssetBundles if you want modders to ship large content packs.
 - Add unit tests for loader/merging logic.

*/
