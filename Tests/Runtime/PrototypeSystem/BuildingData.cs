using PrototypeSystem;
using UnityEngine;

namespace Tests.Runtime.PrototypeSystem {
    [CreateAssetMenu(menuName = "Prototype System/Building Data", fileName = "New Building Data")]
    public class BuildingData : ScriptableObjectPrototypeData {
        [field:SerializeField] public int Floors { get; set; }
    }
}