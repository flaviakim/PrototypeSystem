using System;
using UnityEngine;

namespace PrototypeSystem {
    // [CreateAssetMenu(fileName = "New Prototype Data", menuName = "Prototype System/Prototype Data")]
    public abstract class ScriptableObjectPrototypeData : ScriptableObject, IPrototypeData {
        [SerializeField] private string idName;
        [SerializeField] private string basedOn;

        private void OnValidate() {
            if (string.IsNullOrEmpty(idName)) {
                Debug.LogWarning($"ScriptableObjectPrototypeData {name} idName is empty");
            }
        }

        public string IDName => idName;
        public string BasedOn => basedOn;
    }
}