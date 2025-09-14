namespace TypeObjectSystem {
    public interface IInstance<TType, TInstanceData> 
            where TType : IType 
            where TInstanceData : IInstanceData {
        
        // Once Unity supports C# 11, add this, to force instances to have a factory.
        // public static abstract InstanceFactory<IInstance<TType, TInstanceData>, TType, TInstanceData> Factory { get; }
    }
}