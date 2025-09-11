using System.Collections.Generic;

namespace PrototypeSystem.PrototypeLoader {
    public interface IPrototypeLoader<TPrototypeData> where TPrototypeData : IPrototypeData {
        public Dictionary<string, TPrototypeData> LoadAll();
    }
}