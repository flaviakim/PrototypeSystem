using System;
using System.Collections.Generic;
using System.IO;
using DynamicSaveLoad;
using UnityEngine;

namespace PrototypeSystem {
    public class JsonPrototypeCollection<TPrototypeData> : DictionaryPrototypeCollection<TPrototypeData>
            where TPrototypeData : IPrototypeData {
        private readonly string _directoryPath;
        private readonly DynamicAssetManager _assets;

        public JsonPrototypeCollection(string directoryPath, DynamicAssetManager assets) {
            _directoryPath = directoryPath;
            _assets = assets ?? throw new ArgumentNullException(nameof(assets));
        }

        protected override Dictionary<string, TPrototypeData> LoadPrototypeDatas() {
            Debug.Log($"Loading prototypes from {_directoryPath}");
            var loadedPrototypes = new Dictionary<string, TPrototypeData>();

            var prototypeDatas = _assets.Json.LoadAll<TPrototypeData>(_directoryPath);
            if (prototypeDatas == null || prototypeDatas.Count == 0) {
                Debug.LogWarning($"No prototypes found in {_directoryPath}");
                return loadedPrototypes;
            }

            foreach (var prototypeData in prototypeDatas) {
                if (!IsValidPrototype(prototypeData, out string error)) {
                    Debug.LogError(error);
                    continue;
                }

                if (!loadedPrototypes.TryAdd(prototypeData.IDName, prototypeData)) {
                    Debug.LogError($"Duplicate prototype {prototypeData.IDName}");
                }
            }

            return loadedPrototypes;
        }

        public bool TrySavePrototypeData(TPrototypeData prototypeData, bool overwrite) {
            if (!IsValidPrototype(prototypeData, out string error)) {
                Debug.LogWarning(error);
                return false;
            }

            var path = Path.Combine(_directoryPath, prototypeData.IDName + ".json");
            if (!_assets.Json.TrySave(prototypeData, path, overwrite)) {
                Debug.LogError($"Failed to save prototype {prototypeData.IDName}");
                return false;
            }

            Debug.Log($"Saved prototype {prototypeData.IDName}");
            return true;
        }

        private static bool IsValidPrototype(TPrototypeData prototypeData, out string error) {
            if (prototypeData == null) {
                error = "Prototype data is null.";
                return false;
            }

            if (string.IsNullOrEmpty(prototypeData.IDName)) {
                error = "Prototype ID cannot be null or empty.";
                return false;
            }

            error = string.Empty;
            return true;
        }
    }
}