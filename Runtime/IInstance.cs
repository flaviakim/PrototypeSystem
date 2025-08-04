namespace PrototypeSystem {
    public interface IInstance<out TData> where TData : PrototypeData {
        public string IDName { get; }
    }
}