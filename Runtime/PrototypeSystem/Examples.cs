using PrototypeSystem.PrototypeLoader;
using UnityEngine;

namespace PrototypeSystem {
    #region Examples

    public class Map : MonoBehaviour {
        private void Start() {
            var tileFactory =
                new InstanceFactoryNew<Tile, TilePrototypeData, TileInstanceData>(new PrototypeCollection<TilePrototypeData>(new JSonPrototypeLoader<TilePrototypeData>(Application.streamingAssetsPath)));
            var grid = new Tile[5, 5];
            for (int x = 0; x < 5; x++) {
                for (int y = 0; y < 5; y++) {
                    grid[x, y] = tileFactory.CreateInstance("grass", new TileInstanceData(new Vector2Int(x, y)));
                }
            }
        }
    }

    public class Tile : IInstance<TilePrototypeData, TileInstanceData> {
        public TilePrototypeData PrototypeData { get; private set; }
        public TileInstanceData InstanceData { get; private set; }

        public void Initialize(TilePrototypeData prototypeData, TileInstanceData instanceData) {
            PrototypeData = prototypeData;
            InstanceData = instanceData;
            
        }
    }

    public class TileInstanceData : IInstanceData {
        public TileInstanceData(Vector2Int position) {
            Position = position;
        }

        public Vector2Int Position { get; private set; }
    }

    public class TilePrototypeData : IPrototypeData {
        public TilePrototypeData(string idName, int movementCost) {
            IdName = idName;
            MovementCost = movementCost;
        }

        public string IdName { get; }
        public int MovementCost { get; }
        public string IDName { get; }
        public string BasedOn { get; }
    }

    #endregion
}