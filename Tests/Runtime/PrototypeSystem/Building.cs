using PrototypeSystem;
using UnityEngine;

namespace Tests.Runtime.PrototypeSystem {
    public class Building : MonoBehaviour, IInstance<BuildingData>
    {
        [SerializeField] private BuildingData buildingData;

        public BuildingData Data {
            get => buildingData;
            set => buildingData = value;
        }
    }
}