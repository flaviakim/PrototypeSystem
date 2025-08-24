using System.Collections.Generic;

namespace PrototypeSystem {
    public interface IPrototypeCollection<TPrototypeData> where TPrototypeData : IPrototypeData { 
        public bool TryGetPrototypeForName(string idName, out TPrototypeData prototype);
        public List<string> GetPrototypeNames();
        public List<TPrototypeData> GetPrototypes();
        public void PreloadPrototypes();
    }
}