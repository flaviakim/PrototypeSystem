namespace TypeObjectSystem.Tests.TypeObjectSystem.TileExample {
    public class TilePrototypeData : IPrototypeData {
        public TilePrototypeData(string idName, string basedOn, int movementCost, int defense) {
            IDName = idName;
            BasedOn = basedOn;
            MovementCost = movementCost;
            Defense = defense;
        }

        public string IDName { get; }
        public string BasedOn { get; }
        public int MovementCost { get; }
        public int Defense { get; }
    }

}