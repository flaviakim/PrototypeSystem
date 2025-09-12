using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace PrototypeSystem {
    public class InstanceFactoryMonoBehaviour<TInstance, TPrototypeData, TInstanceData> : InstanceFactory<TInstance,
        TPrototypeData, TInstanceData>
        where TInstance : MonoBehaviour, IInstance<TPrototypeData, TInstanceData>
        where TPrototypeData : IPrototypeData
        where TInstanceData : IInstanceData {
        private static TInstance NewInstanceCreator(TPrototypeData data, GameObject prefab = null) {
            const string defaultGoName = "New Game Object";
            GameObject go = prefab == null ? new GameObject(typeof(TInstance).Name) : Object.Instantiate(prefab);
            go.name = $"{(string.IsNullOrWhiteSpace(go.name) || go.name.Equals(defaultGoName) ? "" : $"{go.name}_")}{typeof(TInstance).Name}_{data.IDName}";
            return go.AddComponent<TInstance>();
        }

        public InstanceFactoryMonoBehaviour(
                IPrototypeCollection<TPrototypeData> prototypeCollection,
                Action<TInstance, TPrototypeData, TInstanceData> newInstanceInitializer = null, 
                GameObject prefab = null) 
            : base(prototypeCollection, (data, _) => NewInstanceCreator(data, prefab), newInstanceInitializer) { }
    }
}