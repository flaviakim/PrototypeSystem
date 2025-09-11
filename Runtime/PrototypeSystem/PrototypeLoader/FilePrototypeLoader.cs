using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace PrototypeSystem.PrototypeLoader {
    public abstract class FilePrototypeLoader<TData> : IPrototypeLoader<TData> where TData : IPrototypeData {
        private readonly string _rootFolder = PrototypeLoadingDefaultValues.RootFolder; 
        private readonly string _relativeFolder;
        protected string FullPath => Path.Combine(_rootFolder, _relativeFolder);
        
        public FilePrototypeLoader(string relativeFolder, string rootFolder = null) {
            _relativeFolder = relativeFolder;
            if (rootFolder != null) {
                _rootFolder = rootFolder;
            }
        }

        public abstract Dictionary<string, TData> LoadAll();
    }
}