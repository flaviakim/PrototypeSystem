using UnityEngine;

namespace PrototypeSystem.ScriptableObjectWrappers {
    public class MonoInstanceFactorySO<TInstance, TPrototypeData, TInitializationData> : InstanceFactorySO<TInstance, TPrototypeData, TInitializationData>
        where TInstance : MonoInstance<TPrototypeData, TInitializationData>
        where TPrototypeData : ScriptableObjectPrototypeData
        where TInitializationData : IInitializationData {
        
        [SerializeField] private GameObject defaultPrefab;

        protected override IInstanceFactory<TInstance, TPrototypeData, TInitializationData> CreateInnerFactory() {
            return new MonoInstanceFactory<TInstance, TPrototypeData, TInitializationData>(CreatePrototypeCollection(), defaultPrefab);
        }
        
    }
}