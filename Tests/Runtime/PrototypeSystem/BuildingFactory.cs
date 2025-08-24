using System.IO;
using Tests.Runtime.PrototypeSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace PrototypeSystem.Tests.PrototypeSystem {
    
    public class BuildingFactory : MonoInstanceFactory<Building, BuildingPrototypeData, BuildingInitializationData> { }
    
}