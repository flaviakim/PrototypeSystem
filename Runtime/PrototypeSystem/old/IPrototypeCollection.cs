// using System.Collections.Generic;
// using PrototypeSystem.PrototypeLoader;
//
// namespace PrototypeSystem {
//     public interface IPrototypeCollection<TPrototypeData> where TPrototypeData : IPrototypeData { 
//         public bool TryGetPrototypeForName(string idName, out TPrototypeData prototype);
//         public IEnumerable<string> GetPrototypeNames();
//         public IEnumerable<TPrototypeData> GetPrototypes();
//         public void AddOrReplace(TPrototypeData def);
//         public void Initialize(IPrototypeLoader<TPrototypeData> prototypeLoader);
//     }
// }