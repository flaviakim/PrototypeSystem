using System;
// using PrototypeSystemV2.PrototypeLoader;
using UnityEngine;

namespace PrototypeSystem {
    public class InstanceFactory<TInstance, TPrototypeData, TInstanceData>
        where TInstance : IInstance<TPrototypeData, TInstanceData>
        where TPrototypeData : IPrototypeData
        where TInstanceData : IInstanceData {
        public IPrototypeCollection<TPrototypeData> PrototypeCollection { get; set; }

        private readonly Func<TPrototypeData, TInstanceData, TInstance> _newInstanceCreator;
        private readonly Action<TInstance, TPrototypeData, TInstanceData> _newInstanceInitializer;

        public InstanceFactory(IPrototypeCollection<TPrototypeData> prototypeCollection, Func<TPrototypeData, TInstanceData, TInstance> newInstanceCreator, Action<TInstance, TPrototypeData, TInstanceData> newInstanceInitializer = null) { 
            PrototypeCollection = prototypeCollection;
            _newInstanceCreator = newInstanceCreator;
            _newInstanceInitializer = newInstanceInitializer;
        }

        public TInstance CreateInstance(string idName, TInstanceData instanceData) {
            if (!PrototypeCollection.TryGetPrototypeForName(idName, out TPrototypeData prototypeData)) {
                Debug.LogError($"Prototype with idName {idName} not found.");
                return default;
            }

            TInstance instance = _newInstanceCreator.Invoke(prototypeData, instanceData);
            _newInstanceInitializer?.Invoke(instance, prototypeData, instanceData);
            return instance;
        }
    }
}