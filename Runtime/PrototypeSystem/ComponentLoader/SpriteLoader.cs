using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace PrototypeSystem.ComponentLoader {
    // TODO improve with SpriteLoader(Async) in old DynamicSaveLoad
    public static class SpriteLoader {
        private static readonly Dictionary<string, Sprite> Cache = new(StringComparer.OrdinalIgnoreCase);
        public static int PixelsPerUnit { get; set; } = 100;

        public static IEnumerator LoadSpriteCoroutine(string relativePath, Action<Sprite> onComplete) {
            if (string.IsNullOrEmpty(relativePath)) {
                onComplete?.Invoke(null);
                yield break;
            }

            if (Cache.TryGetValue(relativePath, out Sprite cached)) {
                onComplete?.Invoke(cached);
                yield break;
            }

            string fullPath = Path.Combine(Application.streamingAssetsPath, relativePath);
            string uri = new Uri(fullPath).AbsoluteUri; // works on all platforms

            using UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(uri);
            yield return uwr.SendWebRequest();
            if (uwr.result != UnityWebRequest.Result.Success) {
                Debug.LogError($"SpriteLoader: failed to load {relativePath}: {uwr.error}");
                onComplete?.Invoke(null);
            }
            else {
                Texture2D tex = DownloadHandlerTexture.GetContent(uwr);
                tex.filterMode = FilterMode.Point;
                var sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), PixelsPerUnit);
                Cache[relativePath] = sprite;
                onComplete?.Invoke(sprite);
            }
        }

        // Synchronous loader (useful in editor or if you need immediate access)
        public static Sprite LoadSpriteFromFile(string relativePath) {
            Debug.Log($"LoadSpriteFromFile: {relativePath}");
            if (string.IsNullOrEmpty(relativePath)) return null;
            if (Cache.TryGetValue(relativePath, out Sprite cached)) return cached;
            string fullPath = Path.Combine(Application.streamingAssetsPath, relativePath);
            if (!File.Exists(fullPath)) {
                Debug.LogWarning($"SpriteLoader: file not found: {fullPath}");
                return null;
            }

            try {
                byte[] bytes = File.ReadAllBytes(fullPath);
                var tex = new Texture2D(2, 2);
                tex.LoadImage(bytes);
                tex.filterMode = FilterMode.Point;
                var sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), PixelsPerUnit);
                Cache[relativePath] = sprite;
                return sprite;
            }
            catch (Exception ex) {
                Debug.LogError($"SpriteLoader: failed to load file {fullPath}: {ex}");
                return null;
            }
        }
    }
}