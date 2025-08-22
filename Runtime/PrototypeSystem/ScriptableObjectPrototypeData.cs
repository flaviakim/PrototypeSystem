using UnityEngine;

namespace PrototypeSystem {
    [CreateAssetMenu(fileName = "New Prototype Data", menuName = "Prototype System/Prototype Data")]
    public class ScriptableObjectPrototypeData : ScriptableObject, IPrototypeData {
        [SerializeField] private string idName;
        
        public string IDName => idName;
    }
}