#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace DynamicSaveLoad {
    public sealed class JsonFileStorage : IJsonStorage {
        private readonly string _basePath;

        public JsonFileStorage(string basePath) => _basePath = basePath;

        public bool TryLoad<T>(string relativePath, out T result) {
            var fullPath = Path.Combine(_basePath, relativePath);
            if (!File.Exists(fullPath)) {
                Debug.LogWarning($"JSON file '{fullPath}' not found.");
                result = default!;
                return false;
            }

            try {
                var json = File.ReadAllText(fullPath);
                result = JsonConvert.DeserializeObject<T>(json)!;
                return result != null;
            }
            catch (JsonException ex) {
                Debug.LogError($"Failed to deserialize '{fullPath}': {ex.Message}");
                result = default!;
                return false;
            }
        }

        public IReadOnlyList<T> LoadAll<T>(string directory, bool recursive = true) {
            var dirFullPath = Path.Combine(_basePath, directory);
            if (!Directory.Exists(dirFullPath)) {
                Debug.LogWarning($"JSON directory '{dirFullPath}' does not exist.");
                return Array.Empty<T>();
            }

            var files = Directory.GetFiles(dirFullPath, "*.json",
                recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);

            var list = new List<T>(files.Length);
            foreach (var file in files) {
                if (TryLoad<T>(Path.GetRelativePath(_basePath, file), out var item)) {
                    list.Add(item);
                }
            }
            return list;
        }

        public bool TrySave<T>(T item, string relativePath, bool overwrite = false) {
            var fullPath = Path.Combine(_basePath, relativePath);
            try {
                if (File.Exists(fullPath) && !overwrite) {
                    Debug.LogWarning($"File '{fullPath}' already exists. Use overwrite=true to replace.");
                    return false;
                }
                var dir = Path.GetDirectoryName(fullPath);
                if (!string.IsNullOrEmpty(dir)) Directory.CreateDirectory(dir);

                var json = JsonConvert.SerializeObject(item, Formatting.Indented);
                File.WriteAllText(fullPath, json);
                return true;
            }
            catch (IOException ex) {
                Debug.LogError($"Failed to save '{fullPath}': {ex.Message}");
                return false;
            }
        }
    }
}