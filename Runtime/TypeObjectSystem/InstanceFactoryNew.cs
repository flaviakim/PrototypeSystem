using System;

namespace TypeObjectSystem {
    public class InstanceFactoryNew<TInstance, TType, TInstanceData> : InstanceFactory<TInstance, TType, TInstanceData>
        where TInstance : IInstance<TType, TInstanceData>, new()
        where TType : IType
        where TInstanceData : IInstanceData {
        
        private static TInstance NewInstanceCreator(TType type, TInstanceData instanceData) {
            return new TInstance();
        }

        public InstanceFactoryNew(ITypeCollection<TType> typeCollection, Action<TInstance, TType, TInstanceData> newInstanceInitializer = null) : base(typeCollection, NewInstanceCreator, newInstanceInitializer) {
        }
    }
}