namespace PrototypeSystem.Examples {
    public class Tile : MonoBehaviour, IInstance<TilePrototypeData, TileInstanceData> {
        
        public Vector2Int Position => InstanceData.Position;
        public int MovementCost => PrototypeData.MovementCost;
        
        private TilePrototypeData PrototypeData { get; set; }
        private TileInstanceData InstanceData { get; set; }

        public void Initialize(TilePrototypeData prototypeData, TileInstanceData instanceData) {
            PrototypeData = prototypeData;
            InstanceData = instanceData;
        }
    }
}