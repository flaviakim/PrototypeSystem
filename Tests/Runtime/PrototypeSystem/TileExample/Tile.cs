using UnityEngine;

namespace PrototypeSystem.Tests.PrototypeSystem.TileExample {
    public class Tile : MonoBehaviour, IInstance<TilePrototypeData, TileInstanceData> {
        
        public Vector2Int Position => InstanceData.Position;
        public int MovementCost => PrototypeData.MovementCost;
        
        public TilePrototypeData PrototypeData { get; private set; }
        public TileInstanceData InstanceData { get; private set; }

        public void Initialize(TilePrototypeData prototypeData, TileInstanceData instanceData) {
            PrototypeData = prototypeData;
            InstanceData = instanceData;
        }
    }
}