using JetBrains.Annotations;

namespace PrototypeSystem.Tests.PrototypeSystem {
    public class PlantData : BasePrototypeData {
        public PlantData(string idName, string name, string description) : base(idName) {
            Name = name;
            Description = description;
        }
        public string Name { get; }
        [CanBeNull] public string Description { get; }
    }
}