// using System.Collections.Generic;
//
// namespace PrototypeSystem {
//     public abstract class DebugValuesPrototypeCollection<TPrototypeData> : DictionaryPrototypeCollection<TPrototypeData> where TPrototypeData : IPrototypeData {
//         protected override Dictionary<string, TPrototypeData> LoadPrototypeDatas() {
//             var prototypes = new Dictionary<string, TPrototypeData>();
//             foreach (var prototype in GetDebugPrototypes()) {
//                 prototypes.Add(prototype.IDName, prototype);
//             }
//
//             return prototypes;
//         }
//
//         protected abstract List<TPrototypeData> GetDebugPrototypes();
//
//
//     }
// }