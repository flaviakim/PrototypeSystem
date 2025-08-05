using System.IO;
using DynamicSavingLoading;
using JetBrains.Annotations;
using NUnit.Framework;

namespace PrototypeSystem.Tests.DynamicSavingLoading {
    
    [TestFixture]
    [TestOf(typeof(DynamicLoader))]
    public class DynamicLoaderTest {
        const string TestAssetDirectoryName = "DynamicSavingLoadingTestAssets";
        const string BasePath = "Packages/PrototypeSystem/Tests/Runtime/Assets/";
        private readonly string _testAsset1Path = Path.Combine(TestAssetDirectoryName, "testAsset1.json");
        private readonly string _testAsset2Path = Path.Combine(TestAssetDirectoryName, "testAsset2.json");
        private readonly string _nestedTestAsset1Path = Path.Combine(TestAssetDirectoryName, "NestedTestAsset", "nestedTestAsset1.json");
        
        [SetUp]
        public void SetUp() {
            DynamicLoader.DefaultDynamicAssetPath = Path.GetFullPath(BasePath);
        }

        [Test]
        public void TestLoadSimpleJson()  {
            var loadedJson = DynamicLoader.LoadJson<TestAsset>(_testAsset1Path);
            Assert.IsNotNull(loadedJson, "Loaded JSON should not be null.");
            Assert.AreEqual("TestAsset1", loadedJson.Name, "Loaded JSON Name should match expected value");
		}

        [Test]
        public void TestLoadSimpleJsonFullPath() {
            var fullJsonPath = Path.GetFullPath(Path.Combine(BasePath, _testAsset1Path));
            Assert.IsTrue(File.Exists(fullJsonPath), $"JSON file does not exist at path: {fullJsonPath}");
            var loadedJson = DynamicLoader.LoadJson<TestAsset>(fullJsonPath, isRelativeToDefaultPath: false);
            Assert.IsNotNull(loadedJson, "Loaded JSON should not be null.");
            Assert.AreEqual("TestAsset1", loadedJson.Name, "Loaded JSON Name should match expected value");
        }
        
        [Test]
        public void TestLoadSimpleJsonWithNoOptionalProperty() {
            var loadedJson = DynamicLoader.LoadJson<TestAsset>(_testAsset1Path);
            Assert.IsNotNull(loadedJson, "Loaded JSON should not be null.");
            Assert.AreEqual("TestAsset1", loadedJson.Name, "Loaded JSON Name should match expected value");
            Assert.IsNull(loadedJson.Description, "Loaded JSON description should be null.");
        }
        
        [Test]
        public void TestLoadSimpleJsonWithOptionalProperty() {
            var loadedJson = DynamicLoader.LoadJson<TestAsset>(_testAsset2Path);
            Assert.IsNotNull(loadedJson, "Loaded JSON should not be null.");
            Assert.AreEqual("TestAsset2", loadedJson.Name, "Loaded JSON Name should match expected value");
            Assert.IsNotNull(loadedJson.Description, "Loaded JSON description should not be null.");
            Assert.AreEqual("This is a test asset with an optional property.", loadedJson.Description, "Loaded JSON description should match expected value");
        }
        
        [Test]
        public void TestLoadAllJson() {
            var loadedJsonList = DynamicLoader.LoadAllJson<TestAsset>(TestAssetDirectoryName, recursive: false);
            Assert.IsNotNull(loadedJsonList, "Loaded JSON list should not be null.");
            Assert.IsNotEmpty(loadedJsonList, "Loaded JSON list should not be empty.");
            Assert.AreEqual(2, loadedJsonList.Count, "Expected to load 2 JSON assets.");
            Assert.AreEqual("TestAsset1", loadedJsonList[0].Name, "First asset name should match expected value");
            Assert.AreEqual("TestAsset2", loadedJsonList[1].Name, "Second asset name should match expected value");
        }
        
        [Test]
        public void TestLoadAllJsonRecursive() {
            var loadedJsonList = DynamicLoader.LoadAllJson<TestAsset>(TestAssetDirectoryName, recursive: true);
            Assert.IsNotNull(loadedJsonList, "Loaded JSON list should not be null.");
            Assert.IsNotEmpty(loadedJsonList, "Loaded JSON list should not be empty.");
            Assert.AreEqual(3, loadedJsonList.Count, "Expected to load 3 JSON assets including nested ones.");
            Assert.AreEqual("TestAsset1", loadedJsonList[0].Name, "First asset name should match expected value");
            Assert.AreEqual("TestAsset2", loadedJsonList[1].Name, "Second asset name should match expected value");
            Assert.AreEqual("NestedTestAsset1", loadedJsonList[2].Name, "Nested asset name should match expected value");
        }

        [Test]
        public void TestLoadAllJsonFullPath() {
            var fullPath = Path.GetFullPath(Path.Combine(BasePath, TestAssetDirectoryName));
            Assert.IsTrue(Directory.Exists(fullPath), $"Directory does not exist at path: {fullPath}");
            var loadedJsonList = DynamicLoader.LoadAllJson<TestAsset>(fullPath, recursive: false);
            Assert.IsNotNull(loadedJsonList, "Loaded JSON list should not be null.");
            Assert.IsNotEmpty(loadedJsonList, "Loaded JSON list should not be empty.");
            Assert.AreEqual(2, loadedJsonList.Count, "Expected to load 2 JSON assets.");
            Assert.AreEqual("TestAsset1", loadedJsonList[0].Name, "First asset name should match expected value");
            Assert.AreEqual("TestAsset2", loadedJsonList[1].Name, "Second asset name should match expected value");
        }
        
        
        public class TestAsset {
            public string Name { get; private set; }
            [CanBeNull] public string Description { get; private set; }

            public TestAsset(string name, [CanBeNull] string description) {
                Name = name;
                Description = description;
            }
        }
    }
}