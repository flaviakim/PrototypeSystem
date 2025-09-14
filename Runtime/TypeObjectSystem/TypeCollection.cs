using System;
using System.Collections.Generic;
using System.Diagnostics;
using TypeObjectSystem.TypeLoader;
using Debug = UnityEngine.Debug;

namespace TypeObjectSystem {
    public class TypeCollection<TType> : ITypeCollection<TType> where TType : IType {
        private readonly Dictionary<string, TType> _types = new(StringComparer.OrdinalIgnoreCase);
        
        private ITypeLoader<TType> _typeLoader;
        public ITypeLoader<TType> TypeLoader {
            get => _typeLoader;
            set {
                if (value == null) {
                    Debug.LogError($"typeLoader is null for TypeCollection of {typeof(TType).Name}.");
                    return;
                }
                _typeLoader = value;
            }
        }

        public TypeCollection(ITypeLoader<TType> typeLoader, bool preloadTypes = true) {
            if (typeLoader == null) {
                Debug.LogError($"typeLoader is null for TypeCollection of {typeof(TType).Name}.");
            }
            TypeLoader = typeLoader;
            if (preloadTypes) {
                PreloadAll();
            }
        }
        
        public bool TryGetTypeForName(string idName, out TType type) {
            CheckForErrors(idName);
            return _types.TryGetValue(idName, out type);
        }

        public IEnumerable<string> GetTypeNames() {
            CheckIsInitialized();
            return _types.Keys;
        }

        public IEnumerable<TType> GetTypes() {
            CheckIsInitialized();
            return _types.Values;
        }
        
        public void PreloadAll() {
            if (TypeLoader == null) {
                Debug.LogError($"typeLoader is null for TypeCollection of {typeof(TType).Name}.");
                return;
            }
            _types.Clear();
            Dictionary<string, TType> typeData = TypeLoader.LoadAll();
            foreach (KeyValuePair<string, TType> pair in typeData) {
                _types.Add(pair.Key, pair.Value);
            }
        }
        
        public void AddOrReplace(TType def) {
            if (def == null || string.IsNullOrEmpty(def.IDName)) {
                Debug.LogError($"Cannot add null or invalid type to collection of {typeof(TType).Name}.");
                return;
            }
            _types[def.IDName] = def;
        }

        public void Clear() {
            _types.Clear();
        }
        
        [Conditional("DEBUG")]
        private void CheckForErrors(string idName) {
            if (string.IsNullOrEmpty(idName)) {
                Debug.LogError("idName is null or empty.");
            }
            CheckIsInitialized();
        }

        [Conditional("DEBUG")]
        private void CheckIsInitialized() {
            if (_types.Count == 0) {
                Debug.LogError($"Type collection for {typeof(TType).Name} is empty.");
            }
        }

    }
}