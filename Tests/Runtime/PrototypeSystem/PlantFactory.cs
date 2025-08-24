using System.IO;
using DynamicSaveLoad;
using UnityEngine;

namespace PrototypeSystem.Tests.PrototypeSystem {
    public class PlantFactory : InstanceFactory<Plant, PlantData, PlantFactory> {
        protected override IPrototypeCollection<PlantData> PrototypeCollection { get; } = new JsonPrototypeCollection<PlantData>("Plants", new DynamicAssetManager(Path.GetFullPath("Packages/PrototypeSystem/Tests/Runtime/Assets/")));
        
        public Plant CreateInstance(string idName) {
            if (!PrototypeCollection.TryGetPrototypeForName(idName, out PlantData plantData)) {
                Debug.LogError($"Couldn't find plant with ID {idName}");
                return null;
            }
            var plant = new Plant(plantData);
            return plant;
        }
    }
}