using UnityEngine;

namespace PrototypeSystem {
    public class MonoInstanceFactory<TInstance, TPrototypeData, TInitializationData> : InstanceFactoryBase<TInstance, TPrototypeData, TInitializationData> 
        where TInstance : MonoInstance<TPrototypeData, TInitializationData>
        where TPrototypeData : ScriptableObjectPrototypeData
        where TInitializationData : IInitializationData {
        
        private readonly GameObject _defaultPrefab;
        
        public MonoInstanceFactory(PrototypeCollection<TPrototypeData> prototypeCollection, GameObject defaultPrefab) : base(prototypeCollection) {
            _defaultPrefab = defaultPrefab;
        }

        public TInstance CreateInstance(string id, Vector3 position, TInitializationData initializationData, GameObject prefabOverride = null, Transform parent = null) {
            if (!PrototypeCollection.TryGetPrototypeForName(id, out TPrototypeData prototypeData)) {
                Debug.LogError($"MonoInstanceFactory: unknown id '{id}'");
                return null;
            }
            return CreateInstance(prototypeData, position, initializationData, prefabOverride, parent);
        }

        public TInstance CreateInstance(TPrototypeData prototypeData, Vector3 position, TInitializationData initializationData, GameObject prefabOverride = null, Transform parent = null) {
            GameObject prefab = prefabOverride ?? _defaultPrefab;
            GameObject go;
            if (prefab != null) {
                go = Object.Instantiate(prefab, position, Quaternion.identity, parent);
                go.name = $"{prefab.name}_{prototypeData.IDName}";
            } else {
                go = new GameObject($"Instance_{prototypeData.IDName}") {
                    transform = {
                        position = position,
                        parent = parent
                    }
                };
                // if (parent != null) go.transform.SetParent(parent, false);
            }

            TInstance instance = go.GetComponentInChildren<TInstance>(true) 
                                 ?? go.AddComponent<TInstance>();
            
            instance.Initialize(prototypeData, initializationData);
            
            return instance;
        }

        public override TInstance CreateInstance(TPrototypeData prototype, TInitializationData initializationData) {
            return CreateInstance(prototype, Vector3.zero, initializationData);
        }
        
    }
}