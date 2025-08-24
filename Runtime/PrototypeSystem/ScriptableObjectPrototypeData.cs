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
}