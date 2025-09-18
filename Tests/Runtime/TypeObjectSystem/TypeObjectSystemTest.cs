using System.IO;
using NUnit.Framework;
using TypeObjectSystem.TypeLoader;
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
            Assert.IsNotNull(grassTile.Type, "Tile type data should not be null.");
            Assert.IsNotNull(grassTile.InstanceData, "Tile type data should not be null.");
            Assert.AreEqual(GrassIDName, grassTile.Type.IDName);
            Assert.AreEqual(InitialPositionGrass, grassTile.InstanceData.Position);
            Assert.AreEqual(GrassTileMovementCost, grassTile.Type.MovementCost);
            Assert.AreEqual(GrassTileDefense, grassTile.Type.Defense);

            // Test Shortcuts
            Assert.AreEqual(GrassIDName, grassTile.IDName);
            Assert.AreEqual(InitialPositionGrass, grassTile.Position);
            Assert.AreEqual(grassTile.InstanceData.Position, grassTile.Position);
            Assert.AreEqual(GrassTileMovementCost, grassTile.MovementCost);
            Assert.AreEqual(GrassTileDefense, grassTile.Defense);
        }

        [Test]
        public void Test_Parent_CorrectlyInstantiatesWithParent() {
            var tileFactory = CreateTileFactory();
            
            Tile swampTile = tileFactory.CreateInstance(SwampIDName, new TileInstanceData(InitialPositionSwamp));
            
            // Sanity check, make sure it actually loaded
            Assert.IsNotNull(swampTile, "Tile should not be null.");
            Assert.IsNotNull(swampTile.Type, "Tile prototype data should not be null.");
            Assert.IsNotNull(swampTile.InstanceData, "Tile prototype data should not be null.");
            // Sanity check, make sure basic properties are set
            Assert.AreEqual(SwampIDName, swampTile.IDName);
            Assert.AreEqual(InitialPositionSwamp, swampTile.Position);
            
            // Actual checks
            Assert.AreEqual(SwampTileMovementCost, swampTile.MovementCost, $"Failed to override {nameof(swampTile.MovementCost)} of {nameof(SwampIDName)}");
            Assert.AreEqual(SwampTileDefense, swampTile.Defense, $"Failed to use property from parent class with {nameof(IType.Parent)}");
        }

        private static InstanceFactoryMonoBehaviour<Tile, TileType, TileInstanceData> CreateTileFactory() {
            var prototypeLoader = new JSonTypeLoader<TileType>(TilesRelativePath, BasePath);
            var prototypeCollection = new TypeCollection<TileType>(prototypeLoader);
            var tileFactory = new InstanceFactoryMonoBehaviour<Tile, TileType, TileInstanceData>(prototypeCollection, (tile, type, instanceData) => tile.Initialize(type, instanceData));
            return tileFactory;
        }
    }

}
