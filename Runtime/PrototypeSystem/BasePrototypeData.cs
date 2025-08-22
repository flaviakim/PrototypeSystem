namespace PrototypeSystem {
    public abstract class BasePrototypeData : IPrototypeData {
        public string IDName { get; }

        protected BasePrototypeData(string idName) {
            IDName = idName;
        }
    }
}