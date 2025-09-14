using System.Collections.Generic;

namespace TypeObjectSystem.TypeLoader {
    public interface ITypeLoader<TType> where TType : IType {
        public Dictionary<string, TType> LoadAll();
    }
}