#nullable enable
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DynamicSaveLoad {
    /// <summary>
    /// Centralized cache so sync/async loaders share sprites and allow explicit disposal.
    /// Keys are relative paths (case-sensitive to match file system on most platforms).
    /// </summary>
    public sealed class SpriteCache : IDisposable {
        private readonly Dictionary<string, Sprite> _cache = new();

        public bool TryGet(string key, out Sprite sprite) => _cache.TryGetValue(key, out sprite);

        public void AddOrReplace(string key, Sprite sprite) {
            if (_cache.TryGetValue(key, out var old)) {
                if (old != null) {
                    if (old.texture != null) UnityEngine.Object.Destroy(old.texture);
                    UnityEngine.Object.Destroy(old);
                }
            }
            _cache[key] = sprite;
        }

        public void Clear() {
            foreach (var s in _cache.Values) {
                if (s != null) {
                    if (s.texture != null) UnityEngine.Object.Destroy(s.texture);
                    UnityEngine.Object.Destroy(s);
                }
            }
            _cache.Clear();
        }

        public void Dispose() => Clear();
    }
}