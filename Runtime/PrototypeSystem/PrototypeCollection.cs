using System;
using System.Collections.Generic;
using PrototypeSystem.PrototypeLoader;
using UnityEngine;

namespace PrototypeSystem {
    public class PrototypeCollection<TPrototypeData> where TPrototypeData : IPrototypeData {
        private readonly Dictionary<string, TPrototypeData> _prototypes = new(StringComparer.OrdinalIgnoreCase);

        public PrototypeCollection(IPrototypeLoader<TPrototypeData> prototypeLoader) {
            Initialize(prototypeLoader);
        }

        public bool TryGetPrototypeForName(string idName, out TPrototypeData prototype) {
            return _prototypes.TryGetValue(idName, out prototype);
        }

        public IEnumerable<string> GetPrototypeNames() {
            return _prototypes.Keys;
        }

        public IEnumerable<TPrototypeData> GetPrototypes() {
            return _prototypes.Values;
        }

        public void Initialize(IPrototypeLoader<TPrototypeData> prototypeLoader) {
            _prototypes.Clear();
            Dictionary<string, TPrototypeData> prototypeData = prototypeLoader.LoadAll();
            foreach (KeyValuePair<string, TPrototypeData> pair in prototypeData) {
                _prototypes.Add(pair.Key, pair.Value);
            }
        }
        
        public void AddOrReplace(TPrototypeData def) {
            if (def == null || string.IsNullOrEmpty(def.IDName)) throw new ArgumentException("Definition or id is null");
            _prototypes[def.IDName] = def;
        }

        public void Clear() {
            _prototypes.Clear();
        }

    }
}