using PrototypeSystem;
using UnityEngine;

namespace Tests.Runtime.PrototypeSystem {
    [CreateAssetMenu(menuName = "Prototype System/Building Data", fileName = "New Building Data")]
    public class BuildingPrototypeData : ScriptableObjectPrototypeData {
        [field:SerializeField] public int Floors { get; set; }
        [field:SerializeField] public float InitialCleanliness { get; set; }
    }
}