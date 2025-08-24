using System;
using PrototypeSystem;
using UnityEngine;

namespace Tests.Runtime.PrototypeSystem {
    public class Building : MonoInstance<BuildingPrototypeData, BuildingInitializationData>
    {
        [SerializeField] private BuildingPrototypeData buildingPrototypeData;

        public Vector2Int Position { get; private set; }

        public override BuildingPrototypeData PrototypeData {
            get => buildingPrototypeData;
            protected set => buildingPrototypeData = value;
        }

        protected override void Initialize(BuildingInitializationData initializationData) {
            Position = initializationData.Position;
        }
    }

    public class BuildingInitializationData : IInitializationData {
        public BuildingInitializationData(Vector2Int position) {
            Position = position;
        }

        public Vector2Int Position { get; }
    }
}