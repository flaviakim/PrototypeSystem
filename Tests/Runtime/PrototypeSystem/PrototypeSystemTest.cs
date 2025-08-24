using System.IO;
using DynamicSaveLoad;
using NUnit.Framework;
using Tests.Runtime.PrototypeSystem;
using UnityEngine;

namespace PrototypeSystem.Tests.PrototypeSystem {
    [TestFixture]
    [TestOf(typeof(JsonPrototypeCollection<>))]
    public class PrototypeSystemTest {
        const string TestAssetDirectoryName = "Plants";
        const string BasePath = "Packages/PrototypeSystem/Tests/Runtime/Assets/";
        private readonly string _plant1Path = Path.Combine(TestAssetDirectoryName, "plant1.json");
        private readonly string _plant2Path = Path.Combine(TestAssetDirectoryName, "plant2.json");
        private const string Plant1IDName = "plant1";
        private const string Plant1Name = "Plant 1";
        private const string Plant1Description = "A beautiful plant.";
        private const string Plant2IDName = "plant2";
        private const string Plant2Name = "Plant 2";
        private const string Plant2Description = "A useful plant.";
        
        [Test]
        public void TestCreatingAPlant() {
            var plantFactory = new PlantFactory(TestAssetDirectoryName, new DynamicAssetManager(BasePath));
            
            var plant = plantFactory.CreateInstance(Plant1IDName, IInitializationData.Empty);
            Assert.IsNotNull(plant, "Plant should not be null.");
            Assert.IsNotNull(plant.PrototypeData, "Plant data should not be null.");
            Assert.IsNotNull(plant.PrototypeData.IDName, "Plant id name should not be null.");
            Assert.IsNotNull(plant.PrototypeData.IDName, "Plant PrototypeData.IDName name should not return null.");
            Assert.AreEqual(Plant1IDName, plant.PrototypeData.IDName, "Plant IDName should match expected value.");
            Assert.AreEqual(plant.PrototypeData.IDName, plant.PrototypeData.IDName, "Plant Data IDName should match plant IDName.");
            Assert.AreEqual(Plant1Name, plant.PrototypeData.Name, "Plant Name should match expected value.");
            Assert.AreEqual(Plant1Description, plant.PrototypeData.Description, "Plant Description should match expected value.");
            var plant2 = plantFactory.CreateInstance(Plant2IDName, IInitializationData.Empty);
            Assert.IsNotNull(plant2, "Plant data should not be null.");
            Assert.AreEqual(Plant2IDName, plant2.PrototypeData.IDName, "Plant IDName should match expected value.");
            Assert.AreEqual(plant2.PrototypeData.IDName, plant2.PrototypeData.IDName, "Plant Data IDName should match plant IDName.");
            Assert.AreEqual(Plant2Name, plant2.PrototypeData.Name, "Plant Name should match expected value.");
            Assert.AreEqual(Plant2Description, plant2.PrototypeData.Description, "Plant Description should match expected value.");
        }

        [Test]
        public void TestCreatingAnInstanceWithSerializedObject() {
            // var buildingFactory = new BuildingFactory();
            var buildingFactory = new MonoInstanceFactory<Building, BuildingPrototypeData, BuildingInitializationData>();
            Building hut = buildingFactory.CreateInstance("hut", new BuildingInitializationData(Vector2Int.one));
            Assert.IsNotNull(hut, "Building should not be null.");
            Assert.IsNotNull(hut.PrototypeData, "Building data should not be null.");
            Assert.AreEqual("hut", hut.PrototypeData.IDName, "Building IDName should match expected value.");
            Assert.AreEqual(1, hut.PrototypeData.Floors, "Hut should have 1 floor.");
            Assert.AreEqual(Vector2Int.one, hut.Position, "Hut position should match set value via initialization.");
            Building villa = buildingFactory.CreateInstance("villa", new BuildingInitializationData(Vector2Int.one));
            Assert.IsNotNull(villa, "Building should not be null.");
            Assert.IsNotNull(villa.PrototypeData, "Building data should not be null.");
            Assert.AreEqual("villa", villa.PrototypeData.IDName, "Building IDName should match expected value.");
            Assert.AreEqual(3, villa.PrototypeData.Floors, "Villa should have 3 floors.");
            Assert.AreEqual(Vector2Int.one, villa.Position, "Villa Position should match expected value.");
        }
    }
}