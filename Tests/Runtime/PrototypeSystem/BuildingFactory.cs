using System.IO;
using Tests.Runtime.PrototypeSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace PrototypeSystem.Tests.PrototypeSystem {
    
    public class BuildingFactory : InstanceFactory<Building, BuildingData, BuildingFactory> {
        protected override IPrototypeCollection<BuildingData> PrototypeCollection { get; } = new ScriptableObjectPrototypeCollection<BuildingData>();

        // public BuildingFactory() {
        //     //
        //     // var hutData = ScriptableObject.CreateInstance<BuildingData>();
        //     // if (hutData == null) {
        //     //     Debug.LogWarning($"Hut Data is null");
        //     //     return;
        //     // }
        //     // hutData.Floors = 1;
        //     // prototypeCollection.Add(hutData);
        //     //
        //     // PrototypeCollection = prototypeCollection;
        // }

        public Building CreateInstance(string idName) {
            var buildingData = PrototypeCollection.TryGetPrototypeForName(idName);
            if (buildingData == null) {
                Debug.LogError($"Couldn't find building with ID {idName}");
                return null;
            }

            var building = new GameObject("idName").AddComponent<Building>();
            building.Data = buildingData;
            return building;
        }
        
    }
}