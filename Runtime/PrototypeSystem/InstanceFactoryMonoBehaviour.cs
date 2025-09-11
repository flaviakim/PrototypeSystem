using UnityEngine;

namespace PrototypeSystem {
    public class InstanceFactoryMonoBehaviour<TInstance, TPrototypeData, TInstanceData> : InstanceFactory<TInstance,
        TPrototypeData, TInstanceData>
        where TInstance : MonoBehaviour, IInstance<TPrototypeData, TInstanceData>
        where TPrototypeData : IPrototypeData
        where TInstanceData : IInstanceData {
        private static TInstance NewInstanceCreator() {
            var go = new GameObject(typeof(TInstance).Name);
            return go.AddComponent<TInstance>();
        }

        public InstanceFactoryMonoBehaviour(IPrototypeCollection<TPrototypeData> prototypeCollection) : base(prototypeCollection, NewInstanceCreator) {
        }
    }
}