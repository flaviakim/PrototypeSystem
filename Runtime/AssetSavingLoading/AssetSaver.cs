using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace AssetSavingLoading {
    public static class AssetSaver {

        public static string AssetPath {
            get => AssetSaverLoader.DefaultAssetPath;
            set => AssetSaverLoader.DefaultAssetPath = value;
        }

        public static bool TrySaveJson<T>(T jsonItem, string jsonPath, bool isOverwrite) {
            var jsonFullPath = Path.Combine(AssetPath, jsonPath);
            if (File.Exists(jsonFullPath) && !isOverwrite) {
                Debug.LogError($"Json file '{jsonFullPath}' already exists.");
                return false;
            }

            var json = JsonConvert.SerializeObject(jsonItem, Formatting.Indented);
            File.WriteAllText(jsonFullPath, json);
            Debug.Log($"Json file saved to '{jsonFullPath}'.");
            return true;
        }

    }
}