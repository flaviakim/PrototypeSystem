/*
ModdableDataDrivenPackage.cs (Refactored)

This version updates the design to:
 - Use immutable (readonly) definition objects.
 - Runtime instances only store a reference to their Definition.
 - No unnecessary duplication of definition values.
 - Clean separation of static data (definitions) and dynamic state (instance).
 - Clear, clean code using SOLID principles.

*/

#define REQUIRE_NEWTONSOFT

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Moddable.DataDriven2 {

    #region Base Definition / Interfaces

    [Serializable]
    public abstract class BaseDefinition {
        [JsonProperty("id", Required = Required.Always)]
        public readonly string id;

        [JsonProperty("basedOn", NullValueHandling = NullValueHandling.Ignore)]
        public readonly string basedOn;

        protected BaseDefinition(string id, string basedOn) {
            this.id = id;
            this.basedOn = basedOn;
        }
    }

    public interface IInstanceWithDefinition<TDef> where TDef : BaseDefinition {
        TDef Definition { get; }
        void Initialize(TDef definition);
    }

    #endregion

    #region Registry

    public class DefinitionRegistry<TDef> where TDef : BaseDefinition {
        private readonly Dictionary<string, TDef> _dict = new(StringComparer.OrdinalIgnoreCase);

        public void AddOrReplace(TDef def) {
            if (def == null || string.IsNullOrEmpty(def.id))
                throw new ArgumentException("Definition or id is null");
            _dict[def.id] = def;
        }

        public bool TryGet(string id, out TDef def) => _dict.TryGetValue(id, out def);
        public IEnumerable<TDef> All => _dict.Values;
        public void Clear() => _dict.Clear();
    }

    #endregion

    #region DataLoader (supports inheritance via JObject)

    public class DataLoader<TDef> where TDef : BaseDefinition {
        private readonly string _relativeFolder;
        private readonly Dictionary<string, JObject> _rawJson = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, JObject> _mergedJsonCache = new(StringComparer.OrdinalIgnoreCase);

        public DataLoader(string relativeFolder) {
            _relativeFolder = relativeFolder;
        }

        public Dictionary<string, TDef> LoadAll() {
            _rawJson.Clear();
            _mergedJsonCache.Clear();

            string fullPath = Path.Combine(Application.streamingAssetsPath, _relativeFolder);
            if (!Directory.Exists(fullPath)) {
                Debug.LogWarning($"DataLoader: folder not found: {fullPath}");
                return new();
            }

            foreach (var file in Directory.GetFiles(fullPath, "*.json", SearchOption.AllDirectories)) {
                try {
                    var text = File.ReadAllText(file);
                    var j = JObject.Parse(text);
                    string id = j.Value<string>("id") ?? Path.GetFileNameWithoutExtension(file);
                    j["id"] = id;
                    _rawJson[id] = j;
                } catch (Exception ex) {
                    Debug.LogError($"DataLoader: failed to parse {file}: {ex}");
                }
            }

            var resolved = new Dictionary<string, TDef>(StringComparer.OrdinalIgnoreCase);
            foreach (var id in _rawJson.Keys) {
                try {
                    var merged = GetMergedJObject(id);
                    var obj = merged.ToObject<TDef>();
                    if (obj != null) resolved[id] = obj;
                } catch (Exception ex) {
                    Debug.LogError($"DataLoader: failed to resolve {id}: {ex}");
                }
            }

            return resolved;
        }

        private JObject GetMergedJObject(string id) {
            if (_mergedJsonCache.TryGetValue(id, out var cached)) return cached;
            if (!_rawJson.TryGetValue(id, out var j))
                throw new Exception($"Definition '{id}' not found in folder {_relativeFolder}");

            var basedOn = j.Value<string>("basedOn");
            JObject merged;
            if (!string.IsNullOrEmpty(basedOn)) {
                if (!_rawJson.ContainsKey(basedOn))
                    throw new Exception($"Base definition '{basedOn}' (used by '{id}') not found");
                var baseMerged = GetMergedJObject(basedOn);
                merged = (JObject)baseMerged.DeepClone();
                merged.Merge(j, new JsonMergeSettings { MergeArrayHandling = MergeArrayHandling.Replace });
            } else {
                merged = (JObject)j.DeepClone();
            }

            _mergedJsonCache[id] = merged;
            return merged;
        }
    }

    #endregion

    #region SpriteLoader

    public static class SpriteLoader {
        private static readonly Dictionary<string, Sprite> _cache = new(StringComparer.OrdinalIgnoreCase);

        public static IEnumerator LoadSpriteCoroutine(string relativePath, Action<Sprite> onComplete) {
            if (string.IsNullOrEmpty(relativePath)) { onComplete?.Invoke(null); yield break; }
            if (_cache.TryGetValue(relativePath, out var cached)) { onComplete?.Invoke(cached); yield break; }

            string fullPath = Path.Combine(Application.streamingAssetsPath, relativePath);
            string uri = new Uri(fullPath).AbsoluteUri;

            using var uwr = UnityWebRequestTexture.GetTexture(uri);
            yield return uwr.SendWebRequest();

            if (uwr.result != UnityWebRequest.Result.Success) {
                Debug.LogError($"SpriteLoader: failed to load {relativePath}: {uwr.error}");
                onComplete?.Invoke(null);
            } else {
                var tex = DownloadHandlerTexture.GetContent(uwr);
                var spr = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100f);
                _cache[relativePath] = spr;
                onComplete?.Invoke(spr);
            }
        }

        public static Sprite LoadSpriteFromFile(string relativePath) {
            if (string.IsNullOrEmpty(relativePath)) return null;
            if (_cache.TryGetValue(relativePath, out var cached)) return cached;

            string fullPath = Path.Combine(Application.streamingAssetsPath, relativePath);
            if (!File.Exists(fullPath)) {
                Debug.LogWarning($"SpriteLoader: file not found: {fullPath}");
                return null;
            }

            try {
                var bytes = File.ReadAllBytes(fullPath);
                var tex = new Texture2D(2, 2);
                tex.LoadImage(bytes);
                var spr = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100f);
                _cache[relativePath] = spr;
                return spr;
            } catch (Exception ex) {
                Debug.LogError($"SpriteLoader: failed to load {fullPath}: {ex}");
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
            if (!_registry.TryGet(id, out var def)) {
                Debug.LogError($"GenericFactory: unknown id '{id}'");
                return null;
            }

            var prefab = prefabOverride ?? _defaultPrefab;
            GameObject go;
            if (prefab != null) {
                go = GameObject.Instantiate(prefab, position, Quaternion.identity, parent);
                go.name = $"{prefab.name}_{id}";
            } else {
                go = new GameObject($"Instance_{id}");
                go.transform.position = position;
                if (parent != null) go.transform.SetParent(parent, false);
            }

            var components = go.GetComponentsInChildren<MonoBehaviour>(true);
            foreach (var comp in components) {
                if (comp is IInstanceWithDefinition<TDef> inst) {
                    inst.Initialize(def);
                }
            }

            return go;
        }
    }

    #endregion

    #region Tile & Unit Definitions

    [Serializable]
    public class TileDefinition : BaseDefinition {
        public readonly float movementSpeed;
        public readonly string sprite;

        [JsonConstructor]
        public TileDefinition(string id, string basedOn, float movementSpeed, string sprite)
            : base(id, basedOn) {
            this.movementSpeed = movementSpeed;
            this.sprite = sprite;
        }
    }

    public class TileInstance : MonoBehaviour, IInstanceWithDefinition<TileDefinition> {
        public TileDefinition Definition { get; private set; }
        private SpriteRenderer _sr;

        public void Initialize(TileDefinition def) {
            Definition = def ?? throw new ArgumentNullException(nameof(def));
            _sr = GetComponent<SpriteRenderer>() ?? gameObject.AddComponent<SpriteRenderer>();

            if (!string.IsNullOrEmpty(def.sprite)) {
                StartCoroutine(SpriteLoader.LoadSpriteCoroutine(def.sprite, sprite => {
                    if (sprite != null) _sr.sprite = sprite;
                }));
            }
        }

        public float MovementSpeed => Definition.movementSpeed;
    }

    [Serializable]
    public class UnitDefinition : BaseDefinition {
        public readonly float moveSpeed;
        public readonly int health;
        public readonly string sprite;

        [JsonConstructor]
        public UnitDefinition(string id, string basedOn, float moveSpeed, int health, string sprite)
            : base(id, basedOn) {
            this.moveSpeed = moveSpeed;
            this.health = health;
            this.sprite = sprite;
        }
    }

    public class UnitInstance : MonoBehaviour, IInstanceWithDefinition<UnitDefinition> {
        public UnitDefinition Definition { get; private set; }
        private SpriteRenderer _sr;

        public void Initialize(UnitDefinition def) {
            Definition = def ?? throw new ArgumentNullException(nameof(def));
            _sr = GetComponent<SpriteRenderer>() ?? gameObject.AddComponent<SpriteRenderer>();

            if (!string.IsNullOrEmpty(def.sprite)) {
                StartCoroutine(SpriteLoader.LoadSpriteCoroutine(def.sprite, sprite => {
                    if (sprite != null) _sr.sprite = sprite;
                }));
            }
        }

        public float MoveSpeed => Definition.moveSpeed;
        public int MaxHealth => Definition.health;
    }

    #endregion

    #region Map Loader

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
        public string mapsFolder = "Maps";
        public string mapFile = "map1.json";

        public GenericFactory<TileDefinition> tileFactory;
        public Transform tileParent;

        public IEnumerator LoadAndInstantiate() {
            string fullPath = Path.Combine(Application.streamingAssetsPath, mapsFolder, mapFile);
            if (!File.Exists(fullPath)) {
                Debug.LogError($"MapLoader: map file not found: {fullPath}");
                yield break;
            }

            var json = File.ReadAllText(fullPath);
            MapDefinition map;
            try { map = JsonConvert.DeserializeObject<MapDefinition>(json); } catch (Exception ex) {
                Debug.LogError($"MapLoader: failed to parse map: {ex}");
                yield break;
            }

            if (map?.tiles == null) yield break;

            foreach (var t in map.tiles) {
                Vector3 pos = new(t.x, t.y, 0);
                tileFactory?.Create(t.id, pos, null, tileParent);
            }
        }
    }

    #endregion

    #region Bootstrapper

    public class DataBootstrapper : MonoBehaviour {
        public string tilesFolder = "Definitions/Tiles";
        public GameObject tileDefaultPrefab;

        public string unitsFolder = "Definitions/Units";
        public GameObject unitDefaultPrefab;

        public DefinitionRegistry<TileDefinition> TileRegistry { get; private set; } = new();
        public DefinitionRegistry<UnitDefinition> UnitRegistry { get; private set; } = new();

        public GenericFactory<TileDefinition> TileFactory { get; private set; }
        public GenericFactory<UnitDefinition> UnitFactory { get; private set; }

        private IEnumerator Start() {
            var tileLoader = new DataLoader<TileDefinition>(tilesFolder);
            foreach (var kv in tileLoader.LoadAll()) TileRegistry.AddOrReplace(kv.Value);

            var unitLoader = new DataLoader<UnitDefinition>(unitsFolder);
            foreach (var kv in unitLoader.LoadAll()) UnitRegistry.AddOrReplace(kv.Value);

            TileFactory = new GenericFactory<TileDefinition>(TileRegistry, tileDefaultPrefab);
            UnitFactory = new GenericFactory<UnitDefinition>(UnitRegistry, unitDefaultPrefab);

            Debug.Log($"Bootstrapper: loaded {TileRegistry.All} tile defs, {UnitRegistry.All} unit defs");
            yield return null;
        }

        public GameObject SpawnTile(string id, Vector3 position, Transform parent = null) => TileFactory?.Create(id, position, null, parent);
        public GameObject SpawnUnit(string id, Vector3 position, Transform parent = null) => UnitFactory?.Create(id, position, null, parent);
    }

    #endregion
}
