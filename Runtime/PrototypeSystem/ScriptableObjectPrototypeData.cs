using System;
using UnityEngine;

namespace PrototypeSystem {
    [CreateAssetMenu(fileName = "New Prototype Data", menuName = "Prototype System/Prototype Data")]
    public class ScriptableObjectPrototypeData : ScriptableObject, IPrototypeData {
        [SerializeField] private string idName;

        private void OnValidate() {
            if (string.IsNullOrEmpty(idName)) {
                Debug.LogWarning($"ScriptableObjectPrototypeData {name} idName is empty");
            }
        }

        public string IDName => idName;
    }

    public abstract class MonoInstance<TPrototypeData, TInitializationData> : MonoBehaviour, IInstance<TPrototypeData, TInitializationData> 
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