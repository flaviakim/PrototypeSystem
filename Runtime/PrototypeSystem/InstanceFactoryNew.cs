using System;

namespace PrototypeSystem {
    public class InstanceFactoryNew<TInstance, TPrototypeData, TInstanceData> : InstanceFactory<TInstance, TPrototypeData, TInstanceData>
        where TInstance : IInstance<TPrototypeData, TInstanceData>, new()
        where TPrototypeData : IPrototypeData
        where TInstanceData : IInstanceData {
        
        private static TInstance NewInstanceCreator(TPrototypeData prototypeData, TInstanceData instanceData) {
            return new TInstance();
        }

        public InstanceFactoryNew(IPrototypeCollection<TPrototypeData> prototypeCollection, Action<TInstance, TPrototypeData, TInstanceData> newInstanceInitializer = null) : base(prototypeCollection, NewInstanceCreator, newInstanceInitializer) {
        }
    }
}