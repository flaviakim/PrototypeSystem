using System.Collections.Generic;
using UnityEngine;

namespace PrototypeSystem.PrototypeLoader {
    public interface IPrototypeLoader<TData> where TData : IPrototypeData {
        public Dictionary<string, TData> LoadAll();
    }
}