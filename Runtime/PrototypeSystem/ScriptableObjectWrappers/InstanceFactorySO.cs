using UnityEngine;

namespace PrototypeSystem.ScriptableObjectWrappers {
    public abstract class InstanceFactorySO<TInstance,TPrototypeData, TInitializationData, TInnerFactory> : ScriptableObject, IInstanceFactory<TInstance, TPrototypeData, TInitializationData>
        where TInstance : IInstance<TPrototypeData>
        where TPrototypeData : IPrototypeData
        where TInitializationData : IInitializationData
        where TInnerFactory : IInstanceFactory<TInstance, TPrototypeData, TInitializationData>
    {
        
        // [SerializeField] private PrototypeLoadingMethod loadingMethod = PrototypeLoadingMethod.ScriptableObject;
        [SerializeField] private PrototypeLoaderSO<TPrototypeData> prototypeLoader;
        
        private TInnerFactory _innerFactory;
        protected TInnerFactory InnerFactory => _innerFactory ??= CreateInnerFactory();
        
        protected virtual PrototypeCollection<TPrototypeData> CreatePrototypeCollection() {
            var prototypeCollection = new PrototypeCollection<TPrototypeData>(prototypeLoader);
            return prototypeCollection;
        }
        protected abstract TInnerFactory CreateInnerFactory();


        public virtual TInstance CreateInstance(TPrototypeData prototype, TInitializationData initializationData) {
            return InnerFactory.CreateInstance(prototype, initializationData);
        }

        public virtual TInstance CreateInstance(string idName, TInitializationData initializationData) {
            return InnerFactory.CreateInstance(idName, initializationData);
        }
    }
}