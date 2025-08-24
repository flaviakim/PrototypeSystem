using UnityEngine;

namespace PrototypeSystem {
    public interface IInstance<TPrototypeData>
            where TPrototypeData : IPrototypeData {
        public TPrototypeData PrototypeData { get; }
    }

    public abstract class Instance<TPrototypeData> : IInstance<TPrototypeData> where TPrototypeData : IPrototypeData {
        public TPrototypeData PrototypeData { get; private set; }
    }

}