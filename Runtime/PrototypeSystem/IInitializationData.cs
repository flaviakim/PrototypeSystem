namespace PrototypeSystem {
    /// <summary>
    /// Data that where the initial value is specific to every instance.
    ///
    /// Data where the actual data varies, but the initial one is always the same should be initialized from the PrototypeData.
    ///
    /// For example a position if a building should be in IInitializationData,
    /// while the start health of a creature should be set from the max health from the PrototypeData
    /// – unless the creatures should be able to spawn with less than 100% health, then it should be in IInitializationData.
    /// </summary>
    public interface IInitializationData {
        /// <summary>
        /// Ease of use implementation when an IInstance does not need any specific InitializationData that is not already in the prototype. 
        /// </summary>
        public static IInitializationData Empty => new EmptyInitializationData();
        
        public class EmptyInitializationData : IInitializationData { }
    }
    
    
}