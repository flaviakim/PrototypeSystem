namespace PrototypeSystem.Tests.PrototypeSystem {
    public class Plant : Instance<PlantPrototypeData, IInitializationData.EmptyInitializationData> {
        public PlantPrototypeData PrototypeData { get; private set; }
        
        public float Growth { get; private set; }

        public Plant(PlantPrototypeData prototypeData, IInitializationData initializationData) {
            PrototypeData = prototypeData;
            Growth = 0;
        }
    }
    
    public class PlantPrototypeData : BasePrototypeData {
        public PlantPrototypeData(string idName, string name, string description) : base(idName) {
            Name = name;
            Description = description;
        }
        public string Name { get; }
        public string Description { get; }
    }
    
}