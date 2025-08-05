using System;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using DynamicSavingLoading;
using UnityEditor;
using UnityEngine;

namespace Tests.Runtime {
    [TestFixture]
    [TestOf(typeof(DynamicLoader))]
    public class DynamicLoaderTest {
        [SetUp]
        public void SetUp() {
            // Debug.Log($"FullPath filename: {Path.GetFullPath("testAsset1.json")}");
            // Debug.Log(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            // Debug.Log($"Other full path: {Path.GetFullPath("Packages/PrototypeSystem/Tests/Runtime/DynamicSavingLoading/Assets/TestAssets/testAsset1.json")}");
            DynamicLoader.DefaultDynamicAssetPath = Path.GetFullPath("Packages/PrototypeSystem/Tests/Runtime/DynamicSavingLoading/Assets/");
        }

        [Test]
        public void TestLoadSimpleJson()  {
            var jsonPath = "TestAssets/testAsset1.json";
            var loadedJson = DynamicLoader.LoadJson<TestAsset>(jsonPath);
            Assert.IsNotNull(loadedJson, "Loaded JSON should not be null.");
            Assert.AreEqual("TestAsset1", loadedJson.Name, "Loaded JSON Name should match expected value");
		}
    }

    public class TestAsset {
        public string Name { get; private set; }

        public TestAsset(string name) {
            Name = name;
        }
    }
}