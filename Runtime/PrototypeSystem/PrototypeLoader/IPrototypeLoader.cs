using System.Collections.Generic;

namespace PrototypeSystem.PrototypeLoader {
    public interface IPrototypeLoader<TData> where TData : IPrototypeData {
        public Dictionary<string, TData> LoadAll();
    }
}