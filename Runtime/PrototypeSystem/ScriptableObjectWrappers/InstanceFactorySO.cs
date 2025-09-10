using UnityEngine;

namespace PrototypeSystem.ScriptableObjectWrappers {
    public abstract class InstanceFactorySO<TInstance,TPrototypeData, TInitializationData> : ScriptableObject, IInstanceFactory<TInstance, TPrototypeData, TInitializationData>
        where TInstance : IInstance<TPrototypeData>
        where TPrototypeData : ScriptableObjectPrototypeData
        where TInitializationData : IInitializationData {
        
        [SerializeField] private PrototypeLoaderSO<TPrototypeData> prototypeLoader;
        
        private IInstanceFactory<TInstance, TPrototypeData, TInitializationData> _innerFactory;
        private IInstanceFactory<TInstance, TPrototypeData, TInitializationData> InnerFactory => _innerFactory ??= CreateInnerFactory();
        
        protected PrototypeCollection<TPrototypeData> CreatePrototypeCollection() {
            var prototypeCollection = new PrototypeCollection<TPrototypeData>(prototypeLoader);
            return prototypeCollection;
        }
        protected abstract IInstanceFactory<TInstance, TPrototypeData, TInitializationData> CreateInnerFactory();


        public virtual TInstance CreateInstance(TPrototypeData prototype, TInitializationData initializationData) {
            return InnerFactory.CreateInstance(prototype, initializationData);
        }

        public virtual TInstance CreateInstance(string idName, TInitializationData initializationData) {
            return InnerFactory.CreateInstance(idName, initializationData);
        }
    }
}