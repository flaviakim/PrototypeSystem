using UnityEngine;

namespace PrototypeSystem.Tests.PrototypeSystem.TileExample {
    public class TileInstanceData : IInstanceData {
        public TileInstanceData(Vector2Int position) {
            Position = position;
        }

        public Vector2Int Position { get; private set; }
    }
}