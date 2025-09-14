using System.Collections.Generic;

namespace TypeObjectSystem {
    public interface ITypeCollection<TType> where TType : IType {
        bool TryGetTypeForName(string idName, out TType type);
        IEnumerable<string> GetTypeNames();
        IEnumerable<TType> GetTypes();
        void PreloadAll();
        void AddOrReplace(TType def);
        void Clear();
    }
}