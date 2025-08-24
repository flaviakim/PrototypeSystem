namespace PrototypeSystem {
    public interface IInstance<TPrototypeData, in TInitializationData>
            where TPrototypeData : IPrototypeData 
            where TInitializationData : IInitializationData {
        // public string GetIDName() {
        //     return PrototypeData.IDName;
        // }
        public TPrototypeData PrototypeData { get; }
        void Initialize(TPrototypeData prototypeData, TInitializationData initializationData);
    }

    public abstract class Instance<TPrototypeData, TInitializationData> : IInstance<TPrototypeData, TInitializationData> where TPrototypeData : IPrototypeData where TInitializationData : IInitializationData {
        public TPrototypeData PrototypeData { get; private set; }
        public virtual void Initialize(TPrototypeData prototypeData, TInitializationData initializationData) {
            PrototypeData = prototypeData;
            Initialize(initializationData);
        }

        protected abstract void Initialize(TInitializationData initializationData);
    }

    public static class InstanceExtensions {
        public static string GetIDName<TPrototypeData, TInitializationData>(
            this IInstance<TPrototypeData, TInitializationData> instance)
            where TPrototypeData : IPrototypeData
            where TInitializationData : IInitializationData
            => instance.PrototypeData.IDName;
    }

}