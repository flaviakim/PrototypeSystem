using System;
using UnityEngine;

namespace TypeObjectSystem {
    public class InstanceFactory<TInstance, TType, TInstanceData>
        where TInstance : IInstance<TType, TInstanceData>
        where TType : IType
        where TInstanceData : IInstanceData {
        public ITypeCollection<TType> TypeCollection { get; set; }

        private readonly Func<TType, TInstanceData, TInstance> _newInstanceCreator;
        private readonly Action<TInstance, TType, TInstanceData> _newInstanceInitializer;

        public InstanceFactory(ITypeCollection<TType> typeCollection, Func<TType, TInstanceData, TInstance> newInstanceCreator, Action<TInstance, TType, TInstanceData> newInstanceInitializer = null) { 
            TypeCollection = typeCollection;
            _newInstanceCreator = newInstanceCreator;
            _newInstanceInitializer = newInstanceInitializer;
        }

        public TInstance CreateInstance(string idName, TInstanceData instanceData) {
            if (!TypeCollection.TryGetTypeForName(idName, out TType type)) {
                Debug.LogError($"Type with idName {idName} not found.");
                return default;
            }

            TInstance instance = _newInstanceCreator.Invoke(type, instanceData);
            _newInstanceInitializer?.Invoke(instance, type, instanceData);
            return instance;
        }
    }
}