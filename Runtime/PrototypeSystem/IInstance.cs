namespace PrototypeSystem {
    public interface IInstance<TPrototypeData, TInstanceData> 
            where TPrototypeData : IPrototypeData 
            where TInstanceData : IInstanceData {
        
        // Once Unity supports C# 11, add this, to force instances to have a factory.
        // public static abstract InstanceFactory<IInstance<TPrototypeData, TInstanceData>, TPrototypeData, TInstanceData> Factory { get; }
    }
}