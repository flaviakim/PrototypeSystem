using System;
using System.Collections.Generic;
using System.Diagnostics;
using PrototypeSystem.PrototypeLoader;
using Debug = UnityEngine.Debug;

namespace PrototypeSystem {
    public class PrototypeCollection<TPrototypeData> where TPrototypeData : IPrototypeData {
        private readonly Dictionary<string, TPrototypeData> _prototypes = new(StringComparer.OrdinalIgnoreCase);
        
        private IPrototypeLoader<TPrototypeData> _prototypeLoader;
        public IPrototypeLoader<TPrototypeData> PrototypeLoader {
            get => _prototypeLoader;
            set {
                if (value == null) {
                    Debug.LogError($"prototypeLoader is null for PrototypeCollection of {typeof(TPrototypeData).Name}.");
                    return;
                }
                _prototypeLoader = value;
            }
        }

        public PrototypeCollection(IPrototypeLoader<TPrototypeData> prototypeLoader, bool initializeNow = true) {
            if (prototypeLoader == null) {
                Debug.LogError($"prototypeLoader is null for PrototypeCollection of {typeof(TPrototypeData).Name}.");
            }
            PrototypeLoader = prototypeLoader;
            if (initializeNow) {
                Initialize();
            }
        }
        
        public bool TryGetPrototypeForName(string idName, out TPrototypeData prototype) {
            CheckForErrors(idName);
            return _prototypes.TryGetValue(idName, out prototype);
        }

        public IEnumerable<string> GetPrototypeNames() {
            CheckIsInitialized();
            return _prototypes.Keys;
        }

        public IEnumerable<TPrototypeData> GetPrototypes() {
            CheckIsInitialized();
            return _prototypes.Values;
        }
        
        public void Initialize() {
            if (PrototypeLoader == null) {
                Debug.LogError($"prototypeLoader is null for PrototypeCollection of {typeof(TPrototypeData).Name}.");
                return;
            }
            _prototypes.Clear();
            Dictionary<string, TPrototypeData> prototypeData = PrototypeLoader.LoadAll();
            foreach (KeyValuePair<string, TPrototypeData> pair in prototypeData) {
                _prototypes.Add(pair.Key, pair.Value);
            }
        }
        
        public void AddOrReplace(TPrototypeData def) {
            if (def == null || string.IsNullOrEmpty(def.IDName)) {
                Debug.LogError($"Cannot add null or invalid prototype to collection of {typeof(TPrototypeData).Name}.");
                return;
            }
            _prototypes[def.IDName] = def;
        }

        public void Clear() {
            _prototypes.Clear();
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
            if (_prototypes.Count == 0) {
                Debug.LogError($"Prototype collection for {typeof(TPrototypeData).Name} is empty.");
            }
        }

    }
}