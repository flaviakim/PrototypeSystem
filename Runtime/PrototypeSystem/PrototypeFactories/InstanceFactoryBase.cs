using System.Collections.Generic;
using PrototypeSystem.PrototypeLoader;
using UnityEngine;

namespace PrototypeSystem {
    public abstract class InstanceFactoryBase<TInstance, TPrototypeData, TInitializationData> : IInstanceFactory<TInstance, TPrototypeData, TInitializationData> 
        where TInstance : IInstance<TPrototypeData> 
        where TPrototypeData : IPrototypeData
        where TInitializationData : IInitializationData {
        protected InstanceFactoryBase(PrototypeCollection<TPrototypeData> prototypeCollection) {
            PrototypeCollection = prototypeCollection;
        }

        protected PrototypeCollection<TPrototypeData> PrototypeCollection { get; private set; }

        public TInstance CreateInstance(string idName, TInitializationData initializationData) {
            if (!PrototypeCollection.TryGetPrototypeForName(idName, out TPrototypeData prototypeData)) {
                Debug.LogError($"Couldn't find instance with ID {idName}");
                return default;
            }
            Debug.Assert(prototypeData != null, "prototypeData != null");
            return CreateInstance(prototypeData, initializationData);
        }

        public abstract TInstance CreateInstance(TPrototypeData prototype, TInitializationData initializationData);
        
        // TODO create ease of use Factory with fields to directly set the fields in PrototypeCollection and Loader.
        
    }
}