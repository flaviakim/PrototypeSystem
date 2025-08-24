#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace DynamicSaveLoad {
    public sealed class JsonFileStorageAsync : IJsonStorageAsync {
        private readonly string _basePath;

        public JsonFileStorageAsync(string basePath) => _basePath = basePath;

        public async Task<T?> Load<T>(string relativePath) {
            var fullPath = Path.Combine(_basePath, relativePath);
            if (!File.Exists(fullPath)) {
                Debug.LogWarning($"JSON file '{fullPath}' not found.");
                return default;
            }

            await Awaitable.BackgroundThreadAsync();
            try {
                var json = await File.ReadAllTextAsync(fullPath).ConfigureAwait(false);
                var obj = JsonConvert.DeserializeObject<T>(json);
                await Awaitable.MainThreadAsync();
                return obj;
            }
            catch (JsonException ex) {
                await Awaitable.MainThreadAsync();
                Debug.LogError($"Failed to deserialize '{fullPath}': {ex.Message}");
                return default;
            }
        }

        public async Task<IReadOnlyList<T>> LoadAll<T>(string directory, bool recursive = true) {
            var dirFullPath = Path.Combine(_basePath, directory);
            if (!Directory.Exists(dirFullPath)) {
                Debug.LogWarning($"JSON directory '{dirFullPath}' does not exist.");
                return Array.Empty<T>();
            }

            await Awaitable.BackgroundThreadAsync();
            var files = Directory.GetFiles(dirFullPath, "*.json",
                recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);

            var list = new List<T>(files.Length);
            foreach (var file in files) {
                try {
                    var json = await File.ReadAllTextAsync(file).ConfigureAwait(false);
                    var obj = JsonConvert.DeserializeObject<T>(json);
                    if (obj != null) list.Add(obj);
                }
                catch {
                    // swallow and report after switch
                }
            }

            await Awaitable.MainThreadAsync();
            return list;
        }

        public async Task<bool> Save<T>(T item, string relativePath, bool overwrite = false) {
            var fullPath = Path.Combine(_basePath, relativePath);
            if (File.Exists(fullPath) && !overwrite) {
                Debug.LogWarning($"File '{fullPath}' already exists. Use overwrite=true to replace.");
                return false;
            }

            await Awaitable.BackgroundThreadAsync();
            try {
                var dir = Path.GetDirectoryName(fullPath);
                if (!string.IsNullOrEmpty(dir)) Directory.CreateDirectory(dir);

                var json = JsonConvert.SerializeObject(item, Formatting.Indented);
                await File.WriteAllTextAsync(fullPath, json).ConfigureAwait(false);
                await Awaitable.MainThreadAsync();
                return true;
            }
            catch (IOException ex) {
                await Awaitable.MainThreadAsync();
                Debug.LogError($"Failed to save '{fullPath}': {ex.Message}");
                return false;
            }
        }
    }
}