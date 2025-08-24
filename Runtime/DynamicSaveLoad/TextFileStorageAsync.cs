#nullable enable
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace DynamicSaveLoad {
    public sealed class TextFileStorageAsync : ITextStorageAsync {
        private readonly string _basePath;
        public TextFileStorageAsync(string basePath) => _basePath = basePath;

        public async Task<string> Load(string relativePath) {
            var fullPath = Path.Combine(_basePath, relativePath);
            if (!File.Exists(fullPath)) {
                Debug.LogWarning($"Text file '{fullPath}' not found.");
                return string.Empty;
            }

            await Awaitable.BackgroundThreadAsync();
            var text = await File.ReadAllTextAsync(fullPath).ConfigureAwait(false);
            await Awaitable.MainThreadAsync();
            return text;
        }
    }
}