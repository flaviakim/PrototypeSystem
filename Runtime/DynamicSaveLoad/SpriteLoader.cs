#nullable enable
using System.IO;
using UnityEngine;

namespace DynamicSaveLoad {
    public sealed class SpriteLoader : ISpriteLoader {
        private readonly string _basePath;
        private readonly SpriteCache _cache;

        public SpriteLoader(string basePath, SpriteCache? sharedCache = null) {
            _basePath = basePath;
            _cache = sharedCache ?? new SpriteCache();
        }

        public bool TryLoad(string relativePath, int pixelsPerUnit, out Sprite sprite, Vector2 pivot = default, bool cache = true) {
            sprite = null!;
            if (string.IsNullOrWhiteSpace(relativePath)) {
                Debug.LogError("Sprite path is null or empty.");
                return false;
            }

            if (cache && _cache.TryGet(relativePath, out sprite)) {
                return true;
            }

            var fullPath = Path.Combine(_basePath, relativePath);
            if (!File.Exists(fullPath)) {
                Debug.LogError($"Sprite file '{fullPath}' not found.");
                return false;
            }

            var bytes = File.ReadAllBytes(fullPath);
            var texture = new Texture2D(2, 2);
            if (!texture.LoadImage(bytes)) {
                Debug.LogError($"Failed to load texture from '{fullPath}'.");
                Object.Destroy(texture);
                return false;
            }

            sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), pivot, pixelsPerUnit);
            sprite.name = Path.GetFileNameWithoutExtension(relativePath);
            sprite.texture.filterMode = FilterMode.Point;
            sprite.texture.wrapMode = TextureWrapMode.Clamp;

            if (cache) _cache.AddOrReplace(relativePath, sprite);
            return true;
        }

        public void ClearCache() => _cache.Clear();

        internal SpriteCache GetCache() => _cache; // for sharing with async loader
    }
}