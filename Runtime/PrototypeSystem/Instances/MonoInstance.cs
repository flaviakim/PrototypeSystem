using UnityEngine;

namespace PrototypeSystem {
    public abstract class MonoInstance<TPrototypeData, TInitializationData> : MonoBehaviour, IInstance<TPrototypeData> 
        where TPrototypeData : ScriptableObjectPrototypeData
        where TInitializationData : IInitializationData {
        
        public abstract TPrototypeData PrototypeData { get; protected set; }

        public void Initialize(TPrototypeData prototypeData, TInitializationData initializationData) {
            PrototypeData = prototypeData;
            Initialize(initializationData);
        }

        protected abstract void Initialize(TInitializationData initializationData);
    }
}