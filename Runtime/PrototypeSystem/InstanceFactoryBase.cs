using System.Collections.Generic;
using UnityEngine;

namespace PrototypeSystem {
    public abstract class InstanceFactoryBase<TInstance, TPrototypeData, TInitializationData> : IInstanceFactory<TInstance, TPrototypeData, TInitializationData> 
        where TInstance : IInstance<TPrototypeData> 
        where TPrototypeData : IPrototypeData
        where TInitializationData : IInitializationData {
        protected abstract IPrototypeCollection<TPrototypeData> PrototypeCollection { get; }
        
        public TInstance CreateInstance(string idName, TInitializationData initializationData) {
            if (!TryGetPrototypeForName(idName, out TPrototypeData prototypeData)) {
                Debug.LogError($"Couldn't find instance with ID {idName}");
                return default;
            }
            Debug.Assert(prototypeData != null, "prototypeData != null");
            return CreateInstance(prototypeData, initializationData);
        }

        public abstract TInstance CreateInstance(TPrototypeData prototype, TInitializationData initializationData);

        public bool TryGetPrototypeForName(string idName, out TPrototypeData prototype) {
            return PrototypeCollection.TryGetPrototypeForName(idName, out prototype);
        }

        public List<TPrototypeData> GetPrototypes() {
            return PrototypeCollection.GetPrototypes();
        }

        public List<string> GetPrototypeNames() {
            return PrototypeCollection.GetPrototypeNames();
        }

        public void PreloadPrototypes() {
            PrototypeCollection.PreloadPrototypes();
        }
        
    }
}