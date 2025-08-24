#nullable enable
using System.IO;
using UnityEngine;

namespace DynamicSaveLoad {
    public sealed class TextFileStorage : ITextStorage {
        private readonly string _basePath;
        public TextFileStorage(string basePath) => _basePath = basePath;

        public bool TryLoad(string relativePath, out string text) {
            var fullPath = Path.Combine(_basePath, relativePath);
            if (!File.Exists(fullPath)) {
                Debug.LogWarning($"Text file '{fullPath}' not found.");
                text = string.Empty;
                return false;
            }
            text = File.ReadAllText(fullPath);
            return true;
        }
    }
}