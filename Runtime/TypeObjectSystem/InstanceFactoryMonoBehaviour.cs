using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TypeObjectSystem {
    public class InstanceFactoryMonoBehaviour<TInstance, TType, TInstanceData> : InstanceFactory<TInstance,
        TType, TInstanceData>
        where TInstance : MonoBehaviour, IInstance<TType, TInstanceData>
        where TType : IType
        where TInstanceData : IInstanceData {
        private static TInstance NewInstanceCreator(TType data, GameObject prefab = null) {
            const string defaultGoName = "New Game Object";
            GameObject go = prefab == null ? new GameObject(typeof(TInstance).Name) : Object.Instantiate(prefab);
            go.name = $"{(string.IsNullOrWhiteSpace(go.name) || go.name.Equals(defaultGoName) ? "" : $"{go.name}_")}{typeof(TInstance).Name}_{data.IDName}";
            return go.AddComponent<TInstance>();
        }

        public InstanceFactoryMonoBehaviour(
                ITypeCollection<TType> typeCollection,
                Action<TInstance, TType, TInstanceData> newInstanceInitializer = null, 
                GameObject prefab = null) 
            : base(typeCollection, (data, _) => NewInstanceCreator(data, prefab), newInstanceInitializer) { }
    }
}