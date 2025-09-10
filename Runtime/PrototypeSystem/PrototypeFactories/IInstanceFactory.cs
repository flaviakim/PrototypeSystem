using System.Collections.Generic;
using PrototypeSystem.PrototypeLoader;

namespace PrototypeSystem {
    public interface IInstanceFactory<out TInstance, TPrototypeData, in TInitializationData>
        where TInstance : IInstance<TPrototypeData>
        where TPrototypeData : IPrototypeData
        where TInitializationData : IInitializationData {
        
        TInstance CreateInstance(TPrototypeData prototype, TInitializationData initializationData);
        TInstance CreateInstance(string idName, TInitializationData initializationData);
        
        // List<TPrototypeData> GetPrototypes();
        // List<string> GetPrototypeNames();
        // void PreloadPrototypes();
        // bool TryGetPrototypeForName(string idName, out TPrototypeData prototype); // TODO maybe find a simple way to make this non-public.
    }
}