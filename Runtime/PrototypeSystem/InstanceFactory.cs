using System.Collections.Generic;

namespace PrototypeSystem {
    public abstract class InstanceFactory<TInstance, TPrototypeData, TFactory>
        where TInstance : IInstance<TPrototypeData>
        where TPrototypeData : IPrototypeData
        where TFactory : InstanceFactory<TInstance, TPrototypeData, TFactory>, new() {


        // Don't set this, as there can be different arguments for different types of prototypes
        // public abstract TInstance CreateInstance(TPrototypeData prototype);

        protected abstract IPrototypeCollection<TPrototypeData> PrototypeCollection { get; }

        protected bool TryGetPrototypeForName(string idName, out TPrototypeData prototype) {
            return PrototypeCollection.TryGetPrototypeForName(idName, out prototype);
        }

        public List<TPrototypeData> GetPrototypes() {
            return PrototypeCollection.GetPrototypes();
        }

        public List<string> GetPrototypeNames() {
            return PrototypeCollection.GetPrototypeNames();
        }

        public void PreloadPrototypes() {
            PrototypeCollection.PreloadPrototypes();
        }

    }
}