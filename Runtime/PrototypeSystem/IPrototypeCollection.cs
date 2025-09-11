using System.Collections.Generic;

namespace PrototypeSystem {
    public interface IPrototypeCollection<TPrototypeData> where TPrototypeData : IPrototypeData {
        bool TryGetPrototypeForName(string idName, out TPrototypeData prototype);
        IEnumerable<string> GetPrototypeNames();
        IEnumerable<TPrototypeData> GetPrototypes();
        void Initialize();
        void AddOrReplace(TPrototypeData def);
        void Clear();
    }
}