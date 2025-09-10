using UnityEngine;

namespace PrototypeSystem.ScriptableObjectWrappers {
    public class MonoInstanceFactorySO<TInstance, TPrototypeData, TInitializationData> : InstanceFactorySO<TInstance, TPrototypeData, TInitializationData, MonoInstanceFactory<TInstance, TPrototypeData, TInitializationData>>
        where TInstance : MonoInstance<TPrototypeData, TInitializationData>
        where TPrototypeData : ScriptableObjectPrototypeData
        where TInitializationData : IInitializationData {
        
        [SerializeField] private GameObject defaultPrefab;

        public TInstance CreateInstance(string id, Vector3 position, TInitializationData initializationData, GameObject prefabOverride = null, Transform parent = null) {
            return InnerFactory.CreateInstance(id, position, initializationData, prefabOverride, parent);
        }

        protected override MonoInstanceFactory<TInstance, TPrototypeData, TInitializationData> CreateInnerFactory() {
            return new MonoInstanceFactory<TInstance, TPrototypeData, TInitializationData>(CreatePrototypeCollection(), defaultPrefab);
        }
        
    }
}