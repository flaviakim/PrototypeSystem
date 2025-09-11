using System;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeSystem {
    public abstract class DebugValuesPrototypeCollection<TPrototypeData> : IPrototypeCollection<TPrototypeData> where TPrototypeData : IPrototypeData {
        private readonly Dictionary<string, TPrototypeData> _prototypes = new(StringComparer.OrdinalIgnoreCase);
        
        protected abstract List<TPrototypeData> GetDebugPrototypes();
        

        public bool TryGetPrototypeForName(string idName, out TPrototypeData prototype) {
            return _prototypes.TryGetValue(idName, out prototype);
        }

        public IEnumerable<string> GetPrototypeNames() {
            return _prototypes.Keys;
        }

        public IEnumerable<TPrototypeData> GetPrototypes() {
            return _prototypes.Values;
        }

        public void Initialize() {
            _prototypes.Clear();
            foreach (var prototype in GetDebugPrototypes()) {
                _prototypes.Add(prototype.IDName, prototype);
            }
            Debug.Log($"Initialized DebugValuesPrototypeCollection with {_prototypes.Count} prototypes of type {typeof(TPrototypeData).Name}.");
#if !DEBUG
            Debug.LogWarning("DebugValuesPrototypeCollection should only be used in debug builds.");
#endif
        }

        public void AddOrReplace(TPrototypeData def) {
            if (def == null || string.IsNullOrEmpty(def.IDName)) {
                throw new ArgumentException("Prototype data or its IDName is null or empty.");
            }
            _prototypes[def.IDName] = def;
        }

        public void Clear() {
            _prototypes.Clear();
        }
    }
}