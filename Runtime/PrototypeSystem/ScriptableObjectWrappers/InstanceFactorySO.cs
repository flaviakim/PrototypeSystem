using System;
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


        public TInstance CreateInstance(TPrototypeData prototype, TInitializationData initializationData) {
            return InnerFactory.CreateInstance(prototype, initializationData);
        }

        public TInstance CreateInstance(string idName, TInitializationData initializationData) {
            return InnerFactory.CreateInstance(idName, initializationData);
        }
    }

    public class BaseInstanceFactorySO<TInstance, TPrototypeData, TInitializationData> : InstanceFactorySO<TInstance, TPrototypeData, TInitializationData> where TInstance : IInstance<TPrototypeData> where TPrototypeData : ScriptableObjectPrototypeData where TInitializationData : IInitializationData {
        protected override IInstanceFactory<TInstance, TPrototypeData, TInitializationData> CreateInnerFactory() {
            return new ActionBasedInstanceFactory(CreatePrototypeCollection(), CreateInstance);
        }
        
        protected class ActionBasedInstanceFactory : InstanceFactoryBase<TInstance, TPrototypeData, TInitializationData> {
            private readonly System.Func<TPrototypeData, TInitializationData, TInstance> _createInstanceFunc;


            public ActionBasedInstanceFactory(PrototypeCollection<TPrototypeData> prototypeCollection, Func<TPrototypeData, TInitializationData, TInstance> createInstanceFunc) : base(prototypeCollection) {
                _createInstanceFunc = createInstanceFunc;
            }

            public override TInstance CreateInstance(TPrototypeData prototype, TInitializationData initializationData) {
                return _createInstanceFunc(prototype, initializationData);
            }
        }
    }

    public class MonoInstanceFactorySO<TInstance, TPrototypeData, TInitializationData> : InstanceFactorySO<TInstance, TPrototypeData, TInitializationData>
        where TInstance : MonoInstance<TPrototypeData, TInitializationData>
        where TPrototypeData : ScriptableObjectPrototypeData
        where TInitializationData : IInitializationData {
        
        [SerializeField] private GameObject defaultPrefab;

        protected override IInstanceFactory<TInstance, TPrototypeData, TInitializationData> CreateInnerFactory() {
            return new MonoInstanceFactory<TInstance, TPrototypeData, TInitializationData>(CreatePrototypeCollection(), defaultPrefab);
        }
        
    }
    
}