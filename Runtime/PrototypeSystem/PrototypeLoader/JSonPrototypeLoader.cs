using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace PrototypeSystem.PrototypeLoader {
    public class JSonPrototypeLoader<TPrototypeData> : FilePrototypeLoader<TPrototypeData> where TPrototypeData : IPrototypeData {
        private readonly Dictionary<string, JObject> _rawJson = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, JObject> _mergedJsonCache = new(StringComparer.OrdinalIgnoreCase);

        public JSonPrototypeLoader(string relativeFolder, string rootFolder = null) : base(relativeFolder, rootFolder) { }
        
        public static IPrototypeCollection<TPrototypeData> CreatePrototypeCollection(string relativeFolder, string rootFolder = null) {
            return new PrototypeCollection<TPrototypeData>(new JSonPrototypeLoader<TPrototypeData>(relativeFolder, rootFolder));
        }

        public override Dictionary<string, TPrototypeData> LoadAll() {
            _rawJson.Clear();
            _mergedJsonCache.Clear();

            string fullPath = FullPath;
            if (!Directory.Exists(fullPath)) {
                Debug.LogWarning($"DataLoader: folder not found: {fullPath}");
                return new Dictionary<string, TPrototypeData>();
            }

            const string idVariableName = "IDName"; // case doesn't matter here
            
            foreach (string file in Directory.GetFiles(fullPath, "*.json", SearchOption.AllDirectories)) {
                try {
                    string text = File.ReadAllText(file);
                    JObject j = JObject.Parse(text);
                    bool hasID = j.TryGetValue(idVariableName, StringComparison.OrdinalIgnoreCase, out JToken idToken); // use TryGetValue to allow ignoring case.
                    string id = hasID ? idToken.ToString() : Path.GetFileNameWithoutExtension(file);
                    
                    if (!hasID) Debug.LogWarning($"{file} does not have a property {idVariableName}! Using file name as id: {id}");

                    j[idVariableName] = id;
                    _rawJson[id] = j;
                } catch (Exception ex) {
                    Debug.LogError($"DataLoader: failed to parse {file}: {ex}");
                }
            }

            var resolved = new Dictionary<string, TPrototypeData>(StringComparer.OrdinalIgnoreCase);
            foreach (string id in _rawJson.Keys) {
                try {
                    var merged = GetMergedJObject(id);
                    var obj = merged.ToObject<TPrototypeData>();
                    if (obj != null) resolved[id] = obj;
                } catch (Exception ex) {
                    Debug.LogError($"DataLoader: failed to resolve {id}: {ex}");
                }
            }

            Debug.Log($"Loaded {resolved.Count} prototypes of type {typeof(TPrototypeData).Name}: {string.Join(", ", resolved.Keys)}");
            
            return resolved;
        }
        
        private JObject GetMergedJObject(string id) {
            if (_mergedJsonCache.TryGetValue(id, out var cached)) return cached;
            if (!_rawJson.TryGetValue(id, out JObject j)) throw new Exception($"Definition '{id}' not found in folder {FullPath}");
            
            const string basedOnVariableName = "basedOn"; // case doesn't matter here

            bool hasBasedOn = j.TryGetValue(basedOnVariableName, StringComparison.OrdinalIgnoreCase, out JToken basedOnToken);
            string basedOn = hasBasedOn ? basedOnToken.ToString() : null;
            JObject merged;
            if (!string.IsNullOrEmpty(basedOn)) {
                if (!_rawJson.ContainsKey(basedOn))
                    throw new Exception($"Base definition '{basedOn}' (used by '{id}') not found");
                JObject baseMerged = GetMergedJObject(basedOn);
                merged = (JObject)baseMerged.DeepClone();
                merged.Merge(j, new JsonMergeSettings { MergeArrayHandling = MergeArrayHandling.Replace });
            } else {
                merged = (JObject)j.DeepClone();
            }
            
            _mergedJsonCache[id] = merged;
            return merged;
        }
        
    }
}