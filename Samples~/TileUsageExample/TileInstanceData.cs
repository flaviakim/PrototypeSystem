using UnityEngine;

namespace PrototypeSystem.Examples {
    public class TileInstanceData : IInstanceData {
        public TileInstanceData(Vector2Int position) {
            Position = position;
        }

        public Vector2Int Position { get; private set; }
    }
}