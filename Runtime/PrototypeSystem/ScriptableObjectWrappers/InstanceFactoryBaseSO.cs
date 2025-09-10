using System;

namespace PrototypeSystem.ScriptableObjectWrappers {
    public abstract class InstanceFactoryBaseSO<TInstance, TPrototypeData, TInitializationData> : InstanceFactorySO<TInstance, TPrototypeData, TInitializationData, InstanceFactoryBaseSO<TInstance, TPrototypeData, TInitializationData>.ActionBasedInstanceFactory> where TInstance : IInstance<TPrototypeData> where TPrototypeData : ScriptableObjectPrototypeData where TInitializationData : IInitializationData {
        protected override ActionBasedInstanceFactory CreateInnerFactory() {
            return new ActionBasedInstanceFactory(CreatePrototypeCollection(), CreateInstance);
        }

        public override TInstance CreateInstance(TPrototypeData prototype, TInitializationData initializationData) {
            return CreateNewInstance(prototype, initializationData);
        }

        protected abstract TInstance CreateNewInstance(TPrototypeData prototype, TInitializationData initializationData);

        public class ActionBasedInstanceFactory : InstanceFactoryBase<TInstance, TPrototypeData, TInitializationData> {
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