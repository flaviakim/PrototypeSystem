using System.Collections.Generic;
using UnityEngine;

namespace PrototypeSystem {
    public abstract class ScriptableObjectPrototypeCollection<TPrototypeData> : MonoBehaviour, IPrototypeCollection<TPrototypeData> where TPrototypeData : ScriptableObjectPrototypeData {
        [SerializeField]
        private List<TPrototypeData> prototypeDataObjects = new();
        
        private readonly ScriptableObjectDictionaryPrototypeCollection _dictionaryCollection;

        public ScriptableObjectPrototypeCollection() {
            _dictionaryCollection = new ScriptableObjectDictionaryPrototypeCollection(this);
        }

        public void Add(TPrototypeData prototypeData) {
            prototypeDataObjects.Add(prototypeData);
        }
        
        public bool Remove(TPrototypeData prototypeData) {
            return prototypeDataObjects.Remove(prototypeData);
        }

        private class ScriptableObjectDictionaryPrototypeCollection : DictionaryPrototypeCollection<TPrototypeData> {
            private readonly ScriptableObjectPrototypeCollection<TPrototypeData> _scriptableObjectPrototypeCollection;
            public ScriptableObjectDictionaryPrototypeCollection(ScriptableObjectPrototypeCollection<TPrototypeData> scriptableObjectPrototypeCollection) {
                _scriptableObjectPrototypeCollection = scriptableObjectPrototypeCollection;
            }

            protected override Dictionary<string, TPrototypeData> LoadPrototypeDatas() {
                var prototypes = new Dictionary<string, TPrototypeData>();
                foreach (TPrototypeData prototype in _scriptableObjectPrototypeCollection.prototypeDataObjects) {
                    if (prototype == null) {
                        Debug.LogWarning($"Null prototype found in {_scriptableObjectPrototypeCollection.name}");
                        continue;
                    }
                    
                    if (string.IsNullOrEmpty(prototype.IDName)) {
                        Debug.LogWarning($"Prototype with empty IDName found in {_scriptableObjectPrototypeCollection.name}");
                        continue;
                    }
                    
                    if (!prototypes.TryAdd(prototype.IDName, prototype)) {
                        Debug.LogWarning($"Duplicate prototype IDName '{prototype.IDName}' found in {_scriptableObjectPrototypeCollection.name}");
                        continue;
                    }
                }

                return prototypes;
            }
        }

        public TPrototypeData TryGetPrototypeForName(string idName) {
            return _dictionaryCollection.TryGetPrototypeForName(idName);
        }

        public List<string> GetPrototypeNames() {
            return _dictionaryCollection.GetPrototypeNames();
        }

        public List<TPrototypeData> GetPrototypes() {
            return _dictionaryCollection.GetPrototypes();
        }

        public void PreloadPrototypes() {
            _dictionaryCollection.PreloadPrototypes();
        }
    }
}