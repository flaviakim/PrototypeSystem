#nullable enable
using System;
using UnityEngine;

namespace DynamicSaveLoad {
    /// <summary>
    /// Unified entry point exposing sync + async services.
    /// Share a single SpriteCache across sync/async loaders to avoid duplication.
    /// </summary>
    public sealed class DynamicAssetManager : IDisposable {
        public string RootPath { get; }

        public IJsonStorage Json { get; }
        public ITextStorage Text { get; }
        public ISpriteLoader Sprites { get; }

        public IJsonStorageAsync JsonAsync { get; }
        public ITextStorageAsync TextAsync { get; }
        public ISpriteLoaderAsync SpritesAsync { get; }

        private readonly SpriteCache _sharedSpriteCache = new();

        public DynamicAssetManager(string rootPath) {
            RootPath = rootPath;

            // Sync
            Json = new JsonFileStorage(RootPath);
            Text = new TextFileStorage(RootPath);
            Sprites = new SpriteLoader(RootPath, _sharedSpriteCache);

            // Async
            JsonAsync = new JsonFileStorageAsync(RootPath);
            TextAsync = new TextFileStorageAsync(RootPath);
            SpritesAsync = new SpriteLoaderAsync(RootPath, _sharedSpriteCache);

            Debug.Log($"Created DynamicAssetManager with root path {RootPath}");
        }

        public void ClearSpriteCache() => _sharedSpriteCache.Clear();

        public void Dispose() => _sharedSpriteCache.Dispose();
    }
}