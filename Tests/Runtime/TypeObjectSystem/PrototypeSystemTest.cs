using System.IO;
using NUnit.Framework;
using TypeObjectSystem.PrototypeLoader;
using TypeObjectSystem.Tests.TypeObjectSystem.TileExample;
using UnityEngine;

namespace TypeObjectSystem.Tests.TypeObjectSystem {
    [TestFixture]
    public class TypeObjectSystemTest {
        private static readonly string BasePath =
            Path.Combine("Packages", "TypeObjectSystem", "Tests", "Runtime", "Assets");

        private static readonly string TilesRelativePath = Path.Combine("TileExample", "Tiles");

        private static readonly Vector2Int InitialPositionGrass = new Vector2Int(1, 2);
        private static readonly Vector2Int InitialPositionSwamp = new Vector2Int(3, 4);
        
        private const string GrassIDName           = "grass";
        private const int    GrassTileMovementCost = 2;
        private const int    GrassTileDefense      = 1;
        private const string SwampIDName           = "swamp";
        private const int    SwampTileMovementCost = 4;
        private const int    SwampTileDefense      = GrassTileDefense;


        [Test]
        public void TestSimpleCreationWorkflow() {
            var tileFactory = CreateTileFactory();

            Tile grassTile = tileFactory.CreateInstance(GrassIDName, new TileInstanceData(InitialPositionGrass));

            Assert.IsNotNull(grassTile, "Tile should not be null.");
            Assert.IsNotNull(grassTile.PrototypeData, "Tile prototype data should not be null.");
            Assert.IsNotNull(grassTile.InstanceData, "Tile prototype data should not be null.");
            Assert.AreEqual(GrassIDName, grassTile.PrototypeData.IDName);
            Assert.AreEqual(InitialPositionGrass, grassTile.InstanceData.Position);
            Assert.AreEqual(GrassTileMovementCost, grassTile.PrototypeData.MovementCost);
            Assert.AreEqual(GrassTileDefense, grassTile.PrototypeData.Defense);

            // Test Shortcuts
            Assert.AreEqual(GrassIDName, grassTile.IDName);
            Assert.AreEqual(InitialPositionGrass, grassTile.Position);
            Assert.AreEqual(grassTile.InstanceData.Position, grassTile.Position);
            Assert.AreEqual(GrassTileMovementCost, grassTile.MovementCost);
            Assert.AreEqual(GrassTileDefense, grassTile.Defense);
        }

        [Test]
        public void Test_BasedOn_CorrectlyInstantiatesWithParent() {
            var tileFactory = CreateTileFactory();
            
            Tile swampTile = tileFactory.CreateInstance(SwampIDName, new TileInstanceData(InitialPositionSwamp));
            
            // Sanity check, make sure it actually loaded
            Assert.IsNotNull(swampTile, "Tile should not be null.");
            Assert.IsNotNull(swampTile.PrototypeData, "Tile prototype data should not be null.");
            Assert.IsNotNull(swampTile.InstanceData, "Tile prototype data should not be null.");
            // Sanity check, make sure basic properties are set
            Assert.AreEqual(SwampIDName, swampTile.IDName);
            Assert.AreEqual(InitialPositionSwamp, swampTile.Position);
            
            // Actual checks
            Assert.AreEqual(SwampTileMovementCost, swampTile.MovementCost, $"Failed to override {nameof(swampTile.MovementCost)} of {nameof(SwampIDName)}");
            Assert.AreEqual(SwampTileDefense, swampTile.Defense, $"Failed to use property from parent class with {nameof(IPrototypeData.BasedOn)}");
        }

        private static InstanceFactoryMonoBehaviour<Tile, TilePrototypeData, TileInstanceData> CreateTileFactory() {
            var prototypeLoader = new JSonPrototypeLoader<TilePrototypeData>(TilesRelativePath, BasePath);
            var prototypeCollection = new PrototypeCollection<TilePrototypeData>(prototypeLoader);
            prototypeCollection.Initialize();
            var tileFactory = new InstanceFactoryMonoBehaviour<Tile, TilePrototypeData, TileInstanceData>(prototypeCollection);
            return tileFactory;
        }
    }

}