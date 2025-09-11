namespace PrototypeSystem {
    public class InstanceFactoryNew<TInstance, TPrototypeData, TInstanceData> : InstanceFactory<TInstance, TPrototypeData, TInstanceData>
        where TInstance : IInstance<TPrototypeData, TInstanceData>, new()
        where TPrototypeData : IPrototypeData
        where TInstanceData : IInstanceData {
        private static TInstance NewInstanceCreator() {
            return new TInstance();
        }

        public InstanceFactoryNew(IPrototypeCollection<TPrototypeData> prototypeCollection) : base(prototypeCollection, NewInstanceCreator) {
        }
    }
}