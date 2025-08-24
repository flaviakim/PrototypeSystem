using UnityEngine;

namespace PrototypeSystem {
    public interface IInstance<TPrototypeData, in TInitializationData>
            where TPrototypeData : IPrototypeData 
            where TInitializationData : IInitializationData {
        public TPrototypeData PrototypeData { get; }
        // void Initialize(TPrototypeData prototypeData, TInitializationData initializationData);
    }

    public abstract class Instance<TPrototypeData, TInitializationData> : IInstance<TPrototypeData, TInitializationData> where TPrototypeData : IPrototypeData where TInitializationData : IInitializationData {
        // private bool _isInitialized = false;
        public TPrototypeData PrototypeData { get; private set; }

        // protected Instance() { }
        //
        // protected Instance(TPrototypeData prototypeData, TInitializationData initializationData) {
        //     Initialize(prototypeData, initializationData);
        // }
        //
        // public void Initialize(TPrototypeData prototypeData, TInitializationData initializationData) {
        //     if (_isInitialized) {
        //         Debug.LogWarning($"Instance {this} is already initialized");
        //         return;
        //     }
        //     PrototypeData = prototypeData;
        //     Initialize(initializationData);
        //     _isInitialized = true;
        // }
        //
        // protected abstract void Initialize(TInitializationData initializationData);
    }

    // public static class InstanceExtensions {
    //     public static string GetIDName<TPrototypeData, TInitializationData>(
    //         this IInstance<TPrototypeData, TInitializationData> instance)
    //         where TPrototypeData : IPrototypeData
    //         where TInitializationData : IInitializationData {
    //         return instance.PrototypeData.IDName;
    //     }
    // }

}