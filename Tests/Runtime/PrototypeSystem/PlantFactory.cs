using System.IO;
using DynamicSaveLoad;
using UnityEngine;

namespace PrototypeSystem.Tests.PrototypeSystem {
    public class PlantFactory : InstanceFactoryJson<Plant, PlantPrototypeData, IInitializationData.EmptyInitializationData> {
        public PlantFactory(string directoryPath, DynamicAssetManager assets) : base(directoryPath, assets) {
            
        }

        public override Plant CreateInstance(PlantPrototypeData prototype, IInitializationData.EmptyInitializationData initializationData) {
            var plant = new Plant(prototype, initializationData);
            return plant;
        }
    }
}