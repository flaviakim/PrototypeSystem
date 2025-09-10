using System;

namespace PrototypeSystem.ScriptableObjectWrappers {
    public abstract class InstanceFactoryBaseSO<TInstance, TPrototypeData, TInitializationData> : InstanceFactorySO<TInstance, TPrototypeData, TInitializationData> where TInstance : IInstance<TPrototypeData> where TPrototypeData : ScriptableObjectPrototypeData where TInitializationData : IInitializationData {
        protected override IInstanceFactory<TInstance, TPrototypeData, TInitializationData> CreateInnerFactory() {
            return new ActionBasedInstanceFactory(CreatePrototypeCollection(), CreateInstance);
        }

        public override TInstance CreateInstance(TPrototypeData prototype, TInitializationData initializationData) {
            return CreateNewInstance(prototype, initializationData);
        }

        protected abstract TInstance CreateNewInstance(TPrototypeData prototype, TInitializationData initializationData);

        private class ActionBasedInstanceFactory : InstanceFactoryBase<TInstance, TPrototypeData, TInitializationData> {
            private readonly Func<TPrototypeData, TInitializationData, TInstance> _createInstanceFunc;


            public ActionBasedInstanceFactory(PrototypeCollection<TPrototypeData> prototypeCollection, Func<TPrototypeData, TInitializationData, TInstance> createInstanceFunc) : base(prototypeCollection) {
                _createInstanceFunc = createInstanceFunc;
            }

            public override TInstance CreateInstance(TPrototypeData prototype, TInitializationData initializationData) {
                return _createInstanceFunc(prototype, initializationData);
            }
        }
    }
}