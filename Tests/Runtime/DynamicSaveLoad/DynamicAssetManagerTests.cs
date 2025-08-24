using System;
using System.IO;
using System.Threading.Tasks;
using DynamicSaveLoad;
using NUnit.Framework;
using UnityEngine;

namespace PrototypeSystem.Tests.DynamicSaveLoad
{
    [TestFixture]
    [TestOf(typeof(DynamicAssetManager))]
    public class DynamicAssetManagerTests
    {
        private const string TestAssetDirectoryName = "DynamicSavingLoadingTestAssets";
        private string _testRootPath;
        private DynamicAssetManager _assetManager;

        private string _testAsset1Path => Path.Combine(TestAssetDirectoryName, "testAsset1.json");
        private string _testAsset2Path => Path.Combine(TestAssetDirectoryName, "testAsset2.json");
        private string _nestedTestAsset1Path => Path.Combine(TestAssetDirectoryName, "NestedTestAsset", "nestedTestAsset1.json");

        [SetUp]
        public void SetUp()
        {
            // Use temporary directory inside Application.temporaryCachePath
            _testRootPath = Path.Combine(Application.temporaryCachePath, TestAssetDirectoryName);
            Debug.Log($"Full path: {Path.GetFullPath(_testRootPath)}, rootPath: {_testRootPath}");
            Directory.CreateDirectory(_testRootPath);
            Directory.CreateDirectory(Path.Combine(_testRootPath, "NestedTestAsset"));

            // Create mock JSON test files
            File.WriteAllText(Path.Combine(_testRootPath, "testAsset1.json"),
                "{\"Name\":\"TestAsset1\",\"Description\":null}");
            File.WriteAllText(Path.Combine(_testRootPath, "testAsset2.json"),
                "{\"Name\":\"TestAsset2\",\"Description\":\"This is a test asset with an optional property.\"}");
            File.WriteAllText(Path.Combine(_testRootPath, "NestedTestAsset", "nestedTestAsset1.json"),
                "{\"Name\":\"NestedTestAsset1\",\"Description\":null}");

            // Initialize DynamicAssetManager with the test root path
            _assetManager = new DynamicAssetManager(_testRootPath);
        }

        [TearDown]
        public void TearDown()
        {
            // Cleanup test directory
            if (Directory.Exists(_testRootPath))
                Directory.Delete(_testRootPath, recursive: true);

            _assetManager.Dispose();
        }

        // ---------------------------
        // Sync JSON Tests
        // ---------------------------

        [Test]
        public void TestLoadSimpleJson()
        {
            var loaded = _assetManager.Json.LoadAll<TestAsset>("", recursive: false);
            Assert.IsNotNull(loaded);
            Assert.AreEqual(2, loaded.Count);
            Assert.AreEqual("TestAsset1", loaded[0].Name);
            Assert.AreEqual("TestAsset2", loaded[1].Name);
            Assert.AreEqual(null, loaded[0].Description);
            Assert.AreEqual("This is a test asset with an optional property.", loaded[1].Description);
        }

        [Test]
        public void TestLoadAllJsonRecursive()
        {
            var loaded = _assetManager.Json.LoadAll<TestAsset>("", recursive: true);
            Assert.IsNotNull(loaded);
            Assert.AreEqual(3, loaded.Count);
            Assert.AreEqual("TestAsset1", loaded[0].Name);
            Assert.AreEqual("TestAsset2", loaded[1].Name);
            Assert.AreEqual("NestedTestAsset1", loaded[2].Name);
        }

        [Test]
        public void TestTrySaveJson()
        {
            var newAsset = new TestAsset("SavedAsset", "Saved via DynamicAssetManager");
            bool success = _assetManager.Json.TrySave(newAsset, "savedAsset.json", overwrite: true);
            Assert.IsTrue(success);
            Assert.IsTrue(File.Exists(Path.Combine(_testRootPath, "savedAsset.json")));
        }

        // ---------------------------
        // Async JSON Tests
        // ---------------------------

        [Test]
        public async Task TestLoadAllJsonAsync()
        {
            var loaded = await _assetManager.JsonAsync.LoadAll<TestAsset>("");
            Assert.IsNotNull(loaded);
            Assert.AreEqual(3, loaded.Count);
            Assert.AreEqual("TestAsset1", loaded[0].Name);
            Assert.AreEqual("NestedTestAsset1", loaded[2].Name);
        }

        [Test]
        public async Task TestSaveJsonAsync()
        {
            var newAsset = new TestAsset("AsyncSavedAsset", "Saved async via DynamicAssetManager");
            bool success = await _assetManager.JsonAsync.Save(newAsset, "asyncSavedAsset.json", overwrite: true);
            Assert.IsTrue(success);
            Assert.IsTrue(File.Exists(Path.Combine(_testRootPath, "asyncSavedAsset.json")));
        }

        // ---------------------------
        // TestAsset Class
        // ---------------------------

        [Serializable]
        public class TestAsset
        {
            public string Name { get; set; }
            public string Description { get; set; }

            public TestAsset() { }

            public TestAsset(string name, string description)
            {
                Name = name;
                Description = description;
            }
        }
    }
}
