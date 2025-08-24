using DynamicSaveLoad;

namespace PrototypeSystem {
    public abstract class InstanceFactoryJson<TInstance, TPrototypeData, TInitializationData> : InstanceFactoryBase<TInstance, TPrototypeData, TInitializationData> 
        where TInstance : Instance<TPrototypeData, TInitializationData>
        where TPrototypeData : IPrototypeData
        where TInitializationData : IInitializationData {
        protected override IPrototypeCollection<TPrototypeData> PrototypeCollection { get; }

        protected InstanceFactoryJson(string directoryPath, DynamicAssetManager assets) {
            PrototypeCollection = new JsonPrototypeCollection<TPrototypeData>(directoryPath, assets);
        }
    }
}