using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace DynamicSavingLoading {
    public static class DynamicLoader {

        /// <summary>
        /// The place where all dynamic assets are stored.
        /// </summary>
        public static string DefaultDynamicAssetPath { get; set; } = Application.streamingAssetsPath;


        /// <summary>
        /// The path to the folder where all sprites are saved within the <see cref="DefaultDynamicAssetPath"/>.
        ///
        /// Can be set to "" if sprites are not organized into one base folder.
        /// </summary>
        public static string SpriteBasePath { get; set; } = "Sprites";
        
        /// <summary>
        /// Loads all JSON files from a directory and its subdirectories.
        /// The JSON files must be in the format of the specified type <typeparamref name="T"/>.
        /// Uses the <see cref="DefaultDynamicAssetPath"/> as the base path for the directory.
        /// Uses <see cref="LoadJson{T}(string,bool)"/> to load each JSON file.
        ///
        /// Returns null if the directory does not exist.
        /// Returns an empty list if no valid JSON files are found.
        /// </summary>
        /// <param name="directoryPath"> The relative path to the directory containing the JSON files, starting from <see cref="DefaultDynamicAssetPath"/>.</param>
        /// <param name="recursive"> If true, the method will also search in subdirectories.</param>
        /// <typeparam name="T"> The type of the objects to be loaded from the JSON files.</typeparam>
        /// <returns> A list of objects of type <typeparamref name="T"/> loaded from the JSON files, or null if the directory does not exist.</returns>
        public static List<T> LoadAllJson<T>(string directoryPath, bool recursive = true) {
            var jsonFullPath = Path.Combine(DefaultDynamicAssetPath, directoryPath);
            if (!Directory.Exists(jsonFullPath)) {
                Debug.LogError($"Json directory '{jsonFullPath}' does not exist.");
                return null!;
            }

            var files = Directory.GetFiles(jsonFullPath, "*.json", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            Debug.Log($"Found {files.Length} json files in {jsonFullPath}");
            var jsonList = new List<T>();
            foreach (var jsonFile in files) {
                var jsonItem = LoadJson<T>(jsonFile);
                if (jsonItem == null) {
                    Debug.LogWarning($"Failed to load json item from file '{jsonFile}' as type '{typeof(T).Name}'.");
                    continue;
                }
                jsonList.Add(jsonItem);
            }
            
            return jsonList;
        }

        /// <summary>
        /// Loads a JSON file from the specified path and deserializes it into an object of type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="jsonPath"> The relative path to the JSON file, starting from <see cref="DefaultDynamicAssetPath"/>.</param>
        /// <param name="isRelativeToDefaultPath"> If true, the path is considered relative to <see cref="DefaultDynamicAssetPath"/>. If false, the path is treated as an absolute path.</param>
        /// <typeparam name="T"> The type of the object to be deserialized from the JSON file.</typeparam>
        /// <returns> An object of type <typeparamref name="T"/> deserialized from the JSON file, or default if the file does not exist or cannot be deserialized.</returns>
        public static T LoadJson<T>(string jsonPath, bool isRelativeToDefaultPath = true) {
            var jsonFullPath = isRelativeToDefaultPath ? Path.Combine(DefaultDynamicAssetPath, jsonPath)
                : jsonPath;
            if (!File.Exists(jsonFullPath)) {
                Debug.LogError($"Json file '{jsonFullPath}' does not exist.");
                return default!;
            }

            var json = File.ReadAllText(jsonFullPath);
            return JsonConvert.DeserializeObject<T>(json);
        }
    
        private static readonly Dictionary<string, Sprite> Sprites = new();

        public static Sprite LoadSprite(string spritePath, int pixelsPerUnit, Vector2 pivot = new(), bool cache = true) {
            if (string.IsNullOrEmpty(spritePath)) {
                Debug.LogError("Sprite path is null or empty.");
                return null!;
            }

            var spritesPathWithBase = spritePath.StartsWith(SpriteBasePath) 
                ? spritePath // in case the Sprite Base Path has already been added manually.
                : Path.Combine(SpriteBasePath, spritePath);
        
            if (cache && Sprites.TryGetValue(spritesPathWithBase, out var cachedSprite)) {
                return cachedSprite;
            }

            var spriteFullPath = Path.Combine(Application.streamingAssetsPath, spritesPathWithBase);
            
            if (!File.Exists(spriteFullPath)) {
                Debug.LogError($"Sprite file '{spriteFullPath}' does not exist.");
                return null!;
            }

            var bytes = File.ReadAllBytes(spriteFullPath);
            var texture = new Texture2D(1, 1);
            texture.LoadImage(bytes);
            var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), pivot, pixelsPerUnit);
            sprite.name = Path.GetFileNameWithoutExtension(spritesPathWithBase);
            sprite.texture.filterMode = FilterMode.Point;
            sprite.texture.wrapMode = TextureWrapMode.Clamp;
            sprite.texture.Apply();
            Debug.Log($"Sprite '{sprite.name}' loaded from '{spriteFullPath}'.");

            if (cache) {
                Sprites[spritesPathWithBase] = sprite; // Cache the loaded sprite
            }

            return sprite;
        }

        public static string LoadText(string textPath) {
            var textFullPath = Path.Combine(Application.streamingAssetsPath, textPath);
            if (!File.Exists(textFullPath)) {
                Debug.LogError($"Text file '{textFullPath}' does not exist.");
                return null!;
            }

            return File.ReadAllText(textFullPath);
        }

    }
}