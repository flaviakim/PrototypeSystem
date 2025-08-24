using UnityEngine;

namespace PrototypeSystem {
    public interface IInstance<TPrototypeData, in TInitializationData>
            where TPrototypeData : IPrototypeData 
            where TInitializationData : IInitializationData {
        public TPrototypeData PrototypeData { get; }
    }

    public abstract class Instance<TPrototypeData, TInitializationData> : IInstance<TPrototypeData, TInitializationData> where TPrototypeData : IPrototypeData where TInitializationData : IInitializationData {
        public TPrototypeData PrototypeData { get; private set; }
    }

}