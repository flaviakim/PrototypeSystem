using System.Collections.Generic;
using TypeObjectSystem.TypeLoader;

namespace TypeObjectSystem {
    public interface ITypeCollection<TType> where TType : IType {
        bool TryGetTypeForName(string idName, out TType type);
        IEnumerable<string> GetTypeNames();
        IEnumerable<TType> GetTypes();
        void LoadTypes(bool clearExisting = true, params ITypeLoader<TType>[] typeLoaders); // TODO refactor this method to async
        void AddOrReplace(TType def);
        void Clear();
    }
}
