#nullable enable
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace DynamicSaveLoad {
    public sealed class SpriteLoaderAsync : ISpriteLoaderAsync {
        private readonly string _basePath;
        private readonly SpriteCache _cache;

        public SpriteLoaderAsync(string basePath, SpriteCache? sharedCache = null) {
            _basePath = basePath;
            _cache = sharedCache ?? new SpriteCache();
        }

        public async Task<Sprite?> Load(string relativePath, int pixelsPerUnit, Vector2 pivot = default, bool cache = true) {
            if (string.IsNullOrWhiteSpace(relativePath)) {
                Debug.LogError("Sprite path is null or empty.");
                return null;
            }

            if (cache && _cache.TryGet(relativePath, out var cached)) {
                return cached;
            }

            var fullPath = Path.Combine(_basePath, relativePath);
            if (!File.Exists(fullPath)) {
                Debug.LogError($"Sprite file '{fullPath}' not found.");
                return null;
            }

            await Awaitable.BackgroundThreadAsync();
            var bytes = await File.ReadAllBytesAsync(fullPath).ConfigureAwait(false);

            await Awaitable.MainThreadAsync();
            var texture = new Texture2D(2, 2);
            if (!texture.LoadImage(bytes)) {
                Debug.LogError($"Failed to load texture from '{fullPath}'.");
                Object.Destroy(texture);
                return null;
            }

            var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), pivot, pixelsPerUnit);
            sprite.name = Path.GetFileNameWithoutExtension(relativePath);
            sprite.texture.filterMode = FilterMode.Point;
            sprite.texture.wrapMode = TextureWrapMode.Clamp;

            if (cache) _cache.AddOrReplace(relativePath, sprite);
            return sprite;
        }

        public void ClearCache() => _cache.Clear();

        internal SpriteCache GetCache() => _cache; // for sharing with sync loader if needed
    }
}