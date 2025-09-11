namespace PrototypeSystem.Examples {
    public class TilePrototypeData : IPrototypeData {
        public TilePrototypeData(string idName, string basedOn, int movementCost) {
            IDName = idName;
            BasedOn = basedOn;
            MovementCost = movementCost;
        }

        public string IDName { get; }
        public string BasedOn { get; }
        public int MovementCost { get; }
    }

}