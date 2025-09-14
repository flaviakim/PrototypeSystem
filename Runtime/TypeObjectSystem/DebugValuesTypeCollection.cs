using System;
using System.Collections.Generic;
using TypeObjectSystem.TypeLoader;
using UnityEngine;

namespace TypeObjectSystem {
    public abstract class DebugValuesTypeCollection<TType> : ITypeCollection<TType> where TType : IType {
        private readonly Dictionary<string, TType> _types = new(StringComparer.OrdinalIgnoreCase);
        
        protected abstract List<TType> GetDebugTypes();

        public bool TryGetTypeForName(string idName, out TType type) {
            return _types.TryGetValue(idName, out type);
        }

        public IEnumerable<string> GetTypeNames() {
            return _types.Keys;
        }

        public IEnumerable<TType> GetTypes() {
            return _types.Values;
        }

        public void LoadTypes(bool clearExisting = true, params ITypeLoader<TType>[] typeLoaders) {
            if (typeLoaders == null) {
                _types.Clear();
            }
            foreach (TType type in GetDebugTypes()) {
                _types.Add(type.IDName, type);
            }
            Debug.Log($"Initialized DebugValuesTypeCollection with {_types.Count} types of {typeof(TType).Name}.");
#if !DEBUG
            Debug.LogWarning("DebugValuesTypeCollection should only be used in debug builds.");
#endif
        }

        public void AddOrReplace(TType def) {
            if (def == null || string.IsNullOrEmpty(def.IDName)) {
                throw new ArgumentException("Type or its IDName is null or empty.");
            }
            _types[def.IDName] = def;
        }

        public void Clear() {
            _types.Clear();
        }
    }
}
