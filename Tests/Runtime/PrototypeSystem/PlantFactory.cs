using System.IO;
using DynamicSaveLoad;
using UnityEngine;

namespace PrototypeSystem.Tests.PrototypeSystem {
    public class PlantFactory : InstanceFactoryBase<Plant, PlantPrototypeData, IInitializationData.EmptyInitializationData> {
        public PlantFactory(PrototypeCollection<PlantPrototypeData> prototypeCollection) : base(prototypeCollection) {
        }

        public override Plant CreateInstance(PlantPrototypeData prototype, IInitializationData.EmptyInitializationData initializationData) {
            var plant = new Plant(prototype, initializationData);
            return plant;
        }
    }
}