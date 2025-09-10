using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Serialization;

namespace PrototypeSystem.PrototypeLoader {
    public abstract class FilePrototypeLoader<TData> : IPrototypeLoader<TData> where TData : IPrototypeData {
        private readonly string _rootFolder = Application.streamingAssetsPath; 
        private readonly string _relativeFolder;
        protected string FullPath => Path.Combine(_rootFolder, _relativeFolder);
        
        public FilePrototypeLoader(string relativeFolder, string rootFolder = null) {
            _relativeFolder = relativeFolder;
            if (!string.IsNullOrEmpty(rootFolder)) {
                _rootFolder = rootFolder;
            }
        }

        public abstract Dictionary<string, TData> LoadAll();
    }
}