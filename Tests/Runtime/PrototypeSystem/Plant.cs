namespace PrototypeSystem.Tests.PrototypeSystem {
    public class Plant : IInstance<PlantData> {
        public string IDName { get; }
        public PlantData Data { get; }

        public Plant(PlantData data) {
            Data = data;
            IDName = data.IDName;
        }
    }
}