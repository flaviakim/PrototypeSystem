using System.IO;
using DynamicSavingLoading;
using JetBrains.Annotations;
using NUnit.Framework;
using UnityEngine;

namespace PrototypeSystem.Tests.PrototypeSystem {
    [TestFixture]
    [TestOf(typeof(JsonPrototypeCollection<>))]
    public class PrototypeSystemTest {
        const string TestAssetDirectoryName = "PlantTestAssets";
        const string BasePath = "Packages/PrototypeSystem/Tests/Runtime/Assets/";
        private readonly string _plant1Path = Path.Combine(TestAssetDirectoryName, "plant1.json");
        private readonly string _plant2Path = Path.Combine(TestAssetDirectoryName, "plant2.json");
        private const string Plant1IDName = "plant1";
        private const string Plant1Name = "Plant 1";
        private const string Plant1Description = "A beautiful plant.";
        private const string Plant2IDName = "plant2";
        private const string Plant2Name = "Plant 2";
        private const string Plant2Description = "A useful plant.";

        [SetUp]
        public void Setup() {
            DynamicLoader.DefaultDynamicAssetPath = Path.GetFullPath(BasePath);
        }
        
        [Test]
        public void TestCreatingAPlant() {
            var plantFactory = new PlantFactory();
            var plant = plantFactory.CreateInstance(Plant1IDName);
            Assert.IsNotNull(plant, "Plant data should not be null.");
            Assert.AreEqual(Plant1IDName, plant.IDName, "Plant IDName should match expected value.");
            Assert.AreEqual(plant.IDName, plant.Data.IDName, "Plant Data IDName should match plant IDName.");
            Assert.AreEqual(Plant1Name, plant.Data.Name, "Plant Name should match expected value.");
            Assert.AreEqual(Plant1Description, plant.Data.Description, "Plant Description should match expected value.");
            var plant2 = plantFactory.CreateInstance(Plant2IDName);
            Assert.IsNotNull(plant2, "Plant data should not be null.");
            Assert.AreEqual(Plant2IDName, plant2.IDName, "Plant IDName should match expected value.");
            Assert.AreEqual(plant2.IDName, plant2.Data.IDName, "Plant Data IDName should match plant IDName.");
            Assert.AreEqual(Plant2Name, plant2.Data.Name, "Plant Name should match expected value.");
            Assert.AreEqual(Plant2Description, plant2.Data.Description, "Plant Description should match expected value.");
        }
        
    }

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

    public class Plant : IInstance<PlantData> {
        public string IDName { get; }
        public PlantData Data { get; }

        public Plant(PlantData data) {
            Data = data;
            IDName = data.IDName;
        }
    }
    
    public class PlantData : PrototypeData {
        public PlantData(string idName, string name, string description) : base(idName) {
            Name = name;
            Description = description;
        }
        public string Name { get; }
        [CanBeNull] public string Description { get; }
    }
}