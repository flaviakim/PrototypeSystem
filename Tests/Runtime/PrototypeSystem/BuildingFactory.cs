using System.IO;
using Tests.Runtime.PrototypeSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace PrototypeSystem.Tests.PrototypeSystem {
    
    public class BuildingFactory : InstanceFactoryBase<Building, BuildingPrototypeData, BuildingInitializationData> {
        protected override IPrototypeCollection<BuildingPrototypeData> PrototypeCollection { get; } = new ScriptableObjectPrototypeCollection<BuildingPrototypeData>();
        public override Building CreateInstance(BuildingPrototypeData prototype, BuildingInitializationData instanceData) {
            var building = new GameObject(prototype.IDName).AddComponent<Building>();
            building.Initialize(prototype, instanceData);
            return building;
        }
        
    }
}