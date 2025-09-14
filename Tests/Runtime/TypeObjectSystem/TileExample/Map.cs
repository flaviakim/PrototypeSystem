using TypeObjectSystem.PrototypeLoader;
using UnityEngine;

namespace TypeObjectSystem.Tests.TypeObjectSystem.TileExample {
    public class Map : MonoBehaviour {
        private void Start() {
            var tileFactory = new InstanceFactoryMonoBehaviour<Tile, TilePrototypeData, TileInstanceData>(new PrototypeCollection<TilePrototypeData>(new JSonPrototypeLoader<TilePrototypeData>(Application.streamingAssetsPath)));
            var grid = new Tile[5, 5];
            for (int x = 0; x < 5; x++) {
                for (int y = 0; y < 5; y++) {
                    grid[x, y] = tileFactory.CreateInstance("grass", new TileInstanceData(new Vector2Int(x, y)));
                }
            }
        }
    }
}