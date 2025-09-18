namespace TypeObjectSystem.Tests.TypeObjectSystem.TileExample {
    public class TileType : IType {
        public TileType(string idName, string parent, int movementCost, int defense) {
            IDName = idName;
            Parent = parent;
            MovementCost = movementCost;
            Defense = defense;
        }

        public string IDName { get; }
        public string Parent { get; }
        public int MovementCost { get; }
        public int Defense { get; }
    }

}
