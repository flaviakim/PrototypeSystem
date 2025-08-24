using UnityEngine;

namespace PrototypeSystem {
    public class MonoInstanceFactory<TInstance, TPrototypeData, TInitializationData> : InstanceFactoryBase<TInstance, TPrototypeData, TInitializationData> 
        where TInstance : MonoInstance<TPrototypeData, TInitializationData>
        where TPrototypeData : ScriptableObjectPrototypeData
        where TInitializationData : IInitializationData {
        
        protected override IPrototypeCollection<TPrototypeData> PrototypeCollection { get; } =
            new ScriptableObjectPrototypeCollection<TPrototypeData>();
        
        public override TInstance CreateInstance(TPrototypeData prototype, TInitializationData initializationData) {
            var instance = new GameObject(prototype.IDName).AddComponent<TInstance>();
            instance.Initialize(prototype, initializationData);
            return instance;
        }
        
    }
}