using UnityEngine;

namespace TypeObjectSystem.Tests.TypeObjectSystem.TileExample {
    public class Tile : MonoBehaviour, IInstance<TileType, TileInstanceData> {
        
        public Vector2Int Position => InstanceData.Position;
        public int MovementCost => Type.MovementCost;
        public int Defense => Type.Defense;
        public string IDName => Type.IDName;

        public TileType Type { get; private set; }
        public TileInstanceData InstanceData { get; private set; }

        public void Initialize(TileType type, TileInstanceData instanceData) {
            Type = type;
            InstanceData = instanceData;
        }
    }
}