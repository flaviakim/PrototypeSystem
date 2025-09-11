namespace PrototypeSystem {
    public interface IInstance<TPrototypeData, TInstanceData> 
            where TPrototypeData : IPrototypeData 
            where TInstanceData : IInstanceData {
        public void Initialize(TPrototypeData prototypeData, TInstanceData instanceData);
    }
}