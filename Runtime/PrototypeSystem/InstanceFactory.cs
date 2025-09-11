using System;
// using PrototypeSystemV2.PrototypeLoader;
using UnityEngine;

namespace PrototypeSystem {
    public class InstanceFactory<TInstance, TPrototypeData, TInstanceData>
        where TInstance : IInstance<TPrototypeData, TInstanceData>
        where TPrototypeData : IPrototypeData
        where TInstanceData : IInstanceData {
        public IPrototypeCollection<TPrototypeData> PrototypeCollection { get; set; }

        private readonly Func<TInstance> _newInstanceCreator;

        public InstanceFactory(IPrototypeCollection<TPrototypeData> prototypeCollection, Func<TInstance> newInstanceCreator) { 
            PrototypeCollection = prototypeCollection;
            _newInstanceCreator = newInstanceCreator;
        }

        public TInstance CreateInstance(string idName, TInstanceData instanceData) {
            if (!PrototypeCollection.TryGetPrototypeForName(idName, out TPrototypeData prototypeData)) {
                Debug.LogError($"Prototype with idName {idName} not found.");
                return default;
            }

            TInstance instance = _newInstanceCreator.Invoke();
            instance.Initialize(prototypeData, instanceData);
            return instance;
        }
    }
}