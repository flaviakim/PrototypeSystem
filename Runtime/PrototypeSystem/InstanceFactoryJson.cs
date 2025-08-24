using DynamicSaveLoad;

namespace PrototypeSystem {
    public abstract class InstanceFactoryJson<TInstance, TPrototypeData, TInitializationData> : InstanceFactoryBase<TInstance, TPrototypeData, TInitializationData> 
        where TInstance : IInstance<TPrototypeData, TInitializationData>, new()
        where TPrototypeData : IPrototypeData
        where TInitializationData : IInitializationData {
        protected override IPrototypeCollection<TPrototypeData> PrototypeCollection { get; }

        protected InstanceFactoryJson(string directoryPath, DynamicAssetManager assets) {
            PrototypeCollection = new JsonPrototypeCollection<TPrototypeData>(directoryPath, assets);
        }

        public override TInstance CreateInstance(TPrototypeData prototype, TInitializationData initializationData) {
            var instance = new TInstance();
            instance.Initialize(prototype, initializationData);
            return instance;
        }
    }
}