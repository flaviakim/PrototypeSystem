#nullable enable
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace DynamicSaveLoad {

    public interface IJsonStorage {
        bool TryLoad<T>(string relativePath, out T result);
        IReadOnlyList<T> LoadAll<T>(string directory, bool recursive = true);
        bool TrySave<T>(T item, string relativePath, bool overwrite = false);
    }

    public interface IJsonStorageAsync {
        Task<T?> Load<T>(string relativePath);
        Task<IReadOnlyList<T>> LoadAll<T>(string directory, bool recursive = true);
        Task<bool> Save<T>(T item, string relativePath, bool overwrite = false);
    }

    public interface ITextStorage {
        bool TryLoad(string relativePath, out string text);
    }

    public interface ITextStorageAsync {
        Task<string> Load(string relativePath);
    }

    public interface ISpriteLoader {
        bool TryLoad(string relativePath, int pixelsPerUnit, out Sprite sprite, Vector2 pivot = default, bool cache = true);
        void ClearCache();
    }

    public interface ISpriteLoaderAsync {
        Task<Sprite?> Load(string relativePath, int pixelsPerUnit, Vector2 pivot = default, bool cache = true);
    }
}


