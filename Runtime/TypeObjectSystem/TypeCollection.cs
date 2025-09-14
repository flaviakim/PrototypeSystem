using System;
using System.Collections.Generic;
using System.Diagnostics;
using TypeObjectSystem.TypeLoader;
using Debug = UnityEngine.Debug;

namespace TypeObjectSystem {
    public class TypeCollection<TType> : ITypeCollection<TType> where TType : IType {
        private readonly Dictionary<string, TType> _types = new(StringComparer.OrdinalIgnoreCase);

        public TypeCollection() {
        }

        public TypeCollection(params ITypeLoader<TType>[] typeLoaders) {
            LoadTypes(typeLoaders: typeLoaders);
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

        public void LoadTypes(bool clearExisting = true, params ITypeLoader<TType>[] typeLoaders) {
            if (typeLoaders == null) {
                Debug.LogError($"{nameof(LoadTypes)} called with null {nameof(typeLoaders)}");
                return;
            }

            if (clearExisting) {
                _types.Clear();
            }

            foreach (ITypeLoader<TType> typeLoader in typeLoaders) {
                Dictionary<string, TType> typeData = typeLoader.LoadAll();
                foreach (KeyValuePair<string, TType> pair in typeData) {
                    _types.Add(pair.Key, pair.Value);
                }
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
