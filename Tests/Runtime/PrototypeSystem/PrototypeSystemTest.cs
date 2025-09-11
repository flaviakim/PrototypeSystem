using System.IO;
using NUnit.Framework;
using PrototypeSystem.PrototypeLoader;
using PrototypeSystem.Tests.PrototypeSystem.TileExample;
using UnityEngine;

namespace PrototypeSystem.Tests.PrototypeSystem {
    [TestFixture]
    public class PrototypeSystemTest {
        private static readonly string BasePath =
            Path.Combine("Packages", "PrototypeSystem", "Tests", "Runtime", "Assets");

        private static readonly string TilesRelativePath = Path.Combine("TileExample", "Tiles");

        [Test]
        public void TestSimpleCreationWorkflow() {
            const string idName = "grass";
            var initialPosition = new Vector2Int(1, 2);
            const int grassTileMovementCost = 2;
            var prototypeLoader = new JSonPrototypeLoader<TilePrototypeData>(TilesRelativePath, BasePath);
            var prototypeCollection = new PrototypeCollection<TilePrototypeData>(prototypeLoader);
            prototypeCollection.Initialize();
            var tileFactory = new InstanceFactoryMonoBehaviour<Tile, TilePrototypeData, TileInstanceData>(prototypeCollection);

            Tile tile = tileFactory.CreateInstance(idName, new TileInstanceData(initialPosition));

            Assert.IsNotNull(tile, "Tile should not be null.");
            Assert.IsNotNull(tile.PrototypeData, "Tile prototype data should not be null.");
            Assert.IsNotNull(tile.InstanceData, "Tile prototype data should not be null.");
            Assert.AreEqual(idName, tile.PrototypeData.IDName);
            Assert.AreEqual(initialPosition, tile.InstanceData.Position);
            Assert.AreEqual(initialPosition, tile.Position);
            Assert.AreEqual(grassTileMovementCost, tile.PrototypeData.MovementCost);
            Assert.AreEqual(grassTileMovementCost, tile.MovementCost);
        }
    }

}