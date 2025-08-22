using UnityEngine;

namespace PrototypeSystem.Tests.PrototypeSystem {
    public class PlantFactory : InstanceFactory<Plant, PlantData, PlantFactory> {
        protected override IPrototypeCollection<PlantData> PrototypeCollection { get; } = new JsonPrototypeCollection<PlantData>("Plants");
        
        public Plant CreateInstance(string idName) {
            var plantData = PrototypeCollection.TryGetPrototypeForName(idName);
            if (plantData == null) {
                Debug.LogError($"Couldn't find plant with ID {idName}");
                return null;
            }
            var plant = new Plant(plantData);
            return plant;
        }
    }
}