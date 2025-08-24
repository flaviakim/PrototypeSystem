using System.IO;
using DynamicSaveLoad;
using UnityEngine;

namespace PrototypeSystem.Tests.PrototypeSystem {
    public class PlantFactory : InstanceFactoryJson<Plant, PlantPrototypeData, IInitializationData> {
        public PlantFactory(string directoryPath, DynamicAssetManager assets) : base(directoryPath, assets) {
            
        }

        public override Plant CreateInstance(PlantPrototypeData prototype, IInitializationData initializationData) {
            var plant = new Plant();
            plant.Initialize(prototype, initializationData);
            return plant;
        }
    }
}